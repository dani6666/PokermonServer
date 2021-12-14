using Pokermon.Core.Extensions;
using Pokermon.Core.Interfaces.Repositories;
using Pokermon.Core.Interfaces.Services;
using Pokermon.Core.Model;
using Pokermon.Core.Model.Entities;
using Pokermon.Core.Model.Enums;
using Pokermon.Core.Model.Responses;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Pokermon.Core.Services
{
    public class GameService : IGameService
    {
        private readonly ITablesRepository _tablesRepository;
        private readonly IGamesRepository _gamesRepository;

        private const int StartingBet = 10;

        public GameService(ITablesRepository tablesRepository, IGamesRepository gamesRepository)
        {
            _tablesRepository = tablesRepository;
            _gamesRepository = gamesRepository;
        }

        public ResponseResult<GameStateResponse> GetGame(int gameId, Guid playerId)
        {
            if (!_tablesRepository.PlayerExists(gameId, playerId))
                return new ResponseResult<GameStateResponse>(OperationError.PlayerDoesNotExist);

            var gameState = _gamesRepository.GetGame(gameId);

            if (gameState == null)
                return new ResponseResult<GameStateResponse>(OperationError.TableDoesNotExist);

            var gameResponse = new GameStateResponse(gameState);

            var thisPlayer = gameState.Players.First(p => p?.Id == playerId);
            gameResponse.PocketCards = thisPlayer.PocketCards?.ConvertAll<int>(c => c);
            gameResponse.Players = gameState.Players.Select(p => p != null ? new PlayerResponse(p) : null).ToArray();
            if (gameState.IsEndOfHand)
            {
                for (var i = 0; i < 8; i++)
                {
                    if (gameResponse.Players[i] != null)
                        gameResponse.Players[i].PocketCards = gameState.Players[i].PocketCards?.ConvertAll<int>(c => c);
                }
            }

            if (gameResponse.PocketCards != null)
            {
                gameResponse.CashToCall = gameState.HighestBet - thisPlayer.CurrentBet;
                gameResponse.CanRaise = thisPlayer.CanRaise;
            }

            return new ResponseResult<GameStateResponse>(gameResponse);
        }

        public OperationError PlaceBet(int id, Guid playerId, int bet)
        {
            if (!_tablesRepository.PlayerExists(id, playerId))
                return OperationError.PlayerDoesNotExist;

            var game = _gamesRepository.GetGame(id);

            if (game == null)
                return OperationError.TableDoesNotExist;

            var player = game.Players[game.CurrentPlayerPosition];

            if (player.Id != playerId)
                return OperationError.OtherPlayersTurn;

            if (player.CurrentCash < bet)
                return OperationError.NotEnoughCashToBet;

            if (player.CurrentCash == bet)
                player.IsAllIn = true;

            if (player.CurrentBet + bet < game.HighestBet && !player.IsAllIn)
                return OperationError.BetTooLow;

            if (player.CurrentBet + bet > game.HighestBet)
            {
                if (player.CanRaise)
                {
                    player.CanRaise = false;
                    game.LastRaisingPlayerPosition = game.CurrentPlayerPosition;
                }
                else
                {
                    return OperationError.PlayerCannotRaise;
                }
            }

            player.CurrentBet += bet;
            player.TotalBet += bet;
            player.CurrentCash -= bet;

            if (game.HighestBet < player.CurrentBet)
                game.HighestBet = player.CurrentBet;

            PassTurn(game);

            return OperationError.NoError;
        }

        public OperationError Check(int id, Guid playerId)
        {
            if (!_tablesRepository.PlayerExists(id, playerId))
                return OperationError.PlayerDoesNotExist;

            var game = _gamesRepository.GetGame(id);

            if (game == null)
                return OperationError.TableDoesNotExist;

            var player = game.Players[game.CurrentPlayerPosition];

            if (player.Id != playerId)
                return OperationError.OtherPlayersTurn;

            if (player.CurrentBet < game.HighestBet)
                return OperationError.BetTooLow;

            PassTurn(game);

            return OperationError.NoError;
        }

        public OperationError Fold(int id, Guid playerId, bool force = false)
        {
            if (!_tablesRepository.PlayerExists(id, playerId))
                return OperationError.PlayerDoesNotExist;

            var game = _gamesRepository.GetGame(id);

            if (game == null)
                return OperationError.TableDoesNotExist;

            var player = game.Players[game.CurrentPlayerPosition];
            if (player.Id != playerId && !force)
                return OperationError.OtherPlayersTurn;

            player.PocketCards = null;

            if (game.Players.Count(p => p?.PocketCards != null) == 1)
                EndRound(game, true);
            else if (!force || player.Id == playerId)
                PassTurn(game);

            return OperationError.NoError;
        }

        public void RemovePlayer(int id, Guid playerId)
        {
            var game = _gamesRepository.GetGame(id);
            for (var i = 0; i < 8; i++)
            {
                if (game.Players[i] == null)
                    continue;

                if (game.Players[i].Id == playerId)
                    game.Players[i] = null;
            }
        }

        public void CreateNewGame(int id)
        {
            _gamesRepository.AddGame(id, new GameState(id));
        }

        public void AddPlayer(int id, int position, Guid playerId)
        {
            var game = _gamesRepository.GetGame(id);
            game.Players[position] = new Player(playerId);

            if (game.IsWaitingForPlayers)
                StartNewHand(game);
        }

        public void RestartGames()
        {
            foreach (var game in _gamesRepository.GetGamesToRestart(DateTime.Now.AddSeconds(-20)))
                StartNewHand(game);
        }

        private static void PassTurn(GameState game)
        {
            var nextPosition = game.CurrentPlayerPosition + 1;

            for (var i = 0; i < 2; i++)
            {
                for (; nextPosition < 8; nextPosition++)
                {
                    if (nextPosition == game.LastRaisingPlayerPosition)
                    {
                        if (game.Players.Count(p => p?.PocketCards != null && !p.IsAllIn) > 1)
                            EndRound(game);
                        else
                            JumpToEndHand(game);

                        return;
                    }

                    var nextPlayer = game.Players[nextPosition];

                    if (nextPlayer?.PocketCards != null && !nextPlayer.IsAllIn)
                    {
                        game.CurrentPlayerPosition = nextPosition;
                        return;
                    }
                }

                nextPosition = 0;
            }

        }

        private static void JumpToEndHand(GameState game)
        {
            while (game.TableCards.Count < 5)
            {
                EndRound(game);
            }

            EndRound(game);
        }

        private static void EndRound(GameState game, bool allPlayersFolded = false)
        {
            foreach (var player in game.Players.Where(p => p != null))
            {
                game.PotValue += player.CurrentBet;
                player.CurrentBet = 0;
                player.CanRaise = true;
            }

            if (game.TableCards.Count == 5 || allPlayersFolded)
            {
                EndHand(game);
            }
            else
            {
                var cardsToAdd = game.TableCards.Count == 0 ? 3 : 1;
                for (int i = 0; i < cardsToAdd; i++)
                {
                    game.TableCards.Add(game.Deck[0]);
                    game.Deck.RemoveAt(0);
                }

                game.HighestBet = 0;
                game.CurrentPlayerPosition = Array.FindIndex(game.Players, p => p?.PocketCards != null);
                game.LastRaisingPlayerPosition = game.CurrentPlayerPosition;
            }
        }

        private static void EndHand(GameState game)
        {
            var winningLayers = DetermineWinners(game);

            game.IsEndOfHand = true;
            game.HandEndTime = DateTime.Now;

            var previousWinners = new List<Player>();
            foreach (var winners in winningLayers)
            {
                previousWinners.AddRange(winners);
                var totalWinnerLeft = winners.Count;
                foreach (var winner in winners.OrderBy(p => (p.IsAllIn, p.TotalBet)))
                {
                    winner.WonCash = 0;
                    foreach (var player in game.Players.Where(p => p != null && p.TotalBet > 0))
                    {
                        if (previousWinners.Contains(player))
                            continue;

                        var maxWin = Math.Min(winner.TotalBet, player.TotalBet / totalWinnerLeft);

                        winner.WonCash += maxWin;
                        player.TotalBet -= maxWin;
                        game.PotValue -= maxWin;
                    }

                    totalWinnerLeft--;
                }

                if (winners.Any(p => !p.IsAllIn))
                    break;
            }

            foreach (var player in game.Players.Where(p => p != null))
            {
                player.WonCash += player.TotalBet;
                game.PotValue -= player.TotalBet;
                player.TotalBet = 0;
            }
        }

        private static IEnumerable<List<Player>> DetermineWinners(GameState gameState)
        {
            var activePlayers = gameState.Players.Where(p => p?.PocketCards != null).ToList();

            if (activePlayers.Count == 1)
                return new List<List<Player>> { activePlayers };

            return gameState.Players
                .Where(p => p?.PocketCards != null)
                .Select(p => (Player: p, Rank: gameState.TableCards
                    .Concat(p.PocketCards)
                    .ToList()
                    .GetAllPossibleHands()
                    .Max(h => h.CalculateRank())))
                .GroupBy(p => p.Rank)
                .OrderByDescending(g => g.Key)
                .Select(g => g.Select(p => p.Player).ToList());
        }


        private static void StartNewHand(GameState game)
        {
            foreach (var player in game.Players.Where(p => p?.WonCash != null))
            {
                player.CurrentCash += player.WonCash!.Value;
                player.WonCash = 0;
                player.IsAllIn = false;
            }

            game.IsEndOfHand = false;

            if (game.Players.Count(p => p != null && p.CurrentCash > 0) < 2)
            {
                game.IsWaitingForPlayers = true;
                return;
            }

            game.IsWaitingForPlayers = false;

            game.Deck.Clear();
            game.TableCards.Clear();

            foreach (var cardColor in Enum.GetValues<CardColor>())
            {
                for (var cardValue = 0; cardValue < 13; cardValue++)
                {
                    game.Deck.Add(new Card(cardColor, cardValue));
                }
            }
            game.Deck.Shuffle();

            foreach (var player in game.Players.Where(p => p != null))
            {
                if (player.CurrentCash == 0)
                {
                    player.PocketCards = null;
                    continue;
                }

                player.PocketCards = new List<Card>();
                for (var i = 0; i < 2; i++)
                {
                    player.PocketCards.Add(game.Deck[0]);

                    game.Deck.RemoveAt(0);
                }

                player.PocketCards.Sort();

                if (player.WonCash == null)
                    continue;

                player.CurrentCash += player.WonCash.Value;
                player.WonCash = null;
            }

            game.HighestBet = StartingBet;
            game.CurrentPlayerPosition = Array.FindIndex(game.Players, p => p?.PocketCards != null);
            game.LastRaisingPlayerPosition = game.CurrentPlayerPosition;
        }
    }
}

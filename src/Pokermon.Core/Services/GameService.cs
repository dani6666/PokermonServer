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

            var thisPlayer = gameState.Players.First(p => p.Id == playerId);
            var isPlaying = gameResponse.PocketCards != null;
            gameResponse.PocketCards = thisPlayer.PocketCards.ConvertAll<int>(c => c);
            gameResponse.Players = gameState.Players.Select(p => p != null ? new PlayerResponse(p) : null).ToArray();
            if (gameState.IsEndOfHand)
            {
                for (var i = 0; i < 8; i++)
                {
                    gameResponse.Players[i].PocketCards = gameState.Players[i].PocketCards.ConvertAll<int>(c => c);
                }
            }

            if (isPlaying)
            {
                gameResponse.CashToCall = gameState.HighestBet - thisPlayer.CurrentBet;
                gameResponse.CanRaise = thisPlayer.CanRaise;
            }

            return new ResponseResult<GameStateResponse>(gameResponse);
        }

        public void CreateNewGame(int id)
        {
            _gamesRepository.AddGame(id, new GameState(id));
        }

        public void AddPlayer(int id, int position, Guid playerId)
        {
            var game = _gamesRepository.GetGame(id);
            game.Players[position] = new Player(playerId);

            if (game.Players.Count(p => p != null) == 2)
                StartNewHand(game);
        }

        private static void StartNewHand(GameState game)
        {
            game.IsEndOfHand = false;

            game.Deck.Clear();

            foreach (var cardColor in Enum.GetValues<CardColor>())
            {
                for (var cardValue = 2; cardValue < 15; cardValue++)
                {
                    game.Deck.Add(new Card(cardColor, cardValue));
                }
            }
            game.Deck.Shuffle();

            foreach (var player in game.Players)
            {
                if (player == null)
                    continue;

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

            game.CurrentPlayerPosition = Array.FindIndex(game.Players, p => p != null);
            game.HighestBet = StartingBet;
        }
    }
}

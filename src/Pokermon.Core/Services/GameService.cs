using Pokermon.Core.Extensions;
using Pokermon.Core.Interfaces.Repositories;
using Pokermon.Core.Model;
using Pokermon.Core.Model.Entities;
using Pokermon.Core.Model.Enums;
using Pokermon.Core.Model.Responses;
using System;

namespace Pokermon.Core.Services
{
    public class GameService
    {
        private readonly ITablesRepository _tablesRepository;
        private readonly IGamesRepository _gamesRepository;

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

            return gameState == null ?
                new ResponseResult<GameStateResponse>(OperationError.TableDoesNotExist) :
                new ResponseResult<GameStateResponse>(new GameStateResponse(gameState));
        }

        public void CreateNewGame(int id, Guid firstPlayerId)
        {
            var newGame = new GameState(id);

            newGame.Players[0] = new Player(firstPlayerId);
        }

        public void StartNewHand(int id, Guid firstPlayerId)
        {
            var game = _gamesRepository.GetGame(id);

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
                if (player?.WonCash == null)
                    continue;

                player.CurrentCash += player.WonCash.Value;
                player.WonCash = null;
            }


            foreach (var player in game.Players)
            {
                if (player == null)
                    continue;

                for (var i = 0; i < 2; i++)
                {
                    player.PocketCards.Add(game.Deck[0]);

                    game.Deck.RemoveAt(0);
                }

                player.PocketCards.Sort();
            }

            game.CurrentPlayerPosition = Array.FindIndex(game.Players, p => p != null);
        }
    }
}

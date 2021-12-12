using Microsoft.Extensions.Hosting;
using Pokermon.Core.Interfaces.Services;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Pokermon.Workers
{
    public class GameRestartWorker : BackgroundService
    {
        private readonly IGameService _gameService;
        public GameRestartWorker(IGameService gameService)
        {
            _gameService = gameService;
        }
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                _gameService.RestartGames();

                await Task.Delay(TimeSpan.FromSeconds(3), stoppingToken);
            }
        }
    }
}

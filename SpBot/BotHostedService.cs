using Microsoft.Extensions.Hosting;
using SpBot.Core;
using System.Threading;
using System.Threading.Tasks;

namespace SpBot
{
    internal sealed class BotHostedService : IHostedService
    {
        private Bot _spBot;

        public BotHostedService(Bot bot)
        {
            _spBot = bot;
        }
        public async Task StartAsync(CancellationToken cancellationToken)
        {
            await _spBot.RunAsync();
        }

        public async Task StopAsync(CancellationToken cancellationToken)
        {
            await _spBot.StopAsync();
        }
    }
}

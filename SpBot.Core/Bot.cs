using DSharpPlus;
using DSharpPlus.CommandsNext;
using DSharpPlus.Entities;
using DSharpPlus.EventArgs;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using SpBot.Core.Commands;
using System;
using System.Threading.Tasks;

namespace SpBot.Core
{
    //Этот код принадлежит Алексею Уланову a.k.a. Ultra_Rabbit'у
    public sealed class Bot
    {
        public DiscordClient Client {get; private set;}
        public CommandsNextExtension Commands {get; private set;}


        private readonly DataScheduler _scheduler;
        public Bot(IOptions<BotConfiguration> config, IServiceProvider services, DataScheduler scheduler)
        {
            _scheduler = scheduler;
            string json = string.Empty;

            var dsconfig = new DiscordConfiguration
            {
                Token = config.Value.Token,
                TokenType = TokenType.Bot,
                AutoReconnect = true,
                MinimumLogLevel = LogLevel.Debug
            };

            Client = new DiscordClient(dsconfig);
            Client.Ready += OnReady;

            var commandsConfig = new CommandsNextConfiguration
            {
                StringPrefixes = new string[] {config.Value.Prefix},
                EnableMentionPrefix = true,
                EnableDms = false,
                DmHelp = true,
                Services = services
            };

            Commands = Client.UseCommandsNext(commandsConfig);
            Commands.RegisterCommands<GeneralCommands>();
        }
        public async Task RunAsync()
        {
            await Client.ConnectAsync(new DiscordActivity("lmao", ActivityType.Watching));
        }

        public async Task StopAsync()
        {
            await Client.DisconnectAsync();
            Client.Dispose();
        }

        public async Task OnReady(DiscordClient client, ReadyEventArgs e)
        {
            await _scheduler.Start();
        }
    }
}

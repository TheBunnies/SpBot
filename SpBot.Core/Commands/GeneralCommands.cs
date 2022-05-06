using DSharpPlus;
using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using DSharpPlus.Entities;
using SPapi.NET;
using System;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace SpBot.Core.Commands
{
    internal sealed class GeneralCommands : BaseCommandModule
    {
        private SpClient _spClient;
        private DiscordWebhookClient _webhookClient;
        public GeneralCommands(SpClient spClient)
        {
            _spClient = spClient;
            _webhookClient = new DiscordWebhookClient() {
                Username = "Nanahira"
            };
        }
        [Command("ping")]
        public async Task PingAsync(CommandContext ctx)
        {
            await ctx.RespondAsync($"Pong! {ctx.Client.Ping}");
        }

        [Command("weather")]
        public async Task GetWeatherAsync(CommandContext ctx)
        {
            var weather = await _spClient.GetWeatherAsync();
            await ctx.RespondAsync($"Погода: {weather}");
        }

        [Command("players")]
        public async Task GetPlayersAsync(CommandContext ctx)
        {
            var players = await _spClient.GetOnlinePlayersAsync();
            var filteredPlayers = players.ServerPlayers.Select((x, i) => $"{i + 1}. {x.Nickname}");
            var embedBuilder = new DiscordEmbedBuilder();
            embedBuilder.Color = DiscordColor.HotPink;
            embedBuilder.WithAuthor("Nanahira", url:null, ctx.Client.CurrentUser.AvatarUrl); 
            embedBuilder.Description = String.Join("\n", filteredPlayers);

            await ctx.RespondAsync(embed: embedBuilder);
        }

        [Command("record")]
        public async Task RecordChatMessagesAsync(CommandContext ctx)
        {
            var webhooks = await ctx.Channel.GetWebhooksAsync();
            DiscordWebhook newWebhook = null;

            if (webhooks.FirstOrDefault(x => x.Name.Contains("Nanahira's webhook")) == null)
                newWebhook = await ctx.Channel.CreateWebhookAsync("Nanahira's webhook");
            else 
                newWebhook = webhooks.FirstOrDefault(x => x.Name.Contains("Nanahira's webhook"));
            
            var webhook = _webhookClient.GetRegisteredWebhook(webhooks.FirstOrDefault(x => x.Name.Contains("Nanahira's webhook")).Id);
            if (webhook == null)
            {
                _webhookClient.AddWebhook(newWebhook);
                await ctx.RespondAsync("Включено");
            }
            else
            {
                 _webhookClient.RemoveWebhook(webhook.Id);
                 await ctx.RespondAsync("Выключено");
            }

            if (_spClient.MessageAdd == null)
            {
                _spClient.MessageAdd += async (sender, e) => 
                {
                    var webhookBuilder = new DiscordWebhookBuilder 
                    { 
                    Content = e.Content,
                    IsTTS = false,
                    Username = e.Author,
                    AvatarUrl = GetPlayerAvatar(e.Uuid),
                    };
                    
                    await _webhookClient.BroadcastMessageAsync(webhookBuilder);
                };
            }
        }
        private string GetPlayerAvatar(string uuid)
        {
            return String.Format("https://minotar.net/helm/{0}/300.png", uuid);
        }
    }
}

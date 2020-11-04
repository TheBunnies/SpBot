using DSharpPlus.Entities;
using SPapi.NET;
using System.Threading.Tasks;

namespace SpBot.Core
{
    //Этот код принадлежит Алексею Уланову a.k.a. Ultra_Rabbit'у
    public sealed class ServerInfoNotifier : IServerInfoNotifier
    {
        private SpClient _spClient;
        private Bot _bot;
        private uint _status = 0;
        
        public ServerInfoNotifier(SpClient spClient, Bot bot)
        {
            _spClient = spClient;
            _bot = bot;
        } 
        public async Task ChangeStatus()
        {
            switch(_status)
            {
                case 0:
                    var weather = await _spClient.GetWeatherAsync();
                    await _bot.Client.UpdateStatusAsync(new DiscordActivity($"Погода на сервере: {weather}"));
                    _status = 1;
                 break;
                case 1: 
                    var daytime = await _spClient.GetDayTimeAsync();
                    await _bot.Client.UpdateStatusAsync(new DiscordActivity($"Время суток на сервере: {daytime.DayTime}"));
                    _status = 2;
                break;
                case 2:
                    var playersDetails = await _spClient.GetOnlinePlayersAsync();
                    await _bot.Client.UpdateStatusAsync(new DiscordActivity($"Макс. кол-во игроков {playersDetails.Max} | Сейчас играют: {playersDetails.Count}"));
                    _status = 0;
                break;
            }
        }
    }
}

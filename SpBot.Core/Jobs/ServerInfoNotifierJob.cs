using Quartz;
using System.Threading.Tasks;

namespace SpBot.Core.Jobs
{
    [DisallowConcurrentExecution]
    public class ServerInfoNotifierJob : IJob
    {
        private readonly IServerInfoNotifier _weatherNotifier;
        public ServerInfoNotifierJob(IServerInfoNotifier weatherNotifier)
        {
            _weatherNotifier = weatherNotifier;
        } 
        public async Task Execute(IJobExecutionContext context)
        {
            await _weatherNotifier.ChangeStatus();
        }
    }
}

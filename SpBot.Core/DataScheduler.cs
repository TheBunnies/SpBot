using Quartz;
using Quartz.Impl;
using SpBot.Core.Jobs;
using System.Threading.Tasks;

namespace SpBot.Core
{
    //Этот код принадлежит Алексею Уланову a.k.a. Ultra_Rabbit'у
    public class DataScheduler
    {
        private readonly JobFactory _jobFactory;
        public DataScheduler(JobFactory jobFactory)
        {
            _jobFactory = jobFactory;
        }
        public async Task Start()
        {
            IScheduler scheduler = await StdSchedulerFactory.GetDefaultScheduler();
            scheduler.JobFactory = _jobFactory;
            await scheduler.Start();
            

            var jobDetail = JobBuilder.Create<ServerInfoNotifierJob>().WithIdentity("WeatherJob", "default").StoreDurably().Build();

            await scheduler.AddJob(jobDetail, true);

            var trigger = TriggerBuilder.Create()
                .WithIdentity("WeatherTrigger", "default")
                .StartNow()
                .ForJob(jobDetail)
                .WithSimpleSchedule(x => x
                .WithIntervalInSeconds(15)
                .RepeatForever())
                .Build();

            await scheduler.ScheduleJob(trigger);
            await scheduler.Start();
        }
    }
}

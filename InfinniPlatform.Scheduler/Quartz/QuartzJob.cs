using System.Threading.Tasks;

using Quartz;

namespace InfinniPlatform.Scheduler.Quartz
{
    /// <summary>
    /// Обработчик заданий Quartz.
    /// </summary>
    internal class QuartzJob : IJob
    {
        /// <summary>
        /// Ключ для доступа к информации о задании.
        /// </summary>
        public const string JobInfoKey = "JobInfo";

        /// <summary>
        /// Ключ для доступа к данным выполнения задания при досрочном выполнении.
        /// </summary>
        public const string TriggerDataKey = "TriggerData";


        public Task Execute(IJobExecutionContext context)
        {
            throw new System.NotImplementedException();
        }
    }
}
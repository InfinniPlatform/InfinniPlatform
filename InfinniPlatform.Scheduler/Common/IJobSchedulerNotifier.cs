using System.Threading.Tasks;

namespace InfinniPlatform.Scheduler.Common
{
    /// <summary>
    /// Шина сообщений для взаимодействия планировщиков заданий в кластере.
    /// </summary>
    internal interface IJobSchedulerMessageQueue
    {
        /// <summary>
        /// Публикует событие о необходимости обработать задание.
        /// </summary>
        Task PublishHandleJob(HandleJobMessage message);
    }
}
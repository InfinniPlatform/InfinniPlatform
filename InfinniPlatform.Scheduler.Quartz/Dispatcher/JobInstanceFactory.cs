using System;

namespace InfinniPlatform.Scheduler.Dispatcher
{
    internal class JobInstanceFactory : IJobInstanceFactory
    {
        public string CreateJobInstance(string jobId, DateTimeOffset scheduledFireTimeUtc)
        {
            // Округляет время срабатывания задания до секунд
            scheduledFireTimeUtc = TruncateToSecond(scheduledFireTimeUtc);

            // Идентификатор экземпляра включает идентификатор задания и время срабатывания
            return $"{jobId}.{scheduledFireTimeUtc:O}";
        }

        private static DateTimeOffset TruncateToSecond(DateTimeOffset dateTime)
        {
            return dateTime.AddTicks(-(dateTime.Ticks % TimeSpan.TicksPerSecond));
        }
    }
}
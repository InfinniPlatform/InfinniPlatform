using System;
using System.Collections.Generic;
using System.Linq;

namespace InfinniPlatform.Scheduler
{
    /// <summary>
    /// Список общих методов расширения на уровне реализации <see cref="InfinniPlatform.Scheduler"/>.
    /// </summary>
    internal static class CommonExtensions
    {
        /// <summary>
        /// Проверяет корректность информации о задании.
        /// </summary>
        /// <param name="jobInfo">Информация о задании.</param>
        /// <exception cref="ArgumentNullException">Значение параметра <paramref name="jobInfo"/> равно <c>null</c>.</exception>
        /// <exception cref="ArgumentException">Значение параметра <paramref name="jobInfo"/> не является экземпляром <see cref="JobInfo"/>.</exception>
        public static JobInfo EnsureJobInfo(IJobInfo jobInfo)
        {
            if (jobInfo == null)
            {
                throw new ArgumentNullException(nameof(jobInfo));
            }

            var jobInfoImpl = jobInfo as JobInfo;

            if (jobInfoImpl == null)
            {
                throw new ArgumentException(nameof(jobInfo));
            }

            return jobInfoImpl;
        }

        /// <summary>
        /// Проверяет корректность списка с информацией о заданиях.
        /// </summary>
        /// <param name="jobInfos">Список с информацией о заданиях.</param>
        /// <exception cref="ArgumentNullException">Значение параметра <paramref name="jobInfos"/> равно <c>null</c> или один из элементов этого списка равен <c>null</c>.</exception>
        /// <exception cref="ArgumentException">Один из элементов списка не является экземпляром <see cref="JobInfo"/>.</exception>
        public static List<JobInfo> EnsureJobInfos(IEnumerable<IJobInfo> jobInfos)
        {
            if (jobInfos == null)
            {
                throw new ArgumentNullException(nameof(jobInfos));
            }

            return jobInfos.Select(EnsureJobInfo).ToList();
        }


        /// <summary>
        /// Проверяет корректность уникального идентификатора задания.
        /// </summary>
        /// <param name="jobId">Уникальный идентификатор задания.</param>
        /// <exception cref="ArgumentNullException">Значение параметра <paramref name="jobId"/> равно <c>null</c> или пустой строке.</exception>
        public static string EnsureJobId(string jobId)
        {
            if (string.IsNullOrEmpty(jobId))
            {
                throw new ArgumentNullException(nameof(jobId));
            }

            return jobId;
        }

        /// <summary>
        /// Проверяет корректность списка уникальных идентификаторов заданий.
        /// </summary>
        /// <param name="jobIds">Список уникальных идентификаторов задания.</param>
        /// <exception cref="ArgumentNullException">Значение параметра <paramref name="jobIds"/> равно <c>null</c> или один из элементов этого списка равен <c>null</c> или пустой строке.</exception>
        public static List<string> EnsureJobIds(IEnumerable<string> jobIds)
        {
            if (jobIds == null)
            {
                throw new ArgumentNullException(nameof(jobIds));
            }

            return jobIds.Select(EnsureJobId).ToList();
        }
    }
}
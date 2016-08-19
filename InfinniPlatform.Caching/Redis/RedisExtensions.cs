using System;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

using InfinniPlatform.Caching.Properties;
using InfinniPlatform.Sdk.Dynamic;

using Sider;

namespace InfinniPlatform.Caching.Redis
{
    internal static class RedisExtensions
    {
        private const string InfoCommandName = "INFO";

        private const int InfoCommandTimeout = 1000;

        private static readonly Regex InfoCommandRegex = new Regex(@"^(?<key>.+?):(?<value>.*?)\s*$", RegexOptions.Compiled | RegexOptions.Multiline);


        /// <summary>
        /// Возвращает информацию о состоянии сервера Redis.
        /// </summary>
        /// <param name="client">Подключение к серверу Redis.</param>
        /// <param name="sections">Категории статистической информации сервера Redis.</param>
        /// <exception cref="TimeoutException"></exception>
        /// <returns>Информация о состоянии сервера Redis.</returns>
        /// <remarks>
        /// К сожалению, используемая библиотека для работы с Redis (Sider) не предоставляет адекватного метода для получения данных о состоянии.
        /// </remarks>
        public static async Task<DynamicWrapper> GetStatusAsync(this IRedisClient<string> client, RedisSectionStatus sections = RedisSectionStatus.Stats | RedisSectionStatus.Clients | RedisSectionStatus.Keyspace)
        {
            var status = new DynamicWrapper();

            foreach (Enum section in Enum.GetValues(typeof(RedisSectionStatus)))
            {
                if (sections.HasFlag(section))
                {
                    var sectionName = section.ToString().ToLower();
                    var sectionStatus = await GetSectionStatusAsync(client, (RedisSectionStatus)section);

                    status[sectionName] = sectionStatus;
                }
            }

            return status;
        }


        private static async Task<DynamicWrapper> GetSectionStatusAsync(IRedisClient<string> client, RedisSectionStatus section)
        {
            var infoCommandTask = Task.Run(() => client.Custom(InfoCommandName,
                w =>
                {
                    var sectionName = section.ToString().ToLower();
                    w.WriteCmdStart(InfoCommandName, 1);
                    w.WriteArg(sectionName);
                },
                r =>
                {
                    var sectionStatus = new DynamicWrapper();

                    var infoCommandResult = r.ReadStrBulk();

                    if (!string.IsNullOrEmpty(infoCommandResult))
                    {
                        foreach (Match item in InfoCommandRegex.Matches(infoCommandResult))
                        {
                            var key = item.Groups["key"].Value;
                            var value = item.Groups["value"].Value;

                            sectionStatus[key] = value;
                        }
                    }

                    return sectionStatus;
                }));

            if (await Task.WhenAny(infoCommandTask, Task.Delay(InfoCommandTimeout)) != infoCommandTask)
            {
                throw new TimeoutException(Resources.RedisInfoCommandCompletedWithTimeout);
            }

            return infoCommandTask.Result;
        }


        /// <summary>
        /// Категории статистической информации сервера Redis.
        /// </summary>
        [Flags]
        public enum RedisSectionStatus
        {
            /// <summary>
            /// Информация с основной статистикой.
            /// </summary>
            Stats = 1,

            /// <summary>
            /// Основная статистика о сервере.
            /// </summary>
            Server = 2,

            /// <summary>
            /// Статистика клиентских соединениях.
            /// </summary>
            Clients = 4,

            /// <summary>
            /// Статистика использования CPU.
            /// </summary>
            Cpu = 8,

            /// <summary>
            /// Статистика о использования памяти.
            /// </summary>
            Memory = 16,

            /// <summary>
            /// Статистика работы кластера.
            /// </summary>
            Cluster = 32,

            /// <summary>
            /// Статистика работы базы данных.
            /// </summary>
            Keyspace = 64,

            /// <summary>
            /// Статистика хранения данных на диске.
            /// </summary>
            Persistence = 128,

            /// <summary>
            /// Информация о репликации данных.
            /// </summary>
            Replication = 256,

            /// <summary>
            /// Статистика выполнения команд.
            /// </summary>
            CommandStats = 512
        }
    }
}
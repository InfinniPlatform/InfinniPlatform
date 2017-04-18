using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using StackExchange.Redis;

namespace InfinniPlatform.Cache.Redis
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
        public static async Task<IGrouping<string, KeyValuePair<string, string>>[]> GetStatusAsync(this ConnectionMultiplexer client, RedisSectionStatus sections = RedisSectionStatus.Stats | RedisSectionStatus.Clients | RedisSectionStatus.Keyspace)
        {
            //TODO Multiple server status.
            var endPoint = client.GetEndPoints().First();

            return await client.GetServer(endPoint).InfoAsync();
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
using System.Linq;
using System.Threading.Tasks;

using InfinniPlatform.Dynamic;

using StackExchange.Redis;

namespace InfinniPlatform.Cache
{
    internal static class RedisExtensions
    {
        /// <summary>
        /// Возвращает информацию о состоянии сервера Redis.
        /// </summary>
        /// <param name="client">Подключение к серверу Redis.</param>
        /// <returns>Информация о состоянии сервера Redis.</returns>
        public static async Task<DynamicDocument> GetStatusAsync(this ConnectionMultiplexer client)
        {
            var endPoint = client.GetEndPoints().First();

            var serverInfo = await client.GetServer(endPoint).InfoAsync();

            var status = new DynamicDocument();

            if (serverInfo != null)
            {
                foreach (var part in serverInfo)
                {
                    var partName = part.Key;
                    var partValue = new DynamicDocument();

                    foreach (var item in part)
                    {
                        partValue[item.Key] = item.Value;
                    }

                    status[partName] = partValue;
                }
            }

            return status;
        }
    }
}
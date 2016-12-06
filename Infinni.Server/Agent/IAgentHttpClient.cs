using System.IO;
using System.Threading.Tasks;

using InfinniPlatform.Sdk.Dynamic;

namespace Infinni.Server.Agent
{
    /// <summary>
    /// Клиент для HTTP-сервиса приложения.
    /// </summary>
    public interface IAgentHttpClient
    {
        /// <summary>
        /// Отправляет GET-запрос.
        /// </summary>
        /// <typeparam name="T">Тип объекта, получаемого в ответе.</typeparam>
        /// <param name="path">Путь запроса.</param>
        /// <param name="address">Адрес агента.</param>
        /// <param name="port">Порт агента.</param>
        /// <param name="queryContent">Параметры запроса.</param>
        Task<T> Get<T>(string path, string address, int port, DynamicWrapper queryContent = null);

        /// <summary>
        /// Отправляет GET-запрос.
        /// </summary>
        /// <typeparam name="T">Тип объекта, получаемого в ответе.</typeparam>
        /// <param name="path">Путь запроса.</param>
        /// <param name="address">Адрес агента.</param>
        /// <param name="port">Порт агента.</param>
        /// <param name="formContent">Содержимое формы запроса.</param>
        Task<T> Post<T>(string path, string address, int port, DynamicWrapper formContent);

        /// <summary>
        /// Отправляет GET-запрос на получение файлового потока.
        /// </summary>
        /// <param name="path">Путь запроса.</param>
        /// <param name="address">Адрес агента.</param>
        /// <param name="port">Порт агента.</param>
        /// <param name="queryContent">Параметры запроса.</param>
        Task<Stream> GetStream(string path, string address, int port, DynamicWrapper queryContent = null);
    }
}
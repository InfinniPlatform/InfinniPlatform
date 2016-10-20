using InfinniPlatform.Sdk.Services;
using InfinniPlatform.Server.Agent;

namespace InfinniPlatform.Server.RestApi
{
    /// <summary>
    /// Обработчик консольного вывода приложения Infinni.Node.
    /// </summary>
    public interface INodeOutputParser
    {
        /// <summary>
        /// Преобразует вывод команды status и добавляет к ответу сервиса.
        /// </summary>
        /// <param name="serviceResult">Ответ сервиса.</param>
        ServiceResult<ProcessResult> FormatAppsStatusOutput(ServiceResult<ProcessResult> serviceResult);
    }
}
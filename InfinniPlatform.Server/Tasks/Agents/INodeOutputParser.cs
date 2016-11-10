using InfinniPlatform.Sdk.Http.Services;

namespace InfinniPlatform.Server.Tasks.Agents
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
        ServiceResult<AgentTaskStatus> FormatAppsInfoOutput(ServiceResult<AgentTaskStatus> serviceResult);
    }
}
using InfinniPlatform.Sdk.Http.Services;
using Infinni.Server.Tasks.Agents;

namespace Infinni.Server.Tasks.Infinni.Node
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

        /// <summary>
        /// Преобразует вывод команды packages и добавляет к ответу сервиса.
        /// </summary>
        /// <param name="serviceResult">Ответ сервиса.</param>
        ServiceResult<AgentTaskStatus> FormatPackagesOutput(ServiceResult<AgentTaskStatus> serviceResult);
    }
}
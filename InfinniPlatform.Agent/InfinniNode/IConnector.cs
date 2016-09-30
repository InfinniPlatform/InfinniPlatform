using System.Threading.Tasks;

namespace InfinniPlatform.Agent.InfinniNode
{
    /// <summary>
    /// Интерфейс взаимодействия с утилитой Infinni.Node.
    /// </summary>
    public interface IConnector
    {
        Task<ProcessHelper.ProcessResult> InstallApp(string appName);
        Task<ProcessHelper.ProcessResult> UninstallApp(string appName);
        Task<ProcessHelper.ProcessResult> StartApp(string appName);
        Task<ProcessHelper.ProcessResult> StopApp(string appName);
    }
}
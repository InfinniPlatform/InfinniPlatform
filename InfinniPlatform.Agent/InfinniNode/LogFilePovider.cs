using System.IO;

using InfinniPlatform.Agent.Settings;

namespace InfinniPlatform.Agent.InfinniNode
{
    public interface ILogFilePovider
    {
        Stream GetAppLog(string fullAppName);

        Stream GetPerformanceLog(string fullAppName);

        Stream GetNodeLog();
    }


    public class LogFilePovider : ILogFilePovider
    {
        private const string AppsDirectoryName = "install";
        private const string LogsDirectoryName = "logs";
        private const string AppLogFilename = "log.txt";
        private const string PerformanceLogFilename = "performance.txt";
        private const string NodeLogFilename = "Infinni.Node.log";

        public LogFilePovider(AgentSettings settings)
        {
            _settings = settings;
        }

        private readonly AgentSettings _settings;

        public Stream GetAppLog(string fullAppName)
        {
            var filePath = Path.Combine(_settings.NodeDirectory, AppsDirectoryName, fullAppName, LogsDirectoryName, AppLogFilename);

            return new FileStream(filePath, FileMode.Open, FileAccess.Read);
        }

        public Stream GetPerformanceLog(string fullAppName)
        {
            var filePath = Path.Combine(_settings.NodeDirectory, AppsDirectoryName, fullAppName, LogsDirectoryName, PerformanceLogFilename);

            return new FileStream(filePath, FileMode.Open, FileAccess.Read);
        }

        public Stream GetNodeLog()
        {
            var filePath = Path.Combine(_settings.NodeDirectory, NodeLogFilename);

            return new FileStream(filePath, FileMode.Open, FileAccess.Read);
        }
    }
}
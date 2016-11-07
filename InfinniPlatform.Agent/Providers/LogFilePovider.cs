using System;
using System.IO;

using InfinniPlatform.Agent.Helpers;
using InfinniPlatform.Agent.Settings;

namespace InfinniPlatform.Agent.Providers
{
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

        public Func<Stream> GetAppLog(string appFullName)
        {
            var filePath = Path.Combine(_settings.NodeDirectory, AppsDirectoryName, appFullName, LogsDirectoryName, AppLogFilename);

            return StreamHelper.TryGetStream(filePath);
        }

        public Func<Stream> GetPerformanceLog(string appFullName)
        {
            var filePath = Path.Combine(_settings.NodeDirectory, AppsDirectoryName, appFullName, LogsDirectoryName, PerformanceLogFilename);

            return StreamHelper.TryGetStream(filePath);
        }

        public Func<Stream> GetNodeLog()
        {
            var filePath = Path.Combine(_settings.NodeDirectory, NodeLogFilename);

            return StreamHelper.TryGetStream(filePath);
        }
    }
}
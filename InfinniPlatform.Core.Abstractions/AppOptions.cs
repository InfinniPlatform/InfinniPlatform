using System;
using System.Collections.Generic;

namespace InfinniPlatform
{
    /// <summary>
    /// The general application settings.
    /// </summary>
    public class AppOptions
    {
        public const string SectionName = "app";


        /// <summary>
        /// The default application settings.
        /// </summary>
        public static readonly AppOptions Default = new AppOptions();


        public AppOptions()
        {
            AppName = "InfinniPlatform";
            AppInstance = Guid.NewGuid().ToString("N");
            PerformanceLoggerNamePrefix = "IPerformanceLogger.";
            StaticFilesMapping = new Dictionary<string, string>();
        }


        /// <summary>
        /// The application name.
        /// </summary>
        /// <example>
        /// App1
        /// </example>
        public string AppName { get; set; }

        /// <summary>
        /// The application instance identifier.
        /// </summary>
        /// <example>
        /// App1_Instance1
        /// </example>
        public string AppInstance { get; set; }

        /// <summary>
        /// The prefix of the <see cref="InfinniPlatform.Logging.IPerformanceLogger" />.
        /// </summary>
        /// <example>
        /// IPerformanceLogger.
        /// </example>
        public string PerformanceLoggerNamePrefix { get; set; }

        /// <summary>
        /// The relative request paths that map to static resources.
        /// </summary>
        public Dictionary<string, string> StaticFilesMapping { get; set; }
    }
}
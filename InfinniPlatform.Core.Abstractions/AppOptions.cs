using System;
using System.Collections.Generic;

namespace InfinniPlatform
{
    /// <summary>
    /// General application settings from configuration.
    /// </summary>
    public class AppOptions
    {
        /// <summary>
        /// Name of option section in configuration file.
        /// </summary>
        public const string SectionName = "app";

        /// <summary>
        /// Default instance of <see cref="AppOptions" />.
        /// </summary>
        public static readonly AppOptions Default = new AppOptions();


        /// <summary>
        /// Initializes a new instance of <see cref="AppOptions" />.
        /// </summary>
        public AppOptions()
        {
            AppName = "InfinniPlatform";
            AppInstance = Guid.NewGuid().ToString("N");
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
        /// The relative request paths that map to static resources.
        /// </summary>
        public Dictionary<string, string> StaticFilesMapping { get; set; }
    }
}
using System;
using System.IO;
using System.Linq;

using InfinniPlatform.Sdk.Settings;

namespace InfinniPlatform.Core.Settings
{
    internal sealed class AppEnvironment : IAppEnvironment
    {
        public const string SectionName = "app";

        public AppEnvironment()
        {
            Name = "InfinniPlatform";
            _instanceId = $"{Environment.MachineName}_{Environment.CurrentDirectory.Split(Path.DirectorySeparatorChar).Last()}";
        }

        private string _instanceId;

        public string Name { get; set; }

        public string InstanceId
        {
            get { return _instanceId; }
            set
            {
                if (!string.IsNullOrWhiteSpace(value))
                {
                    _instanceId = value;
                }
            }
        }
    }
}
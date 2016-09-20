using System;

using InfinniPlatform.Sdk.Settings;

namespace InfinniPlatform.Core.Settings
{
    internal sealed class AppEnvironment : IAppEnvironment
    {
        public const string SectionName = "app";

        public AppEnvironment()
        {
            Name = "InfinniPlatform";
            InstanceId = Guid.NewGuid().ToString("N");
        }

        public string Name { get; set; }

        public string InstanceId { get; set; }
    }
}
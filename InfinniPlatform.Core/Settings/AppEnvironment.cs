using System;

using InfinniPlatform.Sdk.Settings;

namespace InfinniPlatform.Core.Settings
{
    internal sealed class AppEnvironment : IAppEnvironment
    {
        public const string SectionName = "app";

        public AppEnvironment()
        {
            Id = Guid.NewGuid().ToString("N");
            Name = "InfinniPlatform";
        }

        public string Id { get; }

        public string Name { get; set; }
    }
}
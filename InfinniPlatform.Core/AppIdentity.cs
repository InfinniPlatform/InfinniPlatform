using System;

namespace InfinniPlatform.Core
{
    public class AppIdentity : IAppIdentity
    {
        public AppIdentity()
        {
            Id = Guid.NewGuid().ToString("N");
        }

        public string Id { get; }
    }
}
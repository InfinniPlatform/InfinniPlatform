using System;
using System.Threading;
using InfinniPlatform.Api.DocumentStatus;

namespace InfinniPlatform.Metadata.Statuses
{
    public static class StatusFactoryBuilder
    {
        private static readonly Lazy<IStatusFactory> Instance = new Lazy<IStatusFactory>(() => new StatusFactory(), LazyThreadSafetyMode.ExecutionAndPublication);

        public static IStatusFactory GetInstance()
        {
            return Instance.Value;
        }
    }
}

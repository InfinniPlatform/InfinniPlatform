using System;
using InfinniPlatform.Logging;

namespace InfinniPlatform.ServiceHost
{
    public interface IInterface
    {
        Guid M();
    }

    internal class MyClass : IInterface
    {
        private readonly Guid _newGuid;

        public MyClass(ILog log, IPerformanceLog pLog)
        {
            log.Info("OK!");
            pLog.Log("OK!",DateTime.Now);
            _newGuid = Guid.NewGuid();
        }

        public Guid M()
        {
            return _newGuid;
        }
    }
}
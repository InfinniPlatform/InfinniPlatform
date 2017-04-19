using InfinniPlatform.Core.IoC;
using InfinniPlatform.DocumentStorage.Abstractions.Transactions;

namespace InfinniPlatform.DocumentStorage.MongoDB.Transactions
{
    internal class UnitOfWorkFactory : IUnitOfWorkFactory
    {
        public UnitOfWorkFactory(IContainerResolver containerResolver)
        {
            _containerResolver = containerResolver;
        }


        private readonly IContainerResolver _containerResolver;


        public IUnitOfWork Create()
        {
            var requestUnit = _containerResolver.Resolve<UnitOfWork>();
            var currentUnit = requestUnit.Begin();
            return currentUnit;
        }
    }
}
using InfinniPlatform.Sdk.Documents.Transactions;
using InfinniPlatform.Sdk.IoC;

namespace InfinniPlatform.DocumentStorage.Transactions
{
    internal sealed class UnitOfWorkFactory : IUnitOfWorkFactory
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
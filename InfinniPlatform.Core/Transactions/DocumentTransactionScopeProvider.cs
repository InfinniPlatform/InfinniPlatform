﻿using InfinniPlatform.Sdk.IoC;

namespace InfinniPlatform.Core.Transactions
{
    internal sealed class DocumentTransactionScopeProvider : IDocumentTransactionScopeProvider
    {
        public DocumentTransactionScopeProvider(IContainerResolver containerResolver)
        {
            // Зависимость IDocumentTransactionScope регистрируется в IoC со стратегией InstancePerRequest,
            // в то время, как зависимые от нее объекты лучше регистрировать со стратегией SingleInstance.
            // Поэтому был создан IDocumentTransactionScopeProvider, регистрируемый как SingleInstance,
            // который имеет непосредственный доступ к IoC и возвращает IDocumentTransactionScope
            // по требованию. В итоге, если классу нужен доступ к IDocumentTransactionScope, лучше,
            // если он будет получать его через IDocumentTransactionScopeProvider, в противном случае
            // он должен быть зарегистрирован со стратегией InstancePerRequest, как и все зависимые от
            // него классы.

            _containerResolver = containerResolver;
        }


        private readonly IContainerResolver _containerResolver;


        public IDocumentTransactionScope GetTransactionScope()
        {
            return _containerResolver.Resolve<IDocumentTransactionScope>();
        }
    }
}
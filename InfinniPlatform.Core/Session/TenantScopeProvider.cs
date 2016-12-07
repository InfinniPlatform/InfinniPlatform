using System;
using System.Runtime.Remoting.Messaging;

using InfinniPlatform.Sdk.Session;

namespace InfinniPlatform.Core.Session
{
    internal class TenantScopeProvider : ITenantScopeProvider
    {
        private const string TenantScopeKey = "TenantScope";


        public ITenantScope GetTenantScope()
        {
            var prevScopeContext = GetCurrentTenantScope();

            return prevScopeContext?.Scope;
        }

        public ITenantScope BeginTenantScope(string tenantId)
        {
            if (string.IsNullOrEmpty(tenantId))
            {
                throw new ArgumentNullException(nameof(tenantId));
            }

            var prevScopeContext = GetCurrentTenantScope();

            var newScope = new TenantScope(tenantId);

            var newScopeContext = new TenantScopeContext(newScope);

            newScope.OnDispose += () =>
                                  {
                                      SetCurrentTenantScope(prevScopeContext);

                                      newScopeContext.Dispose();
                                  };

            SetCurrentTenantScope(newScopeContext);

            return newScope;
        }


        private static TenantScopeContext GetCurrentTenantScope()
        {
            return CallContext.LogicalGetData(TenantScopeKey) as TenantScopeContext;
        }

        private static void SetCurrentTenantScope(TenantScopeContext scopeContext)
        {
            CallContext.LogicalSetData(TenantScopeKey, scopeContext);
        }
    }
}
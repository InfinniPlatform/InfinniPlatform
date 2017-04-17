using System;
using System.Threading;

using InfinniPlatform.Core.Abstractions.Session;

namespace InfinniPlatform.Core.Session
{
    internal class TenantScopeProvider : ITenantScopeProvider
    {
        private static readonly AsyncLocal<TenantScopeContext> TenantScopeContext = new AsyncLocal<TenantScopeContext>();


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
            return TenantScopeContext.Value;
        }

        private static void SetCurrentTenantScope(TenantScopeContext scopeContext)
        {
            TenantScopeContext.Value = scopeContext;
        }
    }
}
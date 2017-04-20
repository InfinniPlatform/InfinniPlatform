using System;

namespace InfinniPlatform.Session
{
    [Serializable]
    internal class TenantScopeContext : IDisposable
    {
        public TenantScopeContext(ITenantScope scope)
        {
            _scopeRef = new WeakReference<ITenantScope>(scope);
        }


        [NonSerialized]
        private WeakReference<ITenantScope> _scopeRef;


        public ITenantScope Scope
        {
            get
            {
                ITenantScope scope;

                return (_scopeRef != null) && _scopeRef.TryGetTarget(out scope) ? scope : null;
            }
        }


        public void Dispose()
        {
            _scopeRef = null;
        }
    }
}
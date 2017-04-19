using System;

namespace InfinniPlatform.Core.Session
{
    internal class TenantScope : ITenantScope
    {
        public TenantScope(string tenantId)
        {
            TenantId = tenantId;
        }


        public string TenantId { get; }


        public event Action OnDispose;


        public void Dispose()
        {
            OnDispose?.Invoke();
        }
    }
}
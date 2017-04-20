using System;

namespace InfinniPlatform.Session
{
    /// <summary>
    /// Область для выполнения запросов от имени определенной организации.
    /// </summary>
    public interface ITenantScope : IDisposable
    {
        /// <summary>
        /// Идентификатор организации.
        /// </summary>
        string TenantId { get; }
    }
}
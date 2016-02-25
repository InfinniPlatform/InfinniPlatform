using System.Threading.Tasks;

namespace InfinniPlatform.Core.Diagnostics
{
    /// <summary>
    /// Предоставляет информацию о состоянии системы в целом.
    /// </summary>
    public interface ISystemStatusProvider
    {
        /// <summary>
        /// Возвращает информацию о системе.
        /// </summary>
        Task<object> GetStatus();
    }
}
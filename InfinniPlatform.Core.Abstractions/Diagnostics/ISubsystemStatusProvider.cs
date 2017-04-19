using System.Threading.Tasks;

using InfinniPlatform.Core.Http;

namespace InfinniPlatform.Core.Diagnostics
{
    /// <summary>
    /// Предоставляет информацию о состоянии подсистемы.
    /// </summary>
    public interface ISubsystemStatusProvider
    {
        /// <summary>
        /// Имя подсистемы.
        /// </summary>
        string Name { get; }

        /// <summary>
        /// Состояние подсистемы.
        /// </summary>
        /// <param name="request">Запрос.</param>
        Task<object> GetStatus(IHttpRequest request);
    }
}
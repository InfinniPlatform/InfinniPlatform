using InfinniPlatform.Api.Profiling;
using InfinniPlatform.Sdk.Environment;
using InfinniPlatform.Sdk.Environment.Log;

namespace InfinniPlatform.Logging
{
    /// <summary>
    ///     Фабрика для создания <see cref="ILog" />.
    /// </summary>
    public interface ILogFactory
    {
        /// <summary>
        ///     Создает <see cref="ILog" />.
        /// </summary>
        ILog CreateLog(string name = "Log4Net");
    }
}
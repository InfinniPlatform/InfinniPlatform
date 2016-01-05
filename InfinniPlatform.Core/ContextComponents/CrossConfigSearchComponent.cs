using InfinniPlatform.Sdk.ContextComponents;
using InfinniPlatform.Sdk.Environment;

namespace InfinniPlatform.Core.ContextComponents
{
    /// <summary>
    ///     Компонент контекста для поиска документов между различными конфигурациями
    /// </summary>
    public sealed class CrossConfigSearchComponent : ICrossConfigSearchComponent
    {
        private readonly ICrossConfigSearcher _crossConfigSearcher;

        public CrossConfigSearchComponent(ICrossConfigSearcher crossConfigSearcher)
        {
            _crossConfigSearcher = crossConfigSearcher;
        }

        public ICrossConfigSearcher GetCrossConfigSearcher()
        {
            return _crossConfigSearcher;
        }
    }
}
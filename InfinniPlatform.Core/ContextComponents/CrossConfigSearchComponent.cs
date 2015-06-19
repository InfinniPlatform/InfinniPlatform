using InfinniPlatform.Api.ContextComponents;
using InfinniPlatform.Api.Factories;

namespace InfinniPlatform.ContextComponents
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
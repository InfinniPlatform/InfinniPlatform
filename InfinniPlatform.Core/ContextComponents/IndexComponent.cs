using InfinniPlatform.Api.ContextComponents;
using InfinniPlatform.Api.Index;

namespace InfinniPlatform.ContextComponents
{
    /// <summary>
    ///     Компонент для работы с индексами в глобальном контексте
    /// </summary>
    public sealed class IndexComponent : IIndexComponent
    {
        private readonly IIndexFactory _indexFactory;

        public IndexComponent(IIndexFactory indexFactory)
        {
            _indexFactory = indexFactory;
        }

        /// <summary>
        ///     Фабрика для работы с индексами
        /// </summary>
        public IIndexFactory IndexFactory
        {
            get { return _indexFactory; }
        }
    }
}
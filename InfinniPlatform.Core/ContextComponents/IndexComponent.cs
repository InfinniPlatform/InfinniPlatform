using InfinniPlatform.Api.Index;
using InfinniPlatform.Sdk.ContextComponents;
using InfinniPlatform.Sdk.Environment.Index;

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
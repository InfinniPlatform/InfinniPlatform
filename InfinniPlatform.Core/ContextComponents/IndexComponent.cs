using InfinniPlatform.Sdk.ContextComponents;
using InfinniPlatform.Sdk.Environment.Index;

namespace InfinniPlatform.Core.ContextComponents
{
    /// <summary>
    /// Компонент для работы с индексами в глобальном контексте
    /// </summary>
    public sealed class IndexComponent : IIndexComponent
    {
        public IndexComponent(IIndexFactory indexFactory)
        {
            IndexFactory = indexFactory;
        }

        /// <summary>
        /// Фабрика для работы с индексами
        /// </summary>
        public IIndexFactory IndexFactory { get; }
    }
}
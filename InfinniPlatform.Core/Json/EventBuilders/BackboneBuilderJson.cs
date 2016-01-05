using InfinniPlatform.Sdk.Events;

namespace InfinniPlatform.Core.Json.EventBuilders
{
    public class BackboneBuilderJson : BackboneBuilder
    {
        public BackboneBuilderJson()
        {
            RegisterBuilder(EventType.AddItemToCollection, new CollectionItemBuilder());
            RegisterBuilder(EventType.CreateCollection, new ContainerCollectionBuilder());
            RegisterBuilder(EventType.CreateContainer, new ContainerBuilder());
            RegisterBuilder(EventType.CreateProperty, new PropertyBuilder());
            RegisterBuilder(EventType.RemoveItemFromCollection, new ObjectRemoveBuilder());
            RegisterBuilder(EventType.RemoveProperty, new ObjectRemoveBuilder());
            RegisterBuilder(EventType.ChangeCollectionItemIndex, new ChangeCollectionItemIndexBuilder());
        }
    }
}
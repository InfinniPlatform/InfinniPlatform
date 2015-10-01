namespace InfinniPlatform.Sdk.Events
{
    public enum EventType
    {
        CreateContainer = 1,
        CreateProperty = 2,
        RemoveProperty = 4,
        CreateCollection = 8,
        AddItemToCollection = 16,
        RemoveItemFromCollection = 32,
        ChangeCollectionItemIndex = 64
    }
}
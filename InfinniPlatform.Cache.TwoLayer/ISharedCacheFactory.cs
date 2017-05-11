namespace InfinniPlatform.Cache
{
    public interface ISharedCacheFactory
    {
        ISharedCache Create();
    }
}
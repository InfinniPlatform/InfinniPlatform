namespace InfinniPlatform.Sdk.Cache
{
    /// <summary>
    /// Распределленый кэш.
    /// </summary>
    /// <remarks>
    /// Предоставляет возможность разделить кэш между несколькими приложениями.
    /// </remarks>
    public interface ISharedCache : ICache
    {
    }
}
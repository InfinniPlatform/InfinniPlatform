namespace InfinniPlatform.Sdk.Cache
{
    /// <summary>
    /// Двухуровневый кэш.
    /// </summary>
    /// <remarks>
    /// Позволяет достичь большей производительности за счет использования кэширования в памяти
    /// наряду с распределенным кэшем.
    /// </remarks>
    public interface ITwoLayerCache : ICache
    {
    }
}
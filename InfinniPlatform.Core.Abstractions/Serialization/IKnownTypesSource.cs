namespace InfinniPlatform.Serialization
{
    /// <summary>
    /// Источник известных типов для сериализации.
    /// </summary>
    public interface IKnownTypesSource
    {
        /// <summary>
        /// Добавляет известные типы для сериализации.
        /// </summary>
        /// <param name="knownTypesContainer">Контейнер известных типов для сериализации.</param>
        void AddKnownTypes(KnownTypesContainer knownTypesContainer);
    }
}
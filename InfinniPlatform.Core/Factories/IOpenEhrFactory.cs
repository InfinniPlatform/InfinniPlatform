using InfinniPlatform.Core.ModelRepository;
using InfinniPlatform.Core.ModelRepository.DataConverters;

namespace InfinniPlatform.Core.Factories
{
    /// <summary>
    ///     Фабрика моделей OpenEHR
    /// </summary>
    public interface IOpenEhrFactory
    {
        /// <summary>
        ///     Создать конвертер данных
        /// </summary>
        /// <returns>Конвертер данных</returns>
        IDataConverter BuildDataConverter();

        /// <summary>
        ///     Создать экстрактор архетипов
        /// </summary>
        /// <returns>Экстрактор архетипов</returns>
        IArchetypeExtractor BuildArchetypeExtractor();

        /// <summary>
        ///     Создать экстрактор комплексных типов
        /// </summary>
        /// <returns>Экстрактор комлексных типов</returns>
        IComplexTypeExtractor BuildComplexTypeExtractor();

        /// <summary>
        ///     Создать экстрактор шаблонов
        /// </summary>
        /// <returns>Экстрактор шаблонов</returns>
        ITemplateExtractor BuildTemplateExtractor();
    }
}
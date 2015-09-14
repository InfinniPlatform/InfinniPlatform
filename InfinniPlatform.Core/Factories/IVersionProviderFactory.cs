using InfinniPlatform.Api.Index;
using InfinniPlatform.Sdk.Environment.Index;

namespace InfinniPlatform.Factories
{
    public interface IVersionProviderFactory
    {
        /// <summary>
        ///     Создать стратегию с двумя индексами и поддержкой истории версий
        ///     Актуальные версии документов хранятся в одном индексе
        ///     Все остальные версии документов хранятся в другом индексе
        ///     Особенности стратегии:
        ///     - Медленная вставка за счет того, что сначала необходимо перенести предшествующую версию документа
        ///     из одного индекса в другой, затем добавить актуальную версию документа
        ///     - Суммарный размер индексов составляет 1 единицу
        ///     - Задержка при поиске всех версий документа, так как необходимо искать по двум индексам
        /// </summary>
        /// <param name="indexName">Наименование индекса</param>
        /// <param name="typeName"></param>
        /// <param name="configVersion">Версия конфигурации</param>
        /// <returns>Провайдер версий документа</returns>
        IVersionProvider BuildVersionProviderTwoIndexHistory(string indexName, string typeName,
            string configVersion = null);

        /// <summary>
        ///     Создать стратегию с одним индексом и без поддержки истории
        ///     Хранятся только актуальные версии документов в одном индексе
        ///     Особенности стратегии:
        ///     - Быстрая вставка (1)
        ///     - Быстрый поиск (1)
        ///     - Отсутствие информации об истории изменений объекта
        /// </summary>
        /// <param name="indexName">Наименование индекса</param>
        /// <param name="typeName">Наименование типа данных</param>
        /// <returns>Провайдер версий документов</returns>
        IVersionProvider BuildVersionProviderOneIndex(string indexName, string typeName);
    }
}
using System.Collections.Generic;

namespace InfinniPlatform.Sdk.Environment.Index
{
    /// <summary>
    ///     Провайдер стандартных операций с данными
    /// </summary>
    public interface ICrudOperationProvider
    {
        /// <summary>
        ///     Индексировать список динамических объектов
        /// </summary>
        /// <param name="itemsToIndex">Динамические объекты для индексации</param>
        void SetItems(IEnumerable<dynamic> itemsToIndex);

        /// <summary>
        ///     Индексировать динамический объект с использованием стратегии по умолчанию
        ///     (Стратегия по умолчанию следующая (UpdateItemStrategy):
        ///     1) Если объект существует в индексе - он апдейтится
        ///     2) Если объект не существует в индексе - он вставляется в индекс
        /// </summary>
        /// <param name="item">Объект для индексации</param>
        void Set(dynamic item);

        /// <summary>
        ///     Индексировать динамический объект с указанием конкретной стратегии индексации
        /// </summary>
        /// <param name="item">Объект для индексации</param>
        /// <param name="indexItemStrategy">Стратегия индексации объекта</param>
        void Set(dynamic item, IndexItemStrategy indexItemStrategy);

        /// <summary>
        ///     Удалить объект из индекса
        /// </summary>
        /// <param name="key">Идентификатор удаляемого объекта индекса</param>
        void Remove(string key);

        /// <summary>
        ///     Удалить объекты с идентификаторами из списка
        /// </summary>
        /// <param name="ids">Список идентификаторов</param>
        void RemoveItems(IEnumerable<string> ids);

        /// <summary>
        ///     Получить объект по идентификатору
        /// </summary>
        /// <param name="key">Идентификатор индексируемого объекта</param>
        /// <returns>Индексируемый объект</returns>
        dynamic GetItem(string key);

        /// <summary>
        ///     Получить список объектов из индекса
        /// </summary>
        /// <param name="ids">Список идентификаторов</param>
        /// <returns>Список документов индекса</returns>
        IEnumerable<dynamic> GetItems(IEnumerable<string> ids);

        /// <summary>
        ///     Получить количество записей в индексе
        /// </summary>
        /// <returns>Количество записей в индексе</returns>
        int GetTotalCount();

        /// <summary>
        ///     Обновление позволяет делать запросы к только что добавленным данным
        /// </summary>
        /// Операция Refresh должна использоваться только в случае необходимости получения данных, ранее записанных в том же потоке
        /// Во всех остальных случаях достаточно выполнения операции Set. Предназначена для ручного управления логом транзакций elasticsearch
        void Refresh();
    }
}
using InfinniPlatform.Core.Dynamic;

namespace InfinniPlatform.DocumentStorage.Abstractions
{
    /// <summary>
    /// Указатель на список документов для агрегации.
    /// </summary>
    public interface IDocumentAggregateCursor : IDocumentCursor<DynamicWrapper>
    {
        /// <summary>
        /// Создает проекцию для отображения результата агрегации документов.
        /// </summary>
        /// <param name="projection">Правило формирования проекции.</param>
        IDocumentAggregateCursor Project(DynamicWrapper projection);

        /// <summary>
        /// Создает отдельный документ для каждого элемента массива, находящегося в указанном свойстве исходного документа.
        /// </summary>
        /// <param name="arrayProperty">Свойство документа, содержащее массив.</param>
        IDocumentAggregateCursor Unwind(string arrayProperty);

        /// <summary>
        /// Группирует исходные документы по указанному выражению и производит вычисление заданных функций агрегации для каждой группы.
        /// </summary>
        /// <param name="group">Правило группировки и агрегации групп данных.</param>
        IDocumentAggregateCursor Group(DynamicWrapper group);

        /// <summary>
        /// Присоединяет к каждому исходному документу документ из указанной внешней коллекции в соответствии с заданным правилом
        /// (left outer join).
        /// </summary>
        /// <param name="foreignDocumentType">Имя типа внешнего документа.</param>
        /// <param name="localKeyProperty">Свойство исходного документа, содержащее ключ для присоединения.</param>
        /// <param name="foreignKeyProperty">Свойство внешнего документа, содержащее ключ для присоединения.</param>
        /// <param name="resultArrayProperty">Свойство, в которое будет помещен массив присоединенных документов.</param>
        IDocumentAggregateCursor Lookup(string foreignDocumentType, string localKeyProperty, string foreignKeyProperty, string resultArrayProperty);

        /// <summary>
        /// Сортирует документы по возрастанию указанного свойства.
        /// </summary>
        IDocumentAggregateSortedCursor SortBy(string property);

        /// <summary>
        /// Сортирует документы по убыванию указанного свойства.
        /// </summary>
        IDocumentAggregateSortedCursor SortByDescending(string property);

        /// <summary>
        /// Пропускает указанное количество документов в результирующей выборке.
        /// </summary>
        IDocumentAggregateCursor Skip(int skip);

        /// <summary>
        /// Ограничивает результирующую выборку указанным количеством документов.
        /// </summary>
        IDocumentAggregateCursor Limit(int limit);
    }
}
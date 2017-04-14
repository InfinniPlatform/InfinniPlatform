using System;
using System.Linq;
using System.Linq.Expressions;

namespace InfinniPlatform.DocumentStorage.Abstractions
{
    /// <summary>
    /// Указатель на список документов для агрегации.
    /// </summary>
    public interface IDocumentAggregateCursor<TResult> : IDocumentCursor<TResult>
    {
        /// <summary>
        /// Создает проекцию для отображения результата агрегации документов.
        /// </summary>
        /// <param name="projection">Правило формирования проекции.</param>
        IDocumentAggregateCursor<TNewResult> Project<TNewResult>(Expression<Func<TResult, TNewResult>> projection);

        /// <summary>
        /// Создает отдельный документ для каждого элемента массива, находящегося в указанном свойстве исходного документа.
        /// </summary>
        /// <param name="arrayProperty">Свойство документа, содержащее массив.</param>
        IDocumentAggregateCursor<TNewResult> Unwind<TNewResult>(Expression<Func<TResult, object>> arrayProperty);

        /// <summary>
        /// Группирует исходные документы по указанному выражению и производит вычисление заданных функций агрегации для каждой группы.
        /// </summary>
        /// <param name="groupKey">Функция выборки группы.</param>
        /// <param name="groupValue">Функция агрегации группы.</param>
        IDocumentAggregateCursor<TNewResult> Group<TKey, TNewResult>(Expression<Func<TResult, TKey>> groupKey, Expression<Func<IGrouping<TKey, TResult>, TNewResult>> groupValue);

        /// <summary>
        /// Присоединяет к каждому исходному документу документ из указанной внешней коллекции в соответствии с заданным правилом
        /// (left outer join).
        /// </summary>
        /// <param name="foreignDocumentType">Имя типа внешнего документа.</param>
        /// <param name="localKeyProperty">Свойство исходного документа, содержащее ключ для присоединения.</param>
        /// <param name="foreignKeyProperty">Свойство внешнего документа, содержащее ключ для присоединения.</param>
        /// <param name="resultArrayProperty">Свойство, в которое будет помещен массив присоединенных документов.</param>
        IDocumentAggregateCursor<TNewResult> Lookup<TForeignDocument, TNewResult>(string foreignDocumentType, Expression<Func<TResult, object>> localKeyProperty, Expression<Func<TForeignDocument, object>> foreignKeyProperty, Expression<Func<TNewResult, object>> resultArrayProperty);

        /// <summary>
        /// Сортирует документы по возрастанию указанного свойства.
        /// </summary>
        IDocumentAggregateSortedCursor<TResult> SortBy(Expression<Func<TResult, object>> property);

        /// <summary>
        /// Сортирует документы по убыванию указанного свойства.
        /// </summary>
        IDocumentAggregateSortedCursor<TResult> SortByDescending(Expression<Func<TResult, object>> property);

        /// <summary>
        /// Пропускает указанное количество документов в результирующей выборке.
        /// </summary>
        IDocumentAggregateCursor<TResult> Skip(int skip);

        /// <summary>
        /// Ограничивает результирующую выборку указанным количеством документов.
        /// </summary>
        IDocumentAggregateCursor<TResult> Limit(int limit);
    }
}
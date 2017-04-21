using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace InfinniPlatform.DocumentStorage
{
    /// <summary>
    /// Предоставляет методы создания операторов обновления документов.
    /// </summary>
    public interface IDocumentUpdateBuilder<TDocument>
    {
        /// <summary>
        /// Создает оператор обновления, который изменяет имя указанного свойства документа.
        /// </summary>
        IDocumentUpdateBuilder<TDocument> Rename(Expression<Func<TDocument, object>> property, string newProperty);

        /// <summary>
        /// Создает оператор обновления, который удаляет указанное свойство документа.
        /// </summary>
        IDocumentUpdateBuilder<TDocument> Remove(Expression<Func<TDocument, object>> property);


        /// <summary>
        /// Создает оператор обновления, который присваивает свойству документа указанное значение.
        /// </summary>
        IDocumentUpdateBuilder<TDocument> Set<TProperty>(Expression<Func<TDocument, TProperty>> property, TProperty value);

        /// <summary>
        /// Создает оператор обновления, который присваивает свойству документа результат сложения текущего значения с указанным.
        /// </summary>
        IDocumentUpdateBuilder<TDocument> Inc<TProperty>(Expression<Func<TDocument, TProperty>> property, TProperty value);

        /// <summary>
        /// Создает оператор обновления, который присваивает свойству документа результат умножения текущего значения на указанное.
        /// </summary>
        IDocumentUpdateBuilder<TDocument> Mul<TProperty>(Expression<Func<TDocument, TProperty>> property, TProperty value);

        /// <summary>
        /// Создает оператор обновления, который присваивает свойству документа указанное значение, если оно меньше текущего.
        /// </summary>
        IDocumentUpdateBuilder<TDocument> Min<TProperty>(Expression<Func<TDocument, TProperty>> property, TProperty value);

        /// <summary>
        /// Создает оператор обновления, который присваивает свойству документа указанное значение, если оно больше текущего.
        /// </summary>
        IDocumentUpdateBuilder<TDocument> Max<TProperty>(Expression<Func<TDocument, TProperty>> property, TProperty value);

        /// <summary>
        /// Создает оператор обновления, который присваивает свойству документа результат бинарного сложения (OR) текущего значения с указанным.
        /// </summary>
        IDocumentUpdateBuilder<TDocument> BitwiseOr<TProperty>(Expression<Func<TDocument, TProperty>> property, TProperty value);

        /// <summary>
        /// Создает оператор обновления, который присваивает свойству документа результат бинарного умножения (AND) текущего значения на указанное.
        /// </summary>
        IDocumentUpdateBuilder<TDocument> BitwiseAnd<TProperty>(Expression<Func<TDocument, TProperty>> property, TProperty value);

        /// <summary>
        /// Создает оператор обновления, который присваивает свойству документа результат бинарного сложения по модулю 2 (XOR) текущего значения с указанным.
        /// </summary>
        IDocumentUpdateBuilder<TDocument> BitwiseXor<TProperty>(Expression<Func<TDocument, TProperty>> property, TProperty value);

        /// <summary>
        /// Создает оператор обновления, который присваивает свойству документа текущую дату и время.
        /// </summary>
        IDocumentUpdateBuilder<TDocument> CurrentDate(Expression<Func<TDocument, object>> property);


        /// <summary>
        /// Создает оператор обновления, который проверяет, что свойство документа является массивом, и добавляет в него указанный элемент.
        /// </summary>
        IDocumentUpdateBuilder<TDocument> Push<TItem>(Expression<Func<TDocument, IEnumerable<TItem>>> arrayProperty, TItem item);

        /// <summary>
        /// Создает оператор обновления, который проверяет, что свойство документа является массивом, и добавляет в него все указанные элементы.
        /// </summary>
        IDocumentUpdateBuilder<TDocument> PushAll<TItem>(Expression<Func<TDocument, IEnumerable<TItem>>> arrayProperty, IEnumerable<TItem> items);

        /// <summary>
        /// Создает оператор обновления, который проверяет, что свойство документа является массивом, и добавляет в него указанный элемент, если его еще нет в массиве.
        /// </summary>
        IDocumentUpdateBuilder<TDocument> PushUnique<TItem>(Expression<Func<TDocument, IEnumerable<TItem>>> arrayProperty, TItem item);

        /// <summary>
        /// Создает оператор обновления, который проверяет, что свойство документа является массивом, и добавляет в него все указанные элементы, кроме тех, которые уже есть в массиве.
        /// </summary>
        IDocumentUpdateBuilder<TDocument> PushAllUnique<TItem>(Expression<Func<TDocument, IEnumerable<TItem>>> arrayProperty, IEnumerable<TItem> items);


        /// <summary>
        /// Создает оператор обновления, который проверяет, что свойство документа является массивом, и удаляет из него первый элемент.
        /// </summary>
        IDocumentUpdateBuilder<TDocument> PopFirst(Expression<Func<TDocument, object>> arrayProperty);

        /// <summary>
        /// Создает оператор обновления, который проверяет, что свойство документа является массивом, и удаляет из него последний элемент.
        /// </summary>
        IDocumentUpdateBuilder<TDocument> PopLast(Expression<Func<TDocument, object>> arrayProperty);

        /// <summary>
        /// Создает оператор обновления, который проверяет, что свойство документа является массивом, и удаляет из него указанный элемент.
        /// </summary>
        IDocumentUpdateBuilder<TDocument> Pull<TItem>(Expression<Func<TDocument, IEnumerable<TItem>>> arrayProperty, TItem item);

        /// <summary>
        /// Создает оператор обновления, который проверяет, что свойство документа является массивом, и удаляет из него все указанные элементы.
        /// </summary>
        IDocumentUpdateBuilder<TDocument> PullAll<TItem>(Expression<Func<TDocument, IEnumerable<TItem>>> arrayProperty, IEnumerable<TItem> items);

        /// <summary>
        /// Создает оператор обновления, который проверяет, что свойство документа является массивом, и удаляет из него все элементы, удовлетворяющие указанному фильтру.
        /// </summary>
        IDocumentUpdateBuilder<TDocument> PullFilter<TItem>(Expression<Func<TDocument, IEnumerable<TItem>>> arrayProperty, Expression<Func<TItem, bool>> filter = null);
    }
}
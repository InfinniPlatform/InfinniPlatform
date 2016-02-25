using System;
using System.Collections.Generic;

namespace InfinniPlatform.Sdk.Documents
{
    /// <summary>
    /// Предоставляет методы создания операторов обновления документов.
    /// </summary>
    public interface IDocumentUpdateBuilder
    {
        /// <summary>
        /// Создает оператор обновления, который изменяет имя указанного свойства документа.
        /// </summary>
        IDocumentUpdateBuilder Rename(string property, string newProperty);

        /// <summary>
        /// Создает оператор обновления, который удаляет указанное свойство документа.
        /// </summary>
        IDocumentUpdateBuilder Remove(string property);


        /// <summary>
        /// Создает оператор обновления, который присваивает свойству документа указанное значение.
        /// </summary>
        IDocumentUpdateBuilder Set<TProperty>(string property, TProperty value);

        /// <summary>
        /// Создает оператор обновления, который присваивает свойству документа результат сложения текущего значения с указанным.
        /// </summary>
        IDocumentUpdateBuilder Inc(string property, object value);

        /// <summary>
        /// Создает оператор обновления, который присваивает свойству документа результат умножения текущего значения на указанное.
        /// </summary>
        IDocumentUpdateBuilder Mul(string property, object value);

        /// <summary>
        /// Создает оператор обновления, который присваивает свойству документа указанное значение, если оно меньше текущего.
        /// </summary>
        IDocumentUpdateBuilder Min(string property, object value);

        /// <summary>
        /// Создает оператор обновления, который присваивает свойству документа указанное значение, если оно больше текущего.
        /// </summary>
        IDocumentUpdateBuilder Max(string property, object value);

        /// <summary>
        /// Создает оператор обновления, который присваивает свойству документа результат бинарного сложения (OR) текущего значения с указанным.
        /// </summary>
        IDocumentUpdateBuilder BitwiseOr(string property, object value);

        /// <summary>
        /// Создает оператор обновления, который присваивает свойству документа результат бинарного умножения (AND) текущего значения на указанное.
        /// </summary>
        IDocumentUpdateBuilder BitwiseAnd(string property, object value);

        /// <summary>
        /// Создает оператор обновления, который присваивает свойству документа результат бинарного сложения по модулю 2 (XOR) текущего значения с указанным.
        /// </summary>
        IDocumentUpdateBuilder BitwiseXor(string property, object value);

        /// <summary>
        /// Создает оператор обновления, который присваивает свойству документа текущую дату и время.
        /// </summary>
        IDocumentUpdateBuilder CurrentDate(string property);


        /// <summary>
        /// Создает оператор обновления, который проверяет, что свойство документа является массивом, и добавляет в него указанный элемент.
        /// </summary>
        IDocumentUpdateBuilder Push(string arrayProperty, object item);

        /// <summary>
        /// Создает оператор обновления, который проверяет, что свойство документа является массивом, и добавляет в него все указанные элементы.
        /// </summary>
        IDocumentUpdateBuilder PushAll<TItem>(string arrayProperty, IEnumerable<TItem> items);

        /// <summary>
        /// Создает оператор обновления, который проверяет, что свойство документа является массивом, и добавляет в него указанный элемент, если его еще нет в массиве.
        /// </summary>
        IDocumentUpdateBuilder PushUnique(string arrayProperty, object item);

        /// <summary>
        /// Создает оператор обновления, который проверяет, что свойство документа является массивом, и добавляет в него все указанные элементы, кроме тех, которые уже есть в массиве.
        /// </summary>
        IDocumentUpdateBuilder PushAllUnique<TItem>(string arrayProperty, IEnumerable<TItem> items);


        /// <summary>
        /// Создает оператор обновления, который проверяет, что свойство документа является массивом, и удаляет из него первый элемент.
        /// </summary>
        IDocumentUpdateBuilder PopFirst(string arrayProperty);

        /// <summary>
        /// Создает оператор обновления, который проверяет, что свойство документа является массивом, и удаляет из него последний элемент.
        /// </summary>
        IDocumentUpdateBuilder PopLast(string arrayProperty);

        /// <summary>
        /// Создает оператор обновления, который проверяет, что свойство документа является массивом, и удаляет из него указанный элемент.
        /// </summary>
        IDocumentUpdateBuilder Pull(string arrayProperty, object item);

        /// <summary>
        /// Создает оператор обновления, который проверяет, что свойство документа является массивом, и удаляет из него все указанные элементы.
        /// </summary>
        IDocumentUpdateBuilder PullAll<TItem>(string arrayProperty, IEnumerable<TItem> items);

        /// <summary>
        /// Создает оператор обновления, который проверяет, что свойство документа является массивом, и удаляет из него все элементы, удовлетворяющие указанному фильтру.
        /// </summary>
        IDocumentUpdateBuilder PullFilter(string arrayProperty, Func<IDocumentFilterBuilder, object> filter = null);
    }
}
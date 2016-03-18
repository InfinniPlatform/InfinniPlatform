using System;

namespace InfinniPlatform.Sdk.Documents
{
    /// <summary>
    /// Предоставляет методы создания проекции данных документов.
    /// </summary>
    public interface IDocumentProjectionBuilder
    {
        /// <summary>
        /// Создает оператор проекции, который включает указанное свойство документа в результат выборки.
        /// </summary>
        /// <param name="property">Свойство документа.</param>
        IDocumentProjectionBuilder Include(string property);

        /// <summary>
        /// Создает оператор проекции, который исключает указанное свойство документа из результата выборки.
        /// </summary>
        /// <param name="property">Свойство документа.</param>
        IDocumentProjectionBuilder Exclude(string property);


        /// <summary>
        /// Создает оператор проекции, который включает указанное свойство документа, содержащее массив, но возвращает только заданное количество элементов с начала или конца исходного массива.
        /// </summary>
        /// <param name="arrayProperty">Свойство документа, содержащее массив.</param>
        /// <param name="count">Количество элементов с начала или конца массива.</param>
        /// <remarks>
        /// Если <paramref name="count"/> является положительным числом, указанное количество элементов берется с начала массива.
        /// Если <paramref name="count"/> является отрицательным числом, указанное количество элементов берется с конца массива.
        /// </remarks>
        IDocumentProjectionBuilder Slice(string arrayProperty, int count);

        /// <summary>
        /// Создает оператор проекции, который включает указанное свойство документа, содержащее массив, но возвращает элементы из заданного диапазона.
        /// </summary>
        /// <remarks>
        /// <param name="arrayProperty">Свойство документа, содержащее массив.</param>
        /// <param name="index">Индекс начала диапазона.</param>
        /// <param name="limit">Максимальное количество элементов.</param>
        /// Если <paramref name="index"/> является положительным числом, диапазон начинается с начала массива.
        /// Если <paramref name="index"/> является отрицательным числом, диапазон начинается с конца массива.
        /// Значение <paramref name="limit"/> может быть только положительным числом.
        /// </remarks>
        IDocumentProjectionBuilder Slice(string arrayProperty, int index, int limit);

        /// <summary>
        /// Создает оператор проекции, который включает указанное свойство документа, содержащее массив, оставляя в нем один элемент, удовлетворяющий заданному условию.
        /// </summary>
        /// <param name="arrayProperty">Свойство документа, содержащее массив.</param>
        /// <param name="filter">Условие фильтрации элементов.</param>
        IDocumentProjectionBuilder Match(string arrayProperty, Func<IDocumentFilterBuilder, object> filter = null);


        /// <summary>
        /// Создает оператор проекции, который включает указанное свойство документа в результат выборки и помещает в него значение релевантности документа, полученное при полнотекстовом поиске.
        /// </summary>
        /// <param name="property">Свойство документа, в которое будет помещено значение релевантности документа, полученное при полнотекстовом поиске.</param>
        IDocumentProjectionBuilder IncludeTextScore(string property = DocumentStorageExtensions.DefaultTextScoreProperty);
    }
}
using System.Collections.Generic;
using System.Text.RegularExpressions;

using InfinniPlatform.DocumentStorage.Metadata;

namespace InfinniPlatform.DocumentStorage
{
    /// <summary>
    /// Предоставляет методы создания фильтров для поиска документов.
    /// </summary>
    public interface IDocumentFilterBuilder
    {
        /// <summary>
        /// Создает пустой фильтр.
        /// </summary>
        object Empty();


        /// <summary>
        /// Создает фильтр логического отрицания (NOT).
        /// </summary>
        object Not(object filter);

        /// <summary>
        /// Создает фильтр логического сложения (OR).
        /// </summary>
        object Or(params object[] filters);

        /// <summary>
        /// Создает фильтр логического сложения (OR).
        /// </summary>
        object Or(IEnumerable<object> filters);

        /// <summary>
        /// Создает фильтр логического умножения (AND).
        /// </summary>
        object And(params object[] filters);

        /// <summary>
        /// Создает фильтр логического умножения (AND).
        /// </summary>
        object And(IEnumerable<object> filters);


        /// <summary>
        /// Создает фильтр, проверяющий наличие свойства в документе.
        /// </summary>
        object Exists(string property, bool exists = true);

        /// <summary>
        /// Создает фильтр, проверяющий, что тип значения свойства документа равен указанному.
        /// </summary>
        object Type(string property, DataType valueType);


        /// <summary>
        /// Создает фильтр, проверяющий, что значение свойства документа входит в указанное множество.
        /// </summary>
        object In<TProperty>(string property, IEnumerable<TProperty> values);

        /// <summary>
        /// Создает фильтр, проверяющий, что значение свойства документа не входит в указанное множество.
        /// </summary>
        object NotIn<TProperty>(string property, IEnumerable<TProperty> values);

        /// <summary>
        /// Создает фильтр, проверяющий, что значение свойства документа равно (==) указанному значению.
        /// </summary>
        object Eq<TProperty>(string property, TProperty value);

        /// <summary>
        /// Создает фильтр, проверяющий, что значение свойства документа не равно (!=) указанному значению.
        /// </summary>
        object NotEq<TProperty>(string property, TProperty value);

        /// <summary>
        /// Создает фильтр, проверяющий, что значение свойства документа больше (&gt;) указанного значения.
        /// </summary>
        object Gt<TProperty>(string property, TProperty value);

        /// <summary>
        /// Создает фильтр, проверяющий, что значение свойства документа больше или равно (&gt;=) указанному значению.
        /// </summary>
        object Gte<TProperty>(string property, TProperty value);

        /// <summary>
        /// Создает фильтр, проверяющий, что значение свойства документа меньше (&lt;) указанного значения.
        /// </summary>
        object Lt<TProperty>(string property, TProperty value);

        /// <summary>
        /// Создает фильтр, проверяющий, что значение свойства документа меньше или равно (&lt;=) указанному значению.
        /// </summary>
        object Lte<TProperty>(string property, TProperty value);

        /// <summary>
        /// Создает фильтр, проверяющий, что значение свойства документа удовлетворяет указанному регулярному выражению.
        /// </summary>
        object Regex(string property, Regex value);

        /// <summary>
        /// Создает фильтр, проверяющий, что значение свойства документа начинается указанной подстрокой.
        /// </summary>
        object StartsWith(string property, string value, bool ignoreCase = true);

        /// <summary>
        /// Создает фильтр, проверяющий, что значение свойства документа заканчивается указанной подстрокой.
        /// </summary>
        object EndsWith(string property, string value, bool ignoreCase = true);

        /// <summary>
        /// Создает фильтр, проверяющий, что значение свойства документа содержит указанную подстроку.
        /// </summary>
        object Contains(string property, string value, bool ignoreCase = true);

        /// <summary>
        /// Создает фильтр, проверяющий, что свойство является массивом, который содержит хотя бы один элемент, удовлетворяющий указанному фильтру.
        /// </summary>
        object Match(string arrayProperty, object filter);

        /// <summary>
        /// Создает фильтр, проверяющий, что свойство является массивом, который содержит элементы, входящие в указанное множество.
        /// </summary>
        object All<TItem>(string arrayProperty, IEnumerable<TItem> items);

        /// <summary>
        /// Создает фильтр, проверяющий, что свойство является массивом, который содержит хотя бы один элемент, входящий в указанное множество.
        /// </summary>
        object AnyIn<TItem>(string arrayProperty, IEnumerable<TItem> items);

        /// <summary>
        /// Создает фильтр, проверяющий, что свойство является массивом, который содержит хотя бы один элемент, не входящий в указанное множество.
        /// </summary>
        object AnyNotIn<TItem>(string arrayProperty, IEnumerable<TItem> items);

        /// <summary>
        /// Создает фильтр, проверяющий, что свойство является массивом, который содержит хотя бы один элемент, который равен (==) указанному значению.
        /// </summary>
        object AnyEq<TItem>(string arrayProperty, TItem item);

        /// <summary>
        /// Создает фильтр, проверяющий, что свойство является массивом, который содержит хотя бы один элемент, который не равен (!=) указанному значению.
        /// </summary>
        object AnyNotEq<TItem>(string arrayProperty, TItem item);

        /// <summary>
        /// Создает фильтр, проверяющий, что свойство является массивом, который содержит хотя бы один элемент, который больше (&gt;) указанного значения.
        /// </summary>
        object AnyGt<TItem>(string arrayProperty, TItem item);

        /// <summary>
        /// Создает фильтр, проверяющий, что свойство является массивом, который содержит хотя бы один элемент, который больше или равен (&gt;=) указанному значению.
        /// </summary>
        object AnyGte<TItem>(string arrayProperty, TItem item);

        /// <summary>
        /// Создает фильтр, проверяющий, что свойство является массивом, который содержит хотя бы один элемент, который меньше (&lt;) указанного значения.
        /// </summary>
        object AnyLt<TItem>(string arrayProperty, TItem item);

        /// <summary>
        /// Создает фильтр, проверяющий, что свойство является массивом, который содержит хотя бы один элемент, который меньше или равен (&lt;=) указанному значению.
        /// </summary>
        object AnyLte<TItem>(string arrayProperty, TItem item);


        /// <summary>
        /// Создает фильтр, проверяющий, что свойство является массивом, размер которого равен указанному значению.
        /// </summary>
        object SizeEq(string arrayProperty, int value);

        /// <summary>
        /// Создает фильтр, проверяющий, что свойство является массивом, размер которого больше (&gt;) указанного значения.
        /// </summary>
        object SizeGt(string arrayProperty, int value);

        /// <summary>
        /// Создает фильтр, проверяющий, что свойство является массивом, размер которого больше или равно (&gt;=) указанному значению.
        /// </summary>
        object SizeGte(string arrayProperty, int value);

        /// <summary>
        /// Создает фильтр, проверяющий, что свойство является массивом, размер которого меньше (&lt;) указанного значения.
        /// </summary>
        object SizeLt(string arrayProperty, int value);

        /// <summary>
        /// Создает фильтр, проверяющий, что свойство является массивом, размер которого меньше или равно (&lt;=) указанному значению.
        /// </summary>
        object SizeLte(string arrayProperty, int value);


        /// <summary>
        /// Создает фильтр, проверяющий, что документ удовлетворяет условию полнотекстового поиска.
        /// </summary>
        object Text(string search, string language = null, bool caseSensitive = false, bool diacriticSensitive = false);
    }
}
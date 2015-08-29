using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using InfinniPlatform.Sdk.Environment.Index;

namespace InfinniPlatform.Sdk.ApiContracts
{
    public interface IRegisterApi
    {
        /// <summary>
        ///     Получение результата агрегации по регистру на определенную дату
        /// </summary>
        /// <param name="configuration">Конфигурация, содержащая регистр</param>
        /// <param name="register">Имя регистра, по которому необходимо выполнить агрегацию</param>
        /// <param name="endDate">Дата, на которую проводить агрегацию</param>
        /// <param name="dimensions">
        ///     Список измерений для агрегации. Если параметр не задан,
        ///     в качестве измерений будут взяты все свойства регистра, помеченные как Dimension
        /// </param>
        /// <param name="valueProperties">
        ///     Свойства, по которомым будут вычислены агрегирующие значения.
        ///     Если параметр не задан, в качестве значения будет взяты свойства регистра, помеченные как Value
        /// </param>
        /// <param name="valueAggregationTypes">Тип агрегации по значениям (сумма, среднее и тд)</param>
        /// <param name="filter">Фильтр для отбора определенных значений из регистра</param>
        /// <returns>Результат агрегации</returns>
        IEnumerable<object> GetValuesByDate(string configuration,
            string register,
            DateTime endDate,
            IEnumerable<string> dimensions = null,
            IEnumerable<string> valueProperties = null,
            IEnumerable<AggregationType> valueAggregationTypes = null,
            Action<FilterBuilder> filter = null);

        /// <summary>
        ///     Получение результата агрегации по регистру в период между двумя датами
        /// </summary>
        /// <param name="configuration">Конфигурация, содержащая регистр</param>
        /// <param name="register">Имя регистра, по которому необходимо выполнить агрегацию</param>
        /// <param name="startDate">Начальная дата агрегации</param>
        /// <param name="endDate">Конечная дата агрегации</param>
        /// <param name="dimensions">
        ///     Список измерений для агрегации. Если параметр не задан,
        ///     в качестве измерений будут взяты все свойства регистра, помеченные как Dimension
        /// </param>
        /// <param name="valueProperties">
        ///     Свойства, по которомым будут вычислены агрегирующие значения.
        ///     Если параметр не задан, в качестве значения будет взяты свойства регистра, помеченные как Value
        /// </param>
        /// <param name="valueAggregationTypes">Тип агрегации по значениям (сумма, среднее и тд)</param>
        /// <param name="filter">Фильтр для отбора определенных значений из регистра</param>
        /// <returns>Результат агрегации</returns>
        IEnumerable<object> GetValuesBetweenDates(
            string configuration,
            string register,
            DateTime startDate,
            DateTime endDate,
            IEnumerable<string> dimensions = null,
            IEnumerable<string> valueProperties = null,
            IEnumerable<AggregationType> valueAggregationTypes = null,
            Action<FilterBuilder> filter = null);

        /// <summary>
        ///     Получение 'сырых' данных из регистра
        /// </summary>
        /// <param name="configuration">Конфигурация, содержащая регистр</param>
        /// <param name="register">Имя регистра, по которому необходимо выполнить агрегацию</param>
        /// <param name="filter">Фильтр для отбора определенных значений из регистра</param>
        /// <param name="pageNumber">Номер страницы</param>
        /// <param name="pageSize">Размер страницы</param>
        /// <returns>Набор записей регистра</returns>
        IEnumerable<object> GetRegisterEntries(
            string configuration,
            string register,
            Action<FilterBuilder> filter,
            int pageNumber,
            int pageSize);
    }
}

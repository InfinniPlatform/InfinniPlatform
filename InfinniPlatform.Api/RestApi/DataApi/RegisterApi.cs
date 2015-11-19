using System;
using System.Collections.Generic;
using InfinniPlatform.Api.Registers;
using InfinniPlatform.Api.RestApi.CommonApi;
using InfinniPlatform.Api.RestQuery;
using InfinniPlatform.Api.SearchOptions;
using InfinniPlatform.Api.SearchOptions.Builders;
using InfinniPlatform.Sdk.Dynamic;
using InfinniPlatform.Sdk.Environment.Index;
using InfinniPlatform.Sdk.Environment.Register;
using NUnit.Framework;

namespace InfinniPlatform.Api.RestApi.DataApi
{
    public sealed class RegisterApi
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
        public IEnumerable<dynamic> GetValuesByDate(
            string configuration,
            string register,
            DateTime endDate,
            IEnumerable<string> dimensions = null,
            IEnumerable<string> valueProperties = null,
            IEnumerable<AggregationType> valueAggregationTypes = null,
            Action<FilterBuilder> filter = null)
        {
            var filterBuilder = new FilterBuilder();

            if (filter != null)
            {
                filter.Invoke(filterBuilder);
            }

            return GetValuesByDate(configuration, register, endDate, dimensions, valueProperties, valueAggregationTypes, filter == null ? null : filterBuilder.GetFilter());
        }

        public IEnumerable<dynamic> GetValuesByDate(string configuration, string register, DateTime endDate, IEnumerable<string> dimensions, IEnumerable<string> valueProperties, IEnumerable<AggregationType> valueAggregationTypes, IEnumerable<dynamic> filter)
        {
            return RestQueryApi.QueryPostJsonRaw("SystemConfig", "metadata", "GetRegisterValuesByDate", null, new
            {
                Configuration = configuration,
                Register = register,
                Date = endDate,
                Dimensions = dimensions,
                ValueProperties = valueProperties,
                ValueAggregationTypes = valueAggregationTypes,
                Filter = filter
            }).ToDynamicList();
        }

        public IEnumerable<dynamic> GetValuesBetweenDates(
            string configuration,
            string register,
            DateTime startDate,
            DateTime endDate)
        {
            var response = GetValuesBetweenDates(configuration, register, startDate, endDate, null, null, null, new List<dynamic>() );

            return response;
        }

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
        public IEnumerable<dynamic> GetValuesBetweenDates(
            string configuration,
            string register,
            DateTime startDate,
            DateTime endDate,
            IEnumerable<string> dimensions = null,
            IEnumerable<string> valueProperties = null,
            IEnumerable<AggregationType> valueAggregationTypes = null,
            Action<FilterBuilder> filter = null)
        {
            var filterBuilder = new FilterBuilder();

            if (filter != null)
            {
                filter.Invoke(filterBuilder);
            }

            var response = GetValuesBetweenDates(configuration, register, startDate, endDate, dimensions,
                valueProperties, valueAggregationTypes, filter == null ? null : filterBuilder.GetFilter());

            return response;
        }

        public IEnumerable<dynamic> GetValuesBetweenDates(
            string configuration,
            string register,
            DateTime startDate,
            DateTime endDate,
            IEnumerable<string> dimensions = null,
            IEnumerable<string> valueProperties = null,
            IEnumerable<AggregationType> valueAggregationTypes = null,
            IEnumerable<dynamic> filter = null)
        {
            return RestQueryApi.QueryPostJsonRaw("SystemConfig", "metadata", "GetRegisterValuesBetweenDates", null, new
            {
                Configuration = configuration,
                Register = register,
                FromDate = startDate,
                ToDate = endDate,
                Dimensions = dimensions,
                ValueProperties = valueProperties,
                ValueAggregationTypes = valueAggregationTypes,
                Filter = filter
            }).ToDynamicList();             
        }

        public IEnumerable<dynamic> GetValuesByPeriods(
            string configuration,
            string register,
            DateTime startDate,
            DateTime endDate,
            string interval)
        {
            return GetValuesByPeriods(configuration, register, startDate, endDate, interval, null, null, null, new List<dynamic>());
        }

        /// <summary>
        ///     Получение результата агрегации по регистру с разбиением по периодам
        /// </summary>
        /// <param name="configuration">Конфигурация, содержащая регистр</param>
        /// <param name="register">Имя регистра, по которому необходимо выполнить агрегацию</param>
        /// <param name="startDate">Начальная дата агрегации</param>
        /// <param name="endDate">Конечная дата агрегации</param>
        /// <param name="interval">Интервал агрегации (Year, quarter, month, week, day, hour, minute or second)</param>
        /// <param name="dimensions">
        ///     Список измерений для агрегации. Если параметр не задан,
        ///     в качестве измерений будут взяты все свойства регистра, помеченные как Dimension
        /// </param>
        /// <param name="valueProperties">
        ///     Свойства, по которомым будут вычислены агрегирующие значения.
        ///     Если параметр не задан, в качестве значения будет взяты свойства регистра, помеченные как Value
        /// </param>
        /// <param name="timezone">Временная зона (часовой пояс) в формате "+05:00"</param>
        /// <param name="filter">Фильтр для отбора определенных значений из регистра</param>
        /// <returns>Результат агрегации</returns>
        public IEnumerable<dynamic> GetValuesByPeriods(
            string configuration,
            string register,
            DateTime startDate,
            DateTime endDate,
            string interval,
            IEnumerable<string> dimensions = null,
            IEnumerable<string> valueProperties = null,
            string timezone = null,
            Action<FilterBuilder> filter = null)
        {
            var filterBuilder = new FilterBuilder();

            if (filter != null)
            {
                filter.Invoke(filterBuilder);
            }

            return GetValuesByPeriods(configuration, register, startDate, endDate, interval, dimensions, valueProperties,
                timezone, filter == null ? null : filterBuilder.GetFilter());
        }


        public IEnumerable<dynamic> GetValuesByPeriods(
            string configuration,
            string register,
            DateTime startDate,
            DateTime endDate,
            string interval,
            IEnumerable<string> dimensions = null,
            IEnumerable<string> valueProperties = null,
            string timezone = null,
            IEnumerable<dynamic> filter = null)
        {
            var response = RestQueryApi.QueryPostJsonRaw("SystemConfig", "metadata", "GetRegisterValuesByPeriods", null, new
            {
                Configuration = configuration,
                Register = register,
                FromDate = startDate,
                ToDate = endDate,
                Interval = interval,
                Dimensions = dimensions,
                ValueProperty = valueProperties,
                TimeZone = timezone,
                Filter = filter
            });

            IEnumerable<dynamic> result;

            try
            {
                result = response.ToDynamicList();
            }
            catch (Exception)
            {
                result = new List<dynamic>(new[] { response });
            }

            return result;
        }
              
        /// <summary>
        ///     Получение результата агрегации по документу-регистратору
        /// </summary>
        /// <param name="configuration">Конфигурация, содержащая регистр</param>
        /// <param name="register">Имя регистра, по которому необходимо выполнить агрегацию</param>
        /// <param name="registrar">Идентификатор регистратора</param>
        /// <param name="dimensions">
        ///     Список измерений для агрегации. Если параметр не задан,
        ///     в качестве измерений будут взяты все свойства регистра, помеченные как Dimension
        /// </param>
        /// <param name="valueProperties">
        ///     Свойства, по которомым будут вычислены агрегирующие значения.
        ///     Если параметр не задан, в качестве значения будет взяты свойства регистра, помеченные как Value
        /// </param>
        /// <returns>Результат агрегации</returns>
        public IEnumerable<dynamic> GetValuesBуRegistrar(
            string configuration,
            string register,
            string registrar,
            IEnumerable<string> dimensions = null,
            IEnumerable<string> valueProperties = null)
        {
            var response =
                RestQueryApi.QueryPostJsonRaw("SystemConfig", "metadata", "GetRegisterValuesByRegistrar", null, new
                    {
                        Configuration = configuration,
                        Register = register,
                        Registrar = registrar,
                        Dimensions = dimensions,
                        ValueProperties = valueProperties
                    });

            return response.ToDynamicList();
        }

        /// <summary>
        ///     Получение результата агрегации по типу документа-регистратора
        /// </summary>
        /// <param name="configuration">Конфигурация, содержащая регистр</param>
        /// <param name="register">Имя регистра, по которому необходимо выполнить агрегацию</param>
        /// <param name="registrarType">Тип документа-регистратора</param>
        /// <param name="dimensions">
        ///     Список измерений для агрегации. Если параметр не задан,
        ///     в качестве измерений будут взяты все свойства регистра, помеченные как Dimension
        /// </param>
        /// <param name="valueProperties">
        ///     Свойства, по которомым будут вычислены агрегирующие значения.
        ///     Если параметр не задан, в качестве значения будет взяты свойства регистра, помеченные как Value
        /// </param>
        /// <returns>Результат агрегации</returns>
        public IEnumerable<dynamic> GetValuesBуRegistrarType(
            string configuration,
            string register,
            string registrarType,
            IEnumerable<string> dimensions = null,
            IEnumerable<string> valueProperties = null)
        {
            var response =
                RestQueryApi.QueryPostJsonRaw("SystemConfig", "metadata", "GetRegisterValuesByRegistrarType", null, new
                    {
                        Configuration = configuration,
                        Register = register,
                        RegistrarType = registrarType,
                        Dimensions = dimensions,
                        ValueProperties = valueProperties
                    });

            return response.ToDynamicList();
        }

        /// <summary>
        ///     Получение 'сырых' данных из регистра
        /// </summary>
        /// <param name="configuration">Конфигурация, содержащая регистр</param>
        /// <param name="register">Имя регистра, по которому необходимо выполнить агрегацию</param>
        /// <param name="filter">Фильтр для отбора определенных значений из регистра</param>
        /// <param name="pageNumber">Номер страницы</param>
        /// <param name="pageSize">Размер страницы</param>
        /// <returns>Набор записей регистра</returns>
        public IEnumerable<dynamic> GetRegisterEntries(
            string configuration,
            string register,
            Action<FilterBuilder> filter,
            int pageNumber,
            int pageSize)
        {
            var filterBuilder = new FilterBuilder();

            if (filter != null)
            {
                filter.Invoke(filterBuilder);
            }

            return GetRegisterEntries(configuration, register, filterBuilder.GetFilter(), pageNumber, pageSize);
        }

        public IEnumerable<dynamic> GetRegisterEntries(string configuration,
            string register,
            IEnumerable<dynamic> filter,
            int pageNumber,
            int pageSize)
        {
            var response = RestQueryApi.QueryPostJsonRaw("RestfulApi", "configuration", "getdocument", null, new
            {
                Configuration = configuration,
                Metadata = RegisterConstants.RegisterNamePrefix + register,
                Filter = filter,
                PageNumber = pageNumber,
                PageSize = pageSize
            });

            return response.ToDynamicList();
        } 

        /// <summary>
        ///     Получение значений из таблицы итогов на дату, ближайшую к заданной
        /// </summary>
        /// <param name="configuration">Конфигурация, содержащая регистр</param>
        /// <param name="register">Имя регистра, по которому необходимо выполнить агрегацию</param>
        /// <param name="totalsDate">Дата для расчета итогов</param>
        /// <returns>Набор записей регистра итогов</returns>
        public IEnumerable<dynamic> GetRegisterTotals(
            string configuration,
            string register,
            DateTime totalsDate)
        {
            var closestDate =
                RestQueryApi.QueryPostJsonRaw("SystemConfig", "metadata", "GetClosestDateTimeOfTotalCalculation", null,
                    new
                        {
                            Configuration = configuration,
                            Register = register,
                            Date = totalsDate
                        }).ToDynamic();

            if (closestDate != null)
            {
                return new DocumentApi().GetDocument(
                    configuration,
                    RegisterConstants.RegisterTotalNamePrefix + register,
                    f => f.AddCriteria(
                        c => c.Property(RegisterConstants.DocumentDateProperty).IsEquals(closestDate.Date)), 0, 10000);
            }

            return new List<dynamic>();
        }
    }
}
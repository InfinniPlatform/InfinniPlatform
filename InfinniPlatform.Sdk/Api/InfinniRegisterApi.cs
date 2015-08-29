﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using InfinniPlatform.Sdk.ApiContracts;
using InfinniPlatform.Sdk.Environment.Index;
using InfinniPlatform.Sdk.Environment.Register;

namespace InfinniPlatform.Sdk.Api
{
    public sealed class InfinniRegisterApi : BaseApi, IRegisterApi
    {
        private readonly InfinniCustomServiceApi _customServiceApi;
        private readonly InfinniDocumentApi _documentApi;

        public InfinniRegisterApi(string server, string port, string route)
            : base(server, port, route)
        {
            _customServiceApi = new InfinniCustomServiceApi(server,port,route);
            _documentApi = new InfinniDocumentApi(server, port, route);
        }

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
        public IEnumerable<dynamic> GetValuesByDate(string configuration,
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

            return 
                _customServiceApi.ExecuteAction("SystemConfig", "metadata", "GetRegisterValuesByDate", new
                {
                    Configuration = configuration,
                    Register = register,
                    Date = endDate,
                    Dimensions = dimensions,
                    ValueProperties = valueProperties,
                    ValueAggregationTypes = valueAggregationTypes,
                    Filter = filter == null ? null : filterBuilder.GetFilter()
                });


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

            return 
                _customServiceApi.ExecuteAction("SystemConfig", "metadata", "GetRegisterValuesBetweenDates", new
                {
                    Configuration = configuration,
                    Register = register,
                    FromDate = startDate,
                    ToDate = endDate,
                    Dimensions = dimensions,
                    ValueProperties = valueProperties,
                    ValueAggregationTypes = valueAggregationTypes,
                    Filter = filter == null ? null : filterBuilder.GetFilter()
                });

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
            return _documentApi.GetDocument(configuration, RegisterConstants.RegisterNamePrefix + register, filter,
                pageNumber, pageSize);

        }
    }
}

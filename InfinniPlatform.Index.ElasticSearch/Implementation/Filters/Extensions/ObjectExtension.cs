using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using InfinniPlatform.Api.Dynamic;

namespace InfinniPlatform.Index.ElasticSearch.Implementation.Filters.Extensions
{
	public static class ObjectExtension
	{
        /// <summary>
        /// Приведение объекта к строковому значению, понятному ElasticSearch
        /// </summary>
		public static string AsElasticValue(this object filterValue)
		{
			if (filterValue is DateTime)
			{
				return ((DateTime)filterValue).ToString("yyyy'-'MM'-'dd'T'HH':'mm':'ss'.'fff'Z'");
			}

            if (filterValue is double || filterValue is float)
            {
                // В качестве разделителя Elastic Search ожидает точку
                var culture = new CultureInfo("en-US")
                {
                    NumberFormat = new NumberFormatInfo
                    {
                        NumberDecimalSeparator = "."
                    }
                };
                
                var output = ((double)filterValue).ToString(culture);
                return output;
            }

            var enumerable = filterValue as IEnumerable<dynamic>;
            if (enumerable != null)
            {
                return enumerable.DynamicEnumerableToString();
            }

			//поскольку установлен режим поиска по умолчанию lower_case (в настройках конфига elasticsearch - config.yml), приводим к нижнему регистру строку запроса
			return filterValue != null ? filterValue.ToString().ToLowerInvariant() : "";
		}
	}
}
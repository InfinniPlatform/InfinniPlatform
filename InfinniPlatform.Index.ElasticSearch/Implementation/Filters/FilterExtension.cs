using System;
using System.Globalization;

namespace InfinniPlatform.Index.ElasticSearch.Implementation.Filters
{
	public static class FilterExtension
	{
		public static string ConvertToElasticSearchFilterValue(object filterValue)
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
                
                return ((double)filterValue).ToString(culture);
            }

			//поскольку установлен режим поиска по умолчанию lower_case (в настройках конфига elasticsearch - config.yml), приводим к нижнему регистру строку запроса
			return filterValue != null ? filterValue.ToString().ToLowerInvariant() : "";
		}
	}
}
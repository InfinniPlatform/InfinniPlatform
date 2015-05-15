using System;
using System.Collections.Generic;
using System.Text;

using FastReport;
using FastReport.Data;

using InfinniPlatform.FastReport.Properties;
using InfinniPlatform.FastReport.Templates.Data;

namespace InfinniPlatform.FastReport.ReportTemplateBuilders.Data
{
	static class DataSourceBindHelper
	{
		/// <summary>
		/// Получить ссылку на коллекцию источника данных.
		/// </summary>
		public static CollectionBind GetCollectionBind(Column dataSource)
		{
			if (dataSource == null)
			{
				throw new ArgumentNullException();
			}

			var rootDataSource = dataSource;

			var propertyPath = new Stack<string>();

			Column parentColumn = dataSource;

			while (parentColumn != null)
			{
				rootDataSource = parentColumn;

				if (parentColumn is DataSourceBase)
				{
					propertyPath.Push("$");
				}

				propertyPath.Push(rootDataSource.Name);

				parentColumn = parentColumn.Parent as Column;
			}

			propertyPath.Pop();
			propertyPath.Pop();

			return new CollectionBind
					   {
						   DataSource = rootDataSource.Name,
						   Property = string.Join(".", propertyPath)
					   };
		}

		/// <summary>
		/// Получить ссылку на свойство источника данных.
		/// </summary>
		public static PropertyBind GetPropertyBind(Report report, string expression)
		{
			// Проверка формата выражения

			var propertyPath = (expression ?? string.Empty).TrimStart('[').TrimEnd(']').Split('.');

			if (propertyPath.Length <= 0)
			{
				throw new ArgumentException(string.Format(Resources.ExpressionHasIncorrectFormat, expression));
			}

			// Поиск подходящего источника данных

			Column rootDataSource = report.Dictionary.DataSources.FindByName(propertyPath[0]);

			if (rootDataSource == null)
			{
				throw new ArgumentException(string.Format(Resources.ExpressionReferencedOnUnknownDataSource, expression, propertyPath[0]));
			}

			// Раскладывание выражения по свойствам источника данных

			var result = new StringBuilder();

			var parentColumn = rootDataSource;
			var propertyColumn = rootDataSource;

			for (var i = 1; i < propertyPath.Length; ++i)
			{
				var propertyName = propertyPath[i];

				propertyColumn = parentColumn.Columns.FindByName(propertyName);

				if (propertyColumn == null)
				{
					throw new ArgumentException(string.Format(Resources.ExpressionReferencedOnUnknownProperty, expression, propertyName));
				}

				result.Append('.');
				result.Append(propertyName);

				if (propertyColumn is DataSourceBase)
				{
					result.Append(".$");
				}

				parentColumn = propertyColumn;
			}

			if (propertyColumn is DataSourceBase || parentColumn.DataType == typeof(object))
			{
				throw new ArgumentException(string.Format(Resources.ExpressionShouldReferencedOnProperty, expression));
			}

			result.Remove(0, 1);


			return new PropertyBind
					   {
						   DataSource = rootDataSource.Name,
						   Property = result.ToString()
					   };
		}
	}
}
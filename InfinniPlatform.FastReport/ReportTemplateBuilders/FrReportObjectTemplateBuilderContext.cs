using System;
using System.Collections;
using System.Collections.Generic;

namespace InfinniPlatform.FastReport.ReportTemplateBuilders
{
	/// <summary>
	/// Контекст построителя шаблона объекта отчета FastReport.
	/// </summary>
	sealed class FrReportObjectTemplateBuilderContext : IReportObjectTemplateBuilderContext
	{
		/// <summary>
		/// Объект отчета.
		/// </summary>
		public object Report { get; set; }


		private readonly Dictionary<Type, Func<object, object>> _builders
			= new Dictionary<Type, Func<object, object>>();

		/// <summary>
		/// Зарегистрировать построитель шаблона объекта отчета.
		/// </summary>
		/// <typeparam name="TTemplate">Тип шаблона объекта отчета.</typeparam>
		/// <param name="builder">Построитель шаблона объекта отчета.</param>
		public void RegisterBuilder<TTemplate>(IReportObjectTemplateBuilder<TTemplate> builder)
		{
			_builders.Add(typeof(TTemplate), reportObject => builder.BuildTemplate(this, reportObject));
		}

		/// <summary>
		/// Построить шаблон для объекта отчета.
		/// </summary>
		/// <typeparam name="TTemplate">Тип шаблона объекта отчета.</typeparam>
		/// <param name="reportObject">Экземпляр объекта отчета.</param>
		public TTemplate BuildTemplate<TTemplate>(object reportObject)
		{
			if (reportObject != null)
			{
				Func<object, object> builder;

				if (_builders.TryGetValue(typeof(TTemplate), out builder))
				{
					return (TTemplate)builder(reportObject);
				}
			}

			return default(TTemplate);
		}

		/// <summary>
		/// Построить шаблоны для объектов отчета.
		/// </summary>
		/// <typeparam name="TTemplate">Тип шаблона объекта отчета.</typeparam>
		/// <param name="reportObjects">Список экземпляров объектов отчета.</param>
		public ICollection<TTemplate> BuildTemplates<TTemplate>(IEnumerable reportObjects)
		{
			if (reportObjects != null)
			{
				var result = new List<TTemplate>();

				foreach (var reportObject in reportObjects)
				{
					var template = BuildTemplate<TTemplate>(reportObject);

					if (ReferenceEquals(template, null) == false)
					{
						result.Add(template);
					}
				}

				return result;
			}

			return null;
		}
	}
}
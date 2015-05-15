using System.Collections;
using System.Collections.Generic;

namespace InfinniPlatform.FastReport.ReportTemplateBuilders
{
	/// <summary>
	/// Контекст построителя шаблона объекта отчета.
	/// </summary>
	public interface IReportObjectTemplateBuilderContext
	{
		/// <summary>
		/// Объект отчета.
		/// </summary>
		object Report { get; }

		/// <summary>
		/// Построить шаблон для объекта отчета.
		/// </summary>
		/// <typeparam name="TTemplate">Тип шаблона объекта отчета.</typeparam>
		/// <param name="reportObject">Экземпляр объекта отчета.</param>
		TTemplate BuildTemplate<TTemplate>(object reportObject);

		/// <summary>
		/// Построить шаблоны для объектов отчета.
		/// </summary>
		/// <typeparam name="TTemplate">Тип шаблона объекта отчета.</typeparam>
		/// <param name="reportObjects">Список экземпляров объектов отчета.</param>
		ICollection<TTemplate> BuildTemplates<TTemplate>(IEnumerable reportObjects);
	}
}
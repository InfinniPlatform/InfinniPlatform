using System.Collections;

namespace InfinniPlatform.FastReport.ReportObjectBuilders
{
	/// <summary>
	/// Контекст построителя объекта отчета.
	/// </summary>
	public interface IReportObjectBuilderContext
	{
		/// <summary>
		/// Построенный отчет.
		/// </summary>
		object Report { get; set; }

		/// <summary>
		/// Создать объект отчета.
		/// </summary>
		/// <typeparam name="T">Тип объекта отчета.</typeparam>
		T CreateObject<T>() where T : new();

		/// <summary>
		/// Построить объект отчета по шаблону.
		/// </summary>
		/// <param name="template">Шаблон объекта отчета.</param>
		/// <param name="parent">Родительский объект отчета.</param>
		void BuildObject(object template, object parent);

		/// <summary>
		/// Построить объекты отчета по шаблонам.
		/// </summary>
		/// <param name="templates">Список шаблонов объекта отчета.</param>
		/// <param name="parent">Родительский объект отчета.</param>
		void BuildObjects(IEnumerable templates, object parent);
	}
}
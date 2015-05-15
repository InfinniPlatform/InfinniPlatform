namespace InfinniPlatform.FastReport.ReportTemplateBuilders
{
	/// <summary>
	/// Построитель шаблона объекта отчета.
	/// </summary>
	/// <typeparam name="TTemplate">Тип шаблона объекта отчета.</typeparam>
	public interface IReportObjectTemplateBuilder<out TTemplate>
	{
		/// <summary>
		/// Построить шаблон для объекта отчета.
		/// </summary>
		/// <param name="context">Контекст построителя шаблона объекта отчета.</param>
		/// <param name="reportObject">Экземпляр объекта отчета.</param>
		/// <returns>Шаблон объекта отчета.</returns>
		TTemplate BuildTemplate(IReportObjectTemplateBuilderContext context, object reportObject);
	}
}
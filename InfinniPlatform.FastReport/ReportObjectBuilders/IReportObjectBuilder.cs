namespace InfinniPlatform.FastReport.ReportObjectBuilders
{
	/// <summary>
	/// Построитель объекта отчета.
	/// </summary>
	/// <typeparam name="TTemplate">Тип шаблона объекта отчета.</typeparam>
	public interface IReportObjectBuilder<in TTemplate>
	{
		/// <summary>
		/// Построить объект отчета по шаблону.
		/// </summary>
		/// <param name="context">Контекст построителя объекта отчета.</param>
		/// <param name="template">Шаблон объекта отчета.</param>
		/// <param name="parent">Родительский объект отчета.</param>
		void BuildObject(IReportObjectBuilderContext context, TTemplate template, object parent);
	}
}
namespace InfinniPlatform.FastReport.ReportObjectBuilders
{
	/// <summary>
	/// Фабрика для создания контекста построителя объекта отчета.
	/// </summary>
	public interface IReportObjectBuilderContextFactory
	{
		/// <summary>
		/// Создать контекст.
		/// </summary>
		IReportObjectBuilderContext CreateContext();
	}
}
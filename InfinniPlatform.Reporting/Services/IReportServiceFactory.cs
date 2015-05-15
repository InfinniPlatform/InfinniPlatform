namespace InfinniPlatform.Reporting.Services
{
	/// <summary>
	/// Фабрика для создания инфраструктурных сервисов подсистемы отчетов.
	/// </summary>
	public interface IReportServiceFactory
	{
		/// <summary>
		/// Создать сервис для работы с подсистемой отчетов.
		/// </summary>
		IReportService CreateReportService();

		/// <summary>
		/// Создать сервис для работы с хранилищем шаблонов отчетов.
		/// </summary>
		IReportTemplateRepository CreateReportTemplateRepository();
	}
}
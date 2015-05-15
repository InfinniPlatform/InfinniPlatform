namespace InfinniPlatform.FastReport.TemplatesFluent.Reports
{
	/// <summary>
	/// Статический класс для создания конструктора шаблона отчета
	/// </summary>
	public static class ReportTemplateFluent
	{
		/// <summary>
		/// Создать конструктор шаблона отчета
		/// </summary>
		/// <param name="name">Наименование отчета.</param>
		public static ReportTemplateConfig Report(string name)
		{
			return new ReportTemplateConfig(name);
		}
	}
}
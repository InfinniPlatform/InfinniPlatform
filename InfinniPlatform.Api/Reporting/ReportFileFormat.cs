namespace InfinniPlatform.Api.Reporting
{
	/// <summary>
	/// Формат файла отчета.
	/// </summary>
	public enum ReportFileFormat
	{
		/// <summary>
		/// Portable Document Format (PDF).
		/// </summary>
		Pdf = 0,


		/// <summary>
		/// OpenDocument Text (ODT).
		/// </summary>
		Odt = 1,

		/// <summary>
		/// OpenDocument Spreadsheet (ODS).
		/// </summary>
		Ods = 2,


		/// <summary>
		/// Office Open XML Word (DOCX).
		/// </summary>
		Docx = 4,

		/// <summary>
		/// Office Open XML Excel (XLSX).
		/// </summary>
		Xlsx = 8,


		/// <summary>
		/// Rich Text Format (RTF).
		/// </summary>
		Rtf = 16,

		/// <summary>
		/// Comma-Separated Values (CSV).
		/// </summary>
		Csv = 32,

		/// <summary>
		/// Extensible Markup Language (XML).
		/// </summary>
		Xml = 64,
	}
}
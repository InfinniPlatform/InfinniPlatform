namespace InfinniPlatform.Api.PrintView
{
	/// <summary>
	/// Формат файла печатного представления.
	/// </summary>
	public enum PrintViewFileFormat
	{
		/// <summary>
		/// Portable Document Format (PDF).
		/// </summary>
		Pdf = 0,

		/// <summary>
		/// XML Paper Specification (XPS).
		/// </summary>
		Xps = 1,

		/// <summary>
		/// Rich Text Format (RTF).
		/// </summary>
		Rtf = 2,

		/// <summary>
		/// Extensible Markup Language (XML).
		/// </summary>
		Xml = 4,
	}
}
using InfinniPlatform.FastReport.Templates.Data;
using InfinniPlatform.FastReport.Templates.Elements;

namespace InfinniPlatform.FastReport.Templates.Bands
{
	/// <summary>
	/// Блок группировки данных отчета.
	/// </summary>
	public sealed class ReportGroupBand : IDataBindElement
	{
		/// <summary>
		/// Привязка данных.
		/// </summary>
		public IDataBind DataBind { get; set; }


		/// <summary>
		/// Заголовок блока.
		/// </summary>
		public ReportBand Header { get; set; }

		/// <summary>
		/// Итоги блока.
		/// </summary>
		public ReportBand Footer { get; set; }
	}
}
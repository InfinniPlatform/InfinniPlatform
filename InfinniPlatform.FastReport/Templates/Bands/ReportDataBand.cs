using System.Collections.Generic;

using InfinniPlatform.FastReport.Templates.Data;

namespace InfinniPlatform.FastReport.Templates.Bands
{
	/// <summary>
	/// Блок данных отчета.
	/// </summary>
	public sealed class ReportDataBand
	{
		/// <summary>
		/// Привязка данных.
		/// </summary>
		public CollectionBind DataBind { get; set; }


		/// <summary>
		/// Содержимое блока.
		/// </summary>
		public ReportBand Content { get; set; }

		/// <summary>
		/// Вложенный блок данных.
		/// </summary>
		public ReportDataBand Details { get; set; }

		/// <summary>
		/// Блок группировки данных.
		/// </summary>
		public ICollection<ReportGroupBand> Groups { get; set; }
	}
}
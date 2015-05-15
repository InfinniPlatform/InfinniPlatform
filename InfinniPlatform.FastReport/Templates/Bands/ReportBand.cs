using System.Collections.Generic;

using InfinniPlatform.FastReport.Templates.Borders;
using InfinniPlatform.FastReport.Templates.Elements;

namespace InfinniPlatform.FastReport.Templates.Bands
{
	/// <summary>
	/// Блок отчета.
	/// </summary>
	public sealed class ReportBand
	{
		/// <summary>
		/// Наименование блока.
		/// </summary>
		public string Name { get; set; }


		/// <summary>
		/// Границы блока.
		/// </summary>
		public Border Border { get; set; }

		/// <summary>
		/// Настройки печати.
		/// </summary>
		public ReportBandPrintSetup PrintSetup { get; set; }


		/// <summary>
		/// Содержимое блока.
		/// </summary>
		public ICollection<IElement> Elements { get; set; }


		/// <summary>
		/// Высота блока.
		/// </summary>
		/// <remarks>Измеряется в миллиметрах.</remarks>
		public float Height { get; set; }

		/// <summary>
		/// Может увеличиваться.
		/// </summary>
		public bool CanGrow { get; set; }

		/// <summary>
		/// Может сокращаться.
		/// </summary>
		public bool CanShrink { get; set; }
	}
}
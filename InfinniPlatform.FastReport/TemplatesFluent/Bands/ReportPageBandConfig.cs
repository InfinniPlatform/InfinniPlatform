using System;

using InfinniPlatform.FastReport.Templates.Bands;

namespace InfinniPlatform.FastReport.TemplatesFluent.Bands
{
	/// <summary>
	/// Интерфейс для настройки страницы отчета.
	/// </summary>
	public sealed class ReportPageBandConfig
	{
		internal ReportPageBandConfig(ReportPageBand pageBand)
		{
			if (pageBand == null)
			{
				throw new ArgumentNullException("pageBand");
			}

			_pageBand = pageBand;
		}


		private readonly ReportPageBand _pageBand;


		/// <summary>
		/// Заголовок блока.
		/// </summary>
		public ReportPageBandConfig Header(Action<ReportBandConfig> action)
		{
			if (_pageBand.Header == null)
			{
				_pageBand.Header = new ReportBand();
			}

			var configuration = new ReportBandConfig(_pageBand.Header);
			action(configuration);

			return this;
		}

		/// <summary>
		/// Итоги блока.
		/// </summary>
		public ReportPageBandConfig Footer(Action<ReportBandConfig> action)
		{
			if (_pageBand.Footer == null)
			{
				_pageBand.Footer = new ReportBand();
			}

			var configuration = new ReportBandConfig(_pageBand.Footer);
			action(configuration);

			return this;
		}
	}
}
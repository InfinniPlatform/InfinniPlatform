using System;

using InfinniPlatform.FastReport.Templates.Bands;
using InfinniPlatform.FastReport.Templates.Data;

namespace InfinniPlatform.FastReport.TemplatesFluent.Bands
{
	/// <summary>
	/// Интерфейс для настройки блока группировки данных отчета.
	/// </summary>
	public sealed class ReportGroupBandConfig
	{
		internal ReportGroupBandConfig(string dataSourceName, string propertyPath, ReportGroupBand groupBand)
		{
			if (string.IsNullOrWhiteSpace(dataSourceName))
			{
				throw new ArgumentNullException("dataSourceName");
			}

			if (string.IsNullOrWhiteSpace(propertyPath))
			{
				throw new ArgumentNullException("propertyPath");
			}

			if (groupBand == null)
			{
				throw new ArgumentNullException("groupBand");
			}

			groupBand.DataBind = new PropertyBind { DataSource = dataSourceName, Property = propertyPath };

			_groupBand = groupBand;
		}


		private readonly ReportGroupBand _groupBand;


		/// <summary>
		/// Заголовок блока.
		/// </summary>
		public ReportGroupBandConfig Header(Action<ReportBandConfig> action)
		{
			if (_groupBand.Header == null)
			{
				_groupBand.Header = new ReportBand();
			}

			var configuration = new ReportBandConfig(_groupBand.Header);
			action(configuration);

			return this;
		}

		/// <summary>
		/// Итоги блока.
		/// </summary>
		public ReportGroupBandConfig Footer(Action<ReportBandConfig> action)
		{
			if (_groupBand.Footer == null)
			{
				_groupBand.Footer = new ReportBand();
			}

			var configuration = new ReportBandConfig(_groupBand.Footer);
			action(configuration);

			return this;
		}
	}
}
using System;
using System.Collections.Generic;

using InfinniPlatform.FastReport.Templates.Bands;
using InfinniPlatform.FastReport.Templates.Borders;
using InfinniPlatform.FastReport.Templates.Elements;
using InfinniPlatform.FastReport.TemplatesFluent.Borders;

namespace InfinniPlatform.FastReport.TemplatesFluent.Bands
{
	/// <summary>
	/// Интерфейс для настройки блока отчета.
	/// </summary>
	public sealed class ReportBandConfig
	{
		internal ReportBandConfig(ReportBand band)
		{
			if (band == null)
			{
				throw new ArgumentNullException("band");
			}

			_band = band;
		}


		private readonly ReportBand _band;


		/// <summary>
		/// Наименование блока.
		/// </summary>
		public ReportBandConfig Name(string value)
		{
			if (string.IsNullOrWhiteSpace(value))
			{
				throw new ArgumentNullException("value");
			}

			_band.Name = value;

			return this;
		}

		/// <summary>
		/// Высота блока.
		/// </summary>
		/// <exception cref="ArgumentOutOfRangeException"></exception>
		public ReportBandConfig Height(float value)
		{
			if (value < 0)
			{
				throw new ArgumentOutOfRangeException("value");
			}

			_band.Height = value;

			return this;
		}

		/// <summary>
		/// Границы блока.
		/// </summary>
		public ReportBandConfig Border(Action<BorderConfig> action)
		{
			if (_band.Border == null)
			{
				_band.Border = new Border();
			}

			var configuration = new BorderConfig(_band.Border);
			action(configuration);

			return this;
		}

		/// <summary>
		/// Настройки печати.
		/// </summary>
		public ReportBandConfig PrintSetup(Action<ReportBandPrintSetupConfig> action)
		{
			if (_band.PrintSetup == null)
			{
				_band.PrintSetup = new ReportBandPrintSetup();
			}

			var configuration = new ReportBandPrintSetupConfig(_band.PrintSetup);
			action(configuration);

			return this;
		}

		/// <summary>
		/// Содержимое блока.
		/// </summary>
		public ReportBandConfig Elements(Action<ReportBandElementsConfig> action)
		{
			if (_band.Elements == null)
			{
				_band.Elements = new List<IElement>();
			}

			var configuration = new ReportBandElementsConfig(_band.Elements);
			action(configuration);

			return this;
		}

		/// <summary>
		/// Может увеличиваться.
		/// </summary>
		public ReportBandConfig CanGrow()
		{
			_band.CanGrow = true;

			return this;
		}

		/// <summary>
		/// Может сокращаться.
		/// </summary>
		public ReportBandConfig CanShrink()
		{
			_band.CanShrink = true;

			return this;
		}
	}
}
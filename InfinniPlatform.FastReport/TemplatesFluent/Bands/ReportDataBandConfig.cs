using System;
using System.Collections.Generic;

using InfinniPlatform.FastReport.Templates.Bands;
using InfinniPlatform.FastReport.Templates.Data;

namespace InfinniPlatform.FastReport.TemplatesFluent.Bands
{
	/// <summary>
	/// Интерфейс для настройки блока данных отчета.
	/// </summary>
	public sealed class ReportDataBandConfig
	{
		internal ReportDataBandConfig(string dataSourceName, string collectionPath, ReportDataBand dataBand)
		{
			if (string.IsNullOrWhiteSpace(dataSourceName))
			{
				throw new ArgumentNullException("dataSourceName");
			}

			if (dataBand == null)
			{
				throw new ArgumentNullException("dataBand");
			}

			dataBand.DataBind = new CollectionBind { DataSource = dataSourceName, Property = collectionPath };

			_dataSourceName = dataSourceName;
			_dataBand = dataBand;
		}


		private readonly string _dataSourceName;
		private readonly ReportDataBand _dataBand;


		/// <summary>
		/// Содержимое блока.
		/// </summary>
		public ReportDataBandConfig Content(Action<ReportBandConfig> action)
		{
			if (_dataBand.Content == null)
			{
				_dataBand.Content = new ReportBand();
			}

			var configuration = new ReportBandConfig(_dataBand.Content);
			action(configuration);

			return this;
		}

		/// <summary>
		/// Вложенный блок данных.
		/// </summary>
		public ReportDataBandConfig Details(string collectionPath, Action<ReportDataBandConfig> action)
		{
			if (string.IsNullOrWhiteSpace(collectionPath))
			{
				throw new ArgumentNullException("collectionPath");
			}

			if (_dataBand.Details == null)
			{
				_dataBand.Details = new ReportDataBand();
			}

			var configuration = new ReportDataBandConfig(_dataSourceName, collectionPath, _dataBand.Details);
			action(configuration);

			return this;
		}

		/// <summary>
		/// Блок группировки данных.
		/// </summary>
		public ReportDataBandConfig Group(string propertyPath, Action<ReportGroupBandConfig> action)
		{
			if (string.IsNullOrWhiteSpace(propertyPath))
			{
				throw new ArgumentNullException("propertyPath");
			}

			if (_dataBand.Groups == null)
			{
				_dataBand.Groups = new List<ReportGroupBand>();
			}

			var group = new ReportGroupBand();

			var configuration = new ReportGroupBandConfig(_dataSourceName, propertyPath, group);
			action(configuration);

			_dataBand.Groups.Add(group);

			return this;
		}

		/// <summary>
		/// Сортировка источника данных по возрастанию.
		/// </summary>
		public ReportDataBandConfig OrderBy(string propertyPath)
		{
			var collectionBind = _dataBand.DataBind;

			if (collectionBind.SortFields == null)
			{
				collectionBind.SortFields = new List<SortField>();
			}

			collectionBind.SortFields.Add(new SortField { Property = propertyPath, SortOrder = SortOrder.Ascending });

			return this;
		}

		/// <summary>
		/// Сортировка источника данных по убыванию.
		/// </summary>
		public ReportDataBandConfig OrderByDescending(string propertyPath)
		{
			var collectionBind = _dataBand.DataBind;

			if (collectionBind.SortFields == null)
			{
				collectionBind.SortFields = new List<SortField>();
			}

			collectionBind.SortFields.Add(new SortField { Property = propertyPath, SortOrder = SortOrder.Descending });

			return this;
		}
	}
}
using System;
using System.Collections.Generic;

using InfinniPlatform.FastReport.Templates.Data;

namespace InfinniPlatform.FastReport.TemplatesFluent.Data
{
	/// <summary>
	/// Интерфейс для настройки итогов блока данных отчета.
	/// </summary>
	public sealed class TotalsConfig
	{
		internal TotalsConfig(string dataBandName, ICollection<TotalInfo> totals)
		{
			if (string.IsNullOrWhiteSpace(dataBandName))
			{
				throw new ArgumentNullException("dataBandName");
			}

			if (totals == null)
			{
				throw new ArgumentNullException("totals");
			}

			_dataBandName = dataBandName;
			_totals = totals;
		}


		private readonly string _dataBandName;
		private readonly ICollection<TotalInfo> _totals;


		/// <summary>
		/// Сумма.
		/// </summary>
		public TotalsConfig Sum(string name, string printBandName, Action<DataBindConfig> dataBind)
		{
			return RegisterTotal(TotalFunc.Sum, name, printBandName, dataBind);
		}

		/// <summary>
		/// Минимум.
		/// </summary>
		public TotalsConfig Min(string name, string printBandName, Action<DataBindConfig> dataBind)
		{
			return RegisterTotal(TotalFunc.Min, name, printBandName, dataBind);
		}

		/// <summary>
		/// Максимум.
		/// </summary>
		public TotalsConfig Max(string name, string printBandName, Action<DataBindConfig> dataBind)
		{
			return RegisterTotal(TotalFunc.Max, name, printBandName, dataBind);
		}

		/// <summary>
		/// Среднее.
		/// </summary>
		public TotalsConfig Avg(string name, string printBandName, Action<DataBindConfig> dataBind)
		{
			return RegisterTotal(TotalFunc.Avg, name, printBandName, dataBind);
		}

		/// <summary>
		/// Количество.
		/// </summary>
		public TotalsConfig Count(string name, string printBandName)
		{
			return RegisterTotal(TotalFunc.Count, name, printBandName, null);
		}


		private TotalsConfig RegisterTotal(TotalFunc totalFunc, string name, string printBandName, Action<DataBindConfig> dataBind)
		{
			if (string.IsNullOrWhiteSpace(name))
			{
				throw new ArgumentNullException("name");
			}

			if (string.IsNullOrWhiteSpace(printBandName))
			{
				throw new ArgumentNullException("printBandName");
			}

			var totalInfo = new TotalInfo
								{
									Name = name,
									DataBand = _dataBandName,
									PrintBand = printBandName,
									TotalFunc = totalFunc
								};

			if (dataBind != null)
			{
				dataBind(new DataBindConfig(expression => totalInfo.Expression = expression));
			}

			_totals.Add(totalInfo);

			return this;
		}
	}
}
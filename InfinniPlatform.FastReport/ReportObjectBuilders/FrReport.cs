using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;

using FastReport;
using FastReport.Export;
using FastReport.Export.Csv;
using FastReport.Export.Odf;
using FastReport.Export.OoXML;
using FastReport.Export.Pdf;
using FastReport.Export.RichText;
using FastReport.Export.Xml;

using InfinniPlatform.Core.Reporting;
using InfinniPlatform.FastReport.Properties;
using InfinniPlatform.FastReport.Templates.Data;

namespace InfinniPlatform.FastReport.ReportObjectBuilders
{
	/// <summary>
	/// Отчет FastReport.
	/// </summary>
	sealed class FrReport : IReport
	{
		public FrReport(Report report)
		{
			if (report == null)
			{
				throw new ArgumentNullException("report");
			}

			_report = report;
		}


		private Report _report;


		/// <summary>
		/// Объект отчета.
		/// </summary>
		public object Object
		{
			get { return _report; }
		}

		/// <summary>
		/// Задать значение параметра.
		/// </summary>
		/// <param name="name">Наименование параметра.</param>
		/// <param name="value">Значение параметра.</param>
		public void SetParameter(string name, object value)
		{
			_report.SetParameterValue(name, value);
		}

		/// <summary>
		/// Задать источник данных.
		/// </summary>
		/// <param name="name">Наименование источника данных.</param>
		/// <param name="data">Коллекция данных источника данных.</param>
		public void SetDataSource(string name, IEnumerable data)
		{
			var dataSource = _report.Dictionary.FindDataComponent(name);

			if (dataSource != null)
			{
				dataSource.Reference = data;
			}
		}

		/// <summary>
		/// Создать файл отчета.
		/// </summary>
		/// <param name="format">Формат файла отчета.</param>
		/// <returns>Данные файла.</returns>
		public byte[] CreateFile(ReportFileFormat format)
		{
			_report.Prepare();

			using (var result = new MemoryStream())
			{
				var exporter = CreateFileExporter(format);

				_report.Export(exporter, result);

				return result.ToArray();
			}
		}

		private static ExportBase CreateFileExporter(ReportFileFormat fileFormat)
		{
			Func<ExportBase> factory;

			if (FileExporters.TryGetValue(fileFormat, out factory) == false)
			{
				throw new NotSupportedException(string.Format(Resources.NotSupportedReportFileFormat, fileFormat));
			}

			var exporter = factory();

			exporter.ShowProgress = false;

			return exporter;
		}


		private static readonly Dictionary<ReportFileFormat, Func<ExportBase>> FileExporters
			= new Dictionary<ReportFileFormat, Func<ExportBase>>
				  {
					  { ReportFileFormat.Pdf, () => new PDFExport() },
					  { ReportFileFormat.Odt, () => new ODTExport() },
					  { ReportFileFormat.Ods, () => new ODSExport() },
					  { ReportFileFormat.Rtf, () => new RTFExport() },
					  { ReportFileFormat.Csv, () => new CSVExport() },
					  { ReportFileFormat.Xml, () => new XMLExport() },
					  { ReportFileFormat.Docx, () => new Word2007Export() },
					  { ReportFileFormat.Xlsx, () => new Excel2007Export() },
				  };


		public void Dispose()
		{
			var report = _report;

			if (report != null)
			{
				report.Dispose();

				_report = null;
			}
		}
	}
}
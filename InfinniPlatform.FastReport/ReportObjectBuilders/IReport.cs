using System;
using System.Collections;

using InfinniPlatform.Core.Reporting;
using InfinniPlatform.FastReport.Templates.Data;

namespace InfinniPlatform.FastReport.ReportObjectBuilders
{
	/// <summary>
	/// Отчет.
	/// </summary>
	public interface IReport : IDisposable
	{
		/// <summary>
		/// Объект отчета.
		/// </summary>
		object Object { get; }

		/// <summary>
		/// Задать значение параметра.
		/// </summary>
		/// <param name="name">Наименование параметра.</param>
		/// <param name="value">Значение параметра.</param>
		void SetParameter(string name, object value);

		/// <summary>
		/// Задать источник данных.
		/// </summary>
		/// <param name="name">Наименование источника данных.</param>
		/// <param name="data">Коллекция данных источника данных.</param>
		void SetDataSource(string name, IEnumerable data);

		/// <summary>
		/// Создать файл отчета.
		/// </summary>
		/// <param name="format">Формат файла отчета.</param>
		/// <returns>Данные файла.</returns>
		byte[] CreateFile(ReportFileFormat format);
	}
}
using InfinniPlatform.FastReport.Templates.Data;

namespace InfinniPlatform.FastReport.Templates.Elements
{
	/// <summary>
	/// Элемент с поддержкой привязки данных.
	/// </summary>
	public interface IDataBindElement
	{
		/// <summary>
		/// Привязка данных.
		/// </summary>
		IDataBind DataBind { get; set; }
	}
}
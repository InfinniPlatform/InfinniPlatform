namespace InfinniPlatform.FastReport.Templates.Formats
{
	/// <summary>
	/// Формат отображения денежных единиц.
	/// </summary>
	public sealed class CurrencyFormat : NumberFormat
	{
		/// <summary>
		/// Символ валюты.
		/// </summary>
		public string CurrencySymbol { get; set; }
	}
}
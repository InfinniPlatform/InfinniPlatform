namespace InfinniPlatform.Sdk.Contracts
{
	public interface IUrlEncodedDataContext : ICommonContext
	{
		/// <summary>
		///   Результат обработки
		/// </summary>
		dynamic Result { get; set; }

		/// <summary>
		///  Список параметров формы
		/// </summary>
		dynamic FormData { get; set; }
	}
}

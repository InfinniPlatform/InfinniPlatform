using System.IO;

namespace InfinniPlatform.Sdk.Contracts
{
	public interface IUploadContext : ICommonContext
	{
		Stream FileContent { get; set; }

		/// <summary>
		///   Результат обработки
		/// </summary>
		dynamic Result { get; set; }

		/// <summary>
		///  Связанные с файлом данные
		/// </summary>
		dynamic LinkedData { get; set; }
	}
}

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InfinniPlatform.Api.ContextTypes
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

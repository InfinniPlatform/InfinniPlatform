using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InfinniPlatform.Api.ContextTypes
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

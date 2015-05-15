using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InfinniPlatform.Api.ContextTypes
{
	/// <summary>
	///   Виды типов контекста точек расширения
	/// </summary>
	public enum ContextTypeKind
	{
		None = 1, //не является точкой расширения
		ApplyMove = 2, //точка расширения применения изменений
		ApplyFilter = 4, //точка расширения применения фильтра
		ApplyResult = 8, //точка расширения получения результата применения изменений
		SearchContext = 16, //точка расширения результата поиска
		Upload = 32, //точка расширения загрузки файла
		UrlEncodedData = 64 //точка расширения для загрузки параметров формы (key-value)
	}
}

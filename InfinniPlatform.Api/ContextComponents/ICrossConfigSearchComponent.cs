using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using InfinniPlatform.Api.Factories;

namespace InfinniPlatform.Api.ContextComponents
{
	/// <summary>
	///   Компонент для получения документов из различных конфигураций внутри глобального контекста
	/// </summary>
	public interface ICrossConfigSearchComponent
	{
		ICrossConfigSearcher GetCrossConfigSearcher();
	}
}

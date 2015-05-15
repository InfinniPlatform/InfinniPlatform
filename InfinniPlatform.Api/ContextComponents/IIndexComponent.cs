using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using InfinniPlatform.Api.Factories;
using InfinniPlatform.Api.Index;

namespace InfinniPlatform.Api.ContextComponents
{
	/// <summary>
	///  Компонент для работы с индексами в глобальном контексте
	/// </summary>
	public interface IIndexComponent
	{
		IIndexFactory IndexFactory { get; }
	}
}

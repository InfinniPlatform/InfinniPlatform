using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using InfinniPlatform.Api.Factories;

namespace InfinniPlatform.Api.ContextComponents
{
	/// <summary>
	///   Компонент для хранилища, работающего с бинарными данными
	/// </summary>
	public interface IBlobStorageComponent
	{
		/// <summary>
		///   Получить хранилище бинарных данных
		/// </summary>
		/// <returns>Хранилище бинарных данных</returns>
		IBlobStorage GetBlobStorage();
	}
}

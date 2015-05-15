using System;
using System.Collections.Generic;
using InfinniPlatform.Api.ClientNotification;
using InfinniPlatform.Api.Metadata;
using InfinniPlatform.Api.Profiling;
using InfinniPlatform.Api.Transactions;

namespace InfinniPlatform.Api.Context
{
	/// <summary>
	///   Глобальный контекст выполнения скрипта
	/// </summary>
	public interface IGlobalContext
	{
		/// <summary>
		///   Получить компонент, реализующий тип Т из глоабльного контекста
		/// </summary>
		/// <typeparam name="T">Тип ожидаемого контракта</typeparam>
		/// <returns>Экземпляр контракта</returns>
		T GetComponent<T>() where T:class;

	}
}

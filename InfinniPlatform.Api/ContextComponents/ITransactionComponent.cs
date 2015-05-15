using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using InfinniPlatform.Api.Transactions;

namespace InfinniPlatform.Api.ContextComponents
{
	/// <summary>
	///   Компонент выполнения транзакций в глоабльном контексте
	/// </summary>
	public interface ITransactionComponent
	{
		/// <summary>
		///   Получить менеджер транзакций
		/// </summary>
		/// <returns>Менеджер транзакций</returns>
		ITransactionManager GetTransactionManager();
	}
}

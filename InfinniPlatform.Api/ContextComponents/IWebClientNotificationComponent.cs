using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InfinniPlatform.Api.ContextComponents
{
	/// <summary>
	///   Компонент для клиентской нотификации
	/// </summary>
	public interface IWebClientNotificationComponent
	{
		void Notify(string routingKey, object message);
	}
}

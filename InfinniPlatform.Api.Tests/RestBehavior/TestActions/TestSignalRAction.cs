using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using InfinniPlatform.Api.ContextComponents;
using InfinniPlatform.Api.ContextTypes;
using InfinniPlatform.Api.Dynamic;

namespace InfinniPlatform.Api.Tests.RestBehavior.TestActions
{
	public sealed class TestSignalRAction
	{
		public void Action(IApplyContext target)
		{
			dynamic testObject = new DynamicWrapper();
			testObject.TestProperty = "Hello world";

			target.Context.GetComponent<IWebClientNotificationComponent>().Notify("routingKey", testObject.ToString());
			
		}
	}
}

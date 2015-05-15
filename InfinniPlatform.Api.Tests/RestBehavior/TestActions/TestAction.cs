using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using InfinniPlatform.Api.ContextTypes;

namespace InfinniPlatform.Api.Tests.RestBehavior.TestActions
{
	public sealed class TestAction
	{
		public void Action(IApplyContext target)
		{
			target.Item.TestValue = "Test";
			target.Result = target.Item;
		}
	}
}

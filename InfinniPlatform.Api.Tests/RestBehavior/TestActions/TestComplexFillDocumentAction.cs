using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using InfinniPlatform.Api.ContextTypes;

namespace InfinniPlatform.Api.Tests.RestBehavior.TestActions
{
	public sealed class TestComplexFillDocumentAction
	{
		public void Action(IApplyContext target)
		{
			target.Item.PrefiledField = "TestValue";
		}
	}
}

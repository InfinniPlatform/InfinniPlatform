using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using InfinniPlatform.Api.ContextTypes;

namespace InfinniPlatform.Api.Tests.RestBehavior.TestActions
{
	public sealed class TestComplexValidator
	{
		public void Validate(IApplyContext target)
		{
			target.IsValid = false;
			target.ValidationMessage = "testmessage";
		}
	}
}

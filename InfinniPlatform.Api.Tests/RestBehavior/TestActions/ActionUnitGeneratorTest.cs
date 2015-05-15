using InfinniPlatform.Api.ContextTypes;
using InfinniPlatform.Api.Dynamic;

namespace InfinniPlatform.Api.Tests.RestBehavior.TestActions
{
	public sealed class ActionUnitGeneratorTest
	{
		public void Action(IApplyContext target)
		{
			target.Result = new DynamicWrapper();
			target.Result.TestValue = "Test";
		}
	}
}

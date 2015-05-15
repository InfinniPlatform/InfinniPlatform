using InfinniPlatform.Api.Actions;

namespace InfinniPlatform.Core.Tests.Events
{

	public class TestStatusUpdater : IActionOperator
	{
	    public string Description
	    {
	        get { return "TestStatusUpdater"; }
	    }

	    public void Action(dynamic entity)
		{
			entity.Item.TestField = "test";
		}
	}

    public class TestSavedStatusUpdater : IActionOperator
	{
        public string Description
        {
            get { return "TestSavedStatusUpdater"; }
        }

        public void Action(dynamic entity)
		{
			entity.Item.TestFieldSaved = "testFieldSaved";
		}
	}

    public class TestPublishedStatusUpdater : IActionOperator
	{
        public string Description
        {
            get { return "TestPublishedStatusUpdater"; }
        }

        public void Action(dynamic entity)
		{
			entity.Item.TestFieldPublished = "testFieldPublished";
		}
	}


}

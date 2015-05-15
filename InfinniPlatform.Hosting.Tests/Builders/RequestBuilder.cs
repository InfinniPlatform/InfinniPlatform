using System;
using System.Collections.Generic;

namespace InfinniPlatform.Hosting.WebApi.Tests.Builders
{
	public static class RequestBuilder
	{
		public static List<string> BuildDeleteArguments()
		{
			return new List<string>() {"10", "20", "30"};
		}

		public static object BuildTestVerbProcessorComplexArguments()
		{
			return new Dictionary<string, object>()
				       {
					       {"argument1", "test1"},
					       {"argument2", 1},
					       {"argument3", 1.232},
					       {"argument4", new[] {new TestPerson() {FirstName = "test1"}}},
					       {
						       "argument5",
						       new TestPerson() {FirstName = "blahblah", LastName = "uhuhu"}
					       },
					       {"argument7", "True"},
					       {"argument8", new Guid("10CE0436-2F62-4389-8FB8-0692D9DC6291")}
				       };
			                     
	    }

        public static object BuildSomeServicesCheckRequestAnother()
        {
	        return new Dictionary<string, object>()
		               {
			               {"argument1", "test1"},
		               };
        }

        public static object BuildSomeServicesCheckRequestAnother2()
        {

	        return new Dictionary<string, object>()
		               {
			               {"argument2", "test1"},
		               };

        }
    }
}

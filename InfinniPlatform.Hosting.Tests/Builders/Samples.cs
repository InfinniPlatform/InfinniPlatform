using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace InfinniPlatform.Hosting.WebApi.Tests.Builders
{

	public class TestResponseController : ApiController
	{

		public HttpResponseMessage GetData()
		{
			var resp = new HttpResponseMessage(HttpStatusCode.OK);
			return resp;
		}
	}

	public class NoVerbProcessorQuery
	{
		
	}

    public class TestConfigInstallerHttpQuery
    {

        public object ProcessTestGetVerb()
        {
            return "GET";
        }
    }


	public class TestVerbProcessorHttpQuery 
	{

		public object ProcessTestGetVerb(string argument1, int argument2, double argument3, IEnumerable<TestPerson> argument4, TestPerson argument5, bool argument7, Guid argument8)
		{
			return "GET";
		}


		public object ProcessTestPutVerb(string argument1, int argument2, double argument3, IEnumerable<TestPerson> argument4, TestPerson argument5, bool argument7, Guid argument8)
		{
			return "PUT";
		}

		public object ProcessTestPostVerb(string argument1, int argument2, double argument3, IEnumerable<TestPerson> argument4, TestPerson argument5, bool argument7, Guid argument8)
		{
			return "POST";
		}

		public object ProcessTestDeleteVerb(IEnumerable<string> paramsToDelete)
		{
			return "DELETE";
		}
	}


    public class AnotherVerbProcessorWithoutServiceNameHttpQuery
    {
        public object ProcessTestPostVerb(string argument1)
        {
            return "POST_WITHOUTSERVICENAME";
        }
    }

    public class AnotherVerbProcessorHttpQuery
    {
        public object ProcessTestPostVerb(string argument1)
        {
            return "POST2";
        }
    }


    public class AnotherVerbProcessor2HttpQuery
    {
        public object ProcessTestGetVerb(string argument2)
        {
            return "POST3";
        }
    }


}

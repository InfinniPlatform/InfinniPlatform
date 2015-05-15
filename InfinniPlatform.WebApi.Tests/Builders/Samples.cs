using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace InfinniPlatform.WebApi.Tests.Builders
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

		public object ProcessTestGetVerb(string argument1, int argument2, double argument3, IEnumerable<TestPerson> argument4, TestPerson argument5, bool argument7, Guid argument8, bool testDefaultValue = true)
		{
			return "GET";
		}


		public object ProcessTestPutVerb(string argument1, int argument2, double argument3, IEnumerable<TestPerson> argument4, TestPerson argument5, bool argument7, Guid argument8, bool testDefaultValue = true)
		{
			return "PUT";
		}

		public object ProcessTestPostVerb(string argument1, int argument2, double argument3, IEnumerable<TestPerson> argument4, TestPerson argument5, bool argument7, Guid argument8, bool testDefaultValue = true)
		{
			return "POST";
		}

		public object ProcessTestDeleteVerb(IEnumerable<string> paramsToDelete)
		{
			return "DELETE";
		}

	}

    public class TestVerbProcessorUploadQuery
    {
        public object ProcessStreamPostVerb(object linkedData, Stream uploadStream)
        {
            using (Stream file = File.Create(@"TestData\CheckInput.txt"))
            {
                CopyStream(uploadStream, file);
            }
            return string.Format("STREAM_{0}",linkedData);
        }

        private static void CopyStream(Stream input, Stream output)
        {
            byte[] buffer = new byte[8 * 1024];
            int len;
            while ((len = input.Read(buffer, 0, buffer.Length)) > 0)
            {
                output.Write(buffer, 0, len);
            }
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

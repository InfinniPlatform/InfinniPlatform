using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using InfinniPlatform.Sdk.Api;

namespace InfinniPlatform.Sdk.Global
{
    public sealed class ScriptContext
    {

        public InfinniAuthApi GetAuthApi(string server, string port)
        {
            return new InfinniAuthApi(server, port);
        }

        public InfinniCustomServiceApi GetCustomServiceApi(string server, string port)
        {
            return new InfinniCustomServiceApi(server, port);
        }

        public InfinniDocumentApi GetDocumentApi(string server, string port)
        {
            return new InfinniDocumentApi(server,port);
        }

        public InfinniFileApi GetFileApi(string server, string port)
        {
            return new InfinniFileApi(server,port);
        }

        public InfinniSignInApi GetSignInApi(string server, string port)
        {
            return new InfinniSignInApi(server, port);
        }
    }
}

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

        public InfinniAuthApi GetAuthApi(string server, string port, string route)
        {
            return new InfinniAuthApi(server, port, route);
        }

        public InfinniCustomServiceApi GetCustomServiceApi(string server, string port, string route)
        {
            return new InfinniCustomServiceApi(server, port, route);
        }

        public InfinniDocumentApi GetDocumentApi(string server, string port, string route)
        {
            return new InfinniDocumentApi(server,port,route);
        }

        public InfinniFileApi GetFileApi(string server, string port, string route)
        {
            return new InfinniFileApi(server,port,route);
        }

        public InfinniSignInApi GetSignInApi(string server, string port, string route)
        {
            return new InfinniSignInApi(server, port, route);
        }
    }
}

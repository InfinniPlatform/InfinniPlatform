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
        private readonly string _version;

        public ScriptContext(string version)
        {
            _version = version;
        }

        public InfinniAuthApi GetAuthApi(string server, string port)
        {
            return new InfinniAuthApi(server, port,_version);
        }

        public InfinniCustomServiceApi GetCustomServiceApi(string server, string port)
        {
            return new InfinniCustomServiceApi(server, port,_version);
        }

        public InfinniDocumentApi GetDocumentApi(string server, string port)
        {
            return new InfinniDocumentApi(server,port,_version);
        }

        public InfinniFileApi GetFileApi(string server, string port)
        {
            return new InfinniFileApi(server,port,_version);
        }

        public InfinniSignInApi GetSignInApi(string server, string port)
        {
            return new InfinniSignInApi(server, port,_version);
        }
    }
}

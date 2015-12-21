using InfinniPlatform.Sdk.Api;

namespace InfinniPlatform.Sdk.Global
{
    public sealed class ScriptContext
    {
        public InfinniAuthApi GetAuthApi(string server, int port, string route)
        {
            return new InfinniAuthApi(server, port);
        }

        public InfinniCustomServiceApi GetCustomServiceApi(string server, int port, string route)
        {
            return new InfinniCustomServiceApi(server, port);
        }

        public InfinniDocumentApi GetDocumentApi(string server, int port, string route)
        {
            return new InfinniDocumentApi(server, port);
        }

        public InfinniFileApi GetFileApi(string server, int port, string route)
        {
            return new InfinniFileApi(server, port);
        }

        public InfinniSignInApi GetSignInApi(string server, int port, string route)
        {
            return new InfinniSignInApi(server, port);
        }

        public InfinniRegisterApi GetRegisterApi(string server, int port, string route)
        {
            return new InfinniRegisterApi(server, port);
        }

        public InfinniVersionApi GetVersionApi(string server, int port, string route)
        {
            return new InfinniVersionApi(server, port);
        }

        public InfinniMetadataApi GetMetadataApi(string server, int port, string route)
        {
            return new InfinniMetadataApi(server, port);
        }
    }
}
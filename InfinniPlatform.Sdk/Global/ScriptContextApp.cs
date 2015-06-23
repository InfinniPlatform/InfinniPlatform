using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using InfinniPlatform.Sdk.Api;

namespace InfinniPlatform.Sdk.Global
{
    /// <summary>
    ///   Контекст прикладного скрипта, инициализируемый
    ///   через файл конфигурации SDK
    /// </summary>
    public sealed class ScriptContextApp
    {
        private readonly string _version;

        private readonly string _server;

        private readonly string _port;

        public ScriptContextApp(string version)
        {
            _version = version;
            _server = HostingConfig.Default.ServerName;
            _port = HostingConfig.Default.ServerPort.ToString();
        }

        public InfinniAuthApi GetAuthApi()
        {
            return new InfinniAuthApi(_server, _port, _version);
        }

        public InfinniCustomServiceApi GetCustomServiceApi()
        {
            return new InfinniCustomServiceApi(_server, _port, _version);
        }

        public InfinniDocumentApi GetDocumentApi()
        {
            return new InfinniDocumentApi(_server, _port, _version);
        }

        public InfinniFileApi GetFileApi()
        {
            return new InfinniFileApi(_server, _port, _version);
        }

        public InfinniSignInApi GetSignInApi()
        {
            return new InfinniSignInApi(_server, _port, _version);
        }
    }
}

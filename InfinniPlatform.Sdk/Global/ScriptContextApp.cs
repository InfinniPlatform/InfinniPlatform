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
        private readonly string _server;

        private readonly string _port;

        private readonly string _route;

        public ScriptContextApp()
        {
            _server = HostingConfig.Default.ServerName;
            _port = HostingConfig.Default.ServerPort.ToString();
            _route = "1"; //добавить в конфиг идентификатор роутинга масштабирования

        }

        public InfinniAuthApi GetAuthApi()
        {
            return new InfinniAuthApi(_server, _port, _route);
        }

        public InfinniCustomServiceApi GetCustomServiceApi()
        {
            return new InfinniCustomServiceApi(_server, _port, _route);
        }

        public InfinniDocumentApi GetDocumentApi()
        {
            return new InfinniDocumentApi(_server, _port, _route);
        }

        public InfinniFileApi GetFileApi()
        {
            return new InfinniFileApi(_server, _port, _route);
        }

        public InfinniSignInApi GetSignInApi()
        {
            return new InfinniSignInApi(_server, _port, _route);
        }

        public InfinniRegisterApi GetRegisterApi()
        {
            return new InfinniRegisterApi(_server, _port, _route);
        }

        public InfinniMetadataApi GetMetadataApi()
        {
            return new InfinniMetadataApi(_server, _port, _route);
        }

        public InfinniVersionApi GetVersionApi()
        {
            return new InfinniVersionApi(_server,_port, _route);
        }

    }
}

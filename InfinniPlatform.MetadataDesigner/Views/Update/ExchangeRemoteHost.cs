using System.Windows.Forms;

using InfinniPlatform.Core.RestQuery;
using InfinniPlatform.Sdk.Api;

namespace InfinniPlatform.MetadataDesigner.Views.Update
{
    public sealed class ExchangeRemoteHost : IUpdatePrepareConfig
    {
        private readonly string _version;

        public ExchangeRemoteHost(HostingConfig hostingConfig, string version)
        {
            _version = version;
            HostingConfig = hostingConfig;
        }


        public HostingConfig HostingConfig { get; private set; }

        public string Version
        {
            get { return _version; }
        }


        public bool PrepareRoutingOperation()
        {
            if (string.IsNullOrEmpty(HostingConfig.Scheme) || string.IsNullOrEmpty(HostingConfig.Name) || HostingConfig.Port <= 0)
            {
                if (MessageBox.Show(@"Не указана схема, сервер или порт для обновления. Обновить конфигурацию локально?", @"Внимание!",
                                    MessageBoxButtons.YesNo) == DialogResult.No)
                {
                    return false;
                }

                HostingConfig = HostingConfig.Default;
            }


			ControllerRoutingFactory.Instance = new ControllerRoutingFactory(HostingConfig.Default);

            return true;
        }
    }
}
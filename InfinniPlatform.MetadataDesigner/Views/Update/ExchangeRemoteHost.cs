using System.Windows.Forms;

using InfinniPlatform.Api.Hosting;
using InfinniPlatform.Api.TestEnvironment;

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
            if (string.IsNullOrEmpty(HostingConfig.ServerScheme) || string.IsNullOrEmpty(HostingConfig.ServerName) || HostingConfig.ServerPort <= 0)
            {
                if (MessageBox.Show(@"Не указана схема, сервер или порт для обновления. Обновить конфигурацию локально?", @"Внимание!",
                                    MessageBoxButtons.YesNo) == DialogResult.No)
                {
                    return false;
                }

                HostingConfig = HostingConfig.Default;
            }


            TestApi.InitClientRouting(HostingConfig);

            return true;
        }
    }
}
using System.Windows.Forms;

using InfinniPlatform.Api.Hosting;
using InfinniPlatform.Api.TestEnvironment;

namespace InfinniPlatform.MetadataDesigner.Views.Update
{
    public sealed class ExchangeRemoteHost : IUpdatePrepareConfig
    {
        public ExchangeRemoteHost(HostingConfig hostingConfig, string version)
        {
            HostingConfig = hostingConfig;
            Version = version;
        }


        public HostingConfig HostingConfig { get; private set; }

        public string Version { get; private set; }


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

            var versionName = Version;

            if (string.IsNullOrEmpty(versionName))
            {
                if (MessageBox.Show(
                    @"Не указана версия развертывания.\n\rУстановить версию по умолчанию?\n\r (В этом случае обновлена будет версия с идентификатором TestVersion! )",
                    @"Требуется подтверждение", MessageBoxButtons.YesNo) == DialogResult.No)
                {
                    return false;
                }

                Version = "TestVersion";
            }

            TestApi.InitClientRouting(HostingConfig);

            return true;
        }
    }
}
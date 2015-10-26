using System;
using System.Collections.Generic;
using System.Globalization;
using System.Threading;
using System.Windows.Forms;

using InfinniPlatform.Api.RestApi.Auth;
using InfinniPlatform.Sdk.Api;
using InfinniPlatform.Sdk.Dynamic;
using InfinniPlatform.UserInterface.AppHost;
using InfinniPlatform.UserInterface.Services.Metadata;

namespace InfinniPlatform.UserInterface
{
    internal class Program
    {
        [STAThread]
        private static void Main()
        {
	        Thread.CurrentThread.CurrentCulture = new CultureInfo("en-US");
            Thread.CurrentThread.CurrentUICulture = new CultureInfo("en-US");

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            var appViewMetadata = StaticMetadata.CreateAppView(HostingConfig.Default.ServerName, HostingConfig.Default.ServerPort,"1");

            //TODO здесь необходимо настроить, куда API метаданных будет в итоге делать запросы
            AppRunner.Server = HostingConfig.Default.ServerName;
            AppRunner.Port = HostingConfig.Default.ServerPort;
            AppRunner.Run(appViewMetadata);
        }
    }
}
using System;
using System.Globalization;
using System.IO;
using System.Threading;
using System.Windows.Forms;

using InfinniPlatform.Api.RestApi.DataApi;
using InfinniPlatform.Sdk.Environment.Settings;
using InfinniPlatform.UserInterface.AppHost;

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

            var appViewMetadata = StaticMetadata.CreateAppView();

            //TODO здесь необходимо настроить, куда API метаданных будет в итоге делать запросы

	        AppRunner.Run(appViewMetadata);
        }
    }
}
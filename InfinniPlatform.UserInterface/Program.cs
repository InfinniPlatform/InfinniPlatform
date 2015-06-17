using System;
using System.Globalization;
using System.Threading;

using InfinniPlatform.Api.Hosting;
using InfinniPlatform.Api.RestApi.Auth;
using InfinniPlatform.Api.Settings;
using InfinniPlatform.UserInterface.AppHost;

namespace InfinniPlatform.UserInterface
{
	class Program
	{
		[STAThread]
		static void Main()
		{
#if DEBUG
			if (System.Diagnostics.Debugger.IsAttached)
			{
				var hostProcesses = System.Diagnostics.Process.GetProcessesByName("InfinniPlatform.RestfulApi");

				if (hostProcesses.Length == 0)
				{
					Api.TestEnvironment.TestApi.StartServer(p => p.SetHostingConfig(HostingConfig.Default));
				}
			}
#endif
			try
			{
				new SignInApi(null).SignInInternal(AuthorizationStorageExtensions.AdminUser,
				                               AppSettings.GetValue("AdminPassword", "Admin"), true);
			}
			catch 
			{
				//не удалось авторизоваться
			}

			Thread.CurrentThread.CurrentCulture = new CultureInfo("en-US");
			Thread.CurrentThread.CurrentUICulture = new CultureInfo("en-US");

			System.Windows.Forms.Application.EnableVisualStyles();
			System.Windows.Forms.Application.SetCompatibleTextRenderingDefault(false);

			var appViewMetadata = StaticMetadata.CreateAppView();
			AppRunner.Run(appViewMetadata);
		}
	}
}
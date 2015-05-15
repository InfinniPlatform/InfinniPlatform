using System;

using InfinniPlatform.Logging;

namespace InfinniPlatform.WebApi.Logging
{
	public static class LogExtensions
	{
		public static void SetupLogErrorHandler(this AppDomain target)
		{
			target.UnhandledException += (sender, args) =>
											 {
												 var message = (sender != null) ? sender.ToString() : string.Empty;
												 var exception = args.ExceptionObject as Exception;
												 Logger.Log.Fatal(message, exception);
											 };
		}
	}
}
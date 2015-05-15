using System;
using System.Web.Http;
using InfinniPlatform.Logging;

namespace InfinniPlatform.Hosting.WebApi.Implementation.Logging
{
	public static class LogExtensions
	{
		public static void SetupLogErrorHandler(this HttpConfiguration target)
		{
			target.Filters.Add(new LogExceptionFilterAttribute(Logger.Log));
		}

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
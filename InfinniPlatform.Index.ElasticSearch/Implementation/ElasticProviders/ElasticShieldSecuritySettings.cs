using InfinniPlatform.Sdk.Environment.Settings;

namespace InfinniPlatform.Index.ElasticSearch.Implementation.ElasticProviders
{
	internal static class ElasticShieldSecuritySettings
	{
		public static bool IsSet()
		{
			return Login != null && Password != null;
		}

		public static string Login = AppSettings.GetValue("ElasticSearchLogin");
		public static string Password = AppSettings.GetValue("ElasticSearchPassword");
	}
}
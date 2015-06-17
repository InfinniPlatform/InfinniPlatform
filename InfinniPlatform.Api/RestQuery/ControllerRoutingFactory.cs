﻿using InfinniPlatform.Api.Hosting;

namespace InfinniPlatform.Api.RestQuery
{
	/// <summary>
	/// Фабрика роутинга контроллеров REST.
	/// </summary>
	public sealed class ControllerRoutingFactory
	{
		private const string AppServerAddressFormat = "{0}://{1}:{2}/{3}";


		public ControllerRoutingFactory()
			: this(null)
		{
		}

		public ControllerRoutingFactory(HostingConfig hostingConfig)
		{
			_hostingConfig = hostingConfig ?? HostingConfig.Default;
		}


		private readonly HostingConfig _hostingConfig;


		private static ControllerRoutingFactory _instance;
		private static readonly object SyncObject = new object();

		/// <summary>
		/// Свойство добавлено для возможности явной инициализации в тестах.
		/// </summary>
		public static ControllerRoutingFactory Instance
		{
			get
			{
				if (_instance == null)
				{
					lock (SyncObject)
					{
						if (_instance == null)
						{
							_instance = new ControllerRoutingFactory();
						}
					}
				}

				return _instance;
			}
			set
			{
				_instance = value;
			}
		}


		public string BuildRestRoutingUrlStandardApi(string version, string configuration, string metadata, string action)
		{
			return BuildRestRoutingUrl(version, configuration, "StandardApi", metadata, action);
		}

		public string BuildRestRoutingUrlUpload(string version, string configuration, string metadata, string action)
		{
			return BuildRestRoutingUrl(version, configuration, "Upload", metadata, action);
		}

		private string BuildRestRoutingUrl(string version, string configuration, string controller, string metadata, string action)
		{
			return GetCustomRouting(GetRestTemplatePath()
                                        .ReplaceFormat("version",version)
										.ReplaceFormat("configuration", configuration)
										.ReplaceFormat("controller", controller)
										.ReplaceFormat("metadata", metadata)
										.ReplaceFormat("service", action));
		}

		public string BuildRestRoutingUrlUrlEncodedData(string version, string configuration, string metadata, string action)
		{
			return BuildRestRoutingUrl(version, configuration, "UrlEncodedData", metadata, action);
		}


		public string GetRestTemplatePath()
		{
			return "{version}/{configuration}/{controller}/{metadata}/{service}";
		}

		public string GetCustomRouting(string relativePath)
		{
			return string.Format(AppServerAddressFormat,
								 _hostingConfig.ServerScheme,
								 _hostingConfig.ServerName,
								 _hostingConfig.ServerPort,
								 relativePath);
		}

	}


	public static class RoutingExtension
	{
		public static string ReplaceFormat(this string processingString, string oldString, string newString)
		{
			return processingString.Replace(string.Format("{0}", "{" + oldString + "}"), newString);
		}
	}
}
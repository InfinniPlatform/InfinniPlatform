using System;
using System.IO;
using System.Threading;

namespace InfinniPlatform.NodeServiceHost
{
	public sealed class InfinniPlatformServiceHostDomain : IDisposable
	{
		public InfinniPlatformServiceHostDomain()
		{
			_domain = new Lazy<AppDomain>(CreateWorkerServiceDomain);
		}


		private readonly Lazy<AppDomain> _domain;


		private volatile bool _started;
		private readonly object _syncStarted = new object();


		public string GetStatus()
		{
			const string cGetStatusReturn = "GetStatus_return";

			_domain.Value.DoCallBack(() =>
									 {
										 var worker = GetWorkerServiceHost();
										 var status = worker.GetStatus();
										 SetDomainData(AppDomain.CurrentDomain, cGetStatusReturn, status);
									 });

			return GetDomainData<string>(_domain.Value, cGetStatusReturn);
		}

		public void Start()
		{
			if (!_started)
			{
				lock (_syncStarted)
				{
					if (!_started)
					{
						_domain.Value.DoCallBack(() =>
												 {
													 var worker = GetWorkerServiceHost();
													 worker.Start(Timeout.InfiniteTimeSpan);
												 });

						_started = true;
					}
				}
			}
		}

		public void Stop()
		{
			if (_started)
			{
				lock (_syncStarted)
				{
					if (_started)
					{
						_domain.Value.DoCallBack(() =>
												 {
													 var worker = GetWorkerServiceHost();
													 worker.Stop(Timeout.InfiniteTimeSpan);
												 });

						_started = false;
					}
				}
			}
		}

		public void Dispose()
		{
			if (_domain.IsValueCreated)
			{
				Stop();

				DeleteWorkerServiceDomain(_domain.Value);
			}
		}


		private static AppDomain CreateWorkerServiceDomain()
		{
			var currentDomainInfo = AppDomain.CurrentDomain.SetupInformation;
			var domainFriendlyName = typeof(InfinniPlatformServiceHostDomain).Name;

			// Создание домена приложения
			var domain = AppDomain.CreateDomain(domainFriendlyName, null, new AppDomainSetup
																		  {
																			  LoaderOptimization = LoaderOptimization.MultiDomainHost,
																			  ApplicationBase = currentDomainInfo.ApplicationBase,
																			  ConfigurationFile = currentDomainInfo.ConfigurationFile,
																			  ShadowCopyFiles = currentDomainInfo.ShadowCopyFiles
																		  });

			DomainAssemblyResolver.Setup(domain);

			// Установка рабочего каталога
			SetCurrentDirectory(domain, currentDomainInfo.ApplicationBase);

			// Установка обработчика службы
			SetWorkerServiceHost(domain);

			return domain;
		}

		private static void DeleteWorkerServiceDomain(AppDomain domain)
		{
			try
			{
				AppDomain.Unload(domain);
			}
			catch
			{
			}
		}


		private static void SetCurrentDirectory(AppDomain domain, string currentDirectory)
		{
			const string cCurrentDirectory = "CurrentDirectory";

			SetDomainData(domain, cCurrentDirectory, currentDirectory);

			domain.DoCallBack(() =>
							  {
								  var currentDirectoryValue = GetDomainData<string>(AppDomain.CurrentDomain, cCurrentDirectory);
								  Directory.SetCurrentDirectory(currentDirectoryValue);
							  });
		}


		private static void SetWorkerServiceHost(AppDomain domain)
		{
			domain.DoCallBack(() =>
							  {
								  var worker = new Lazy<InfinniPlatformServiceHost>(() => new InfinniPlatformServiceHost());
								  SetDomainData(AppDomain.CurrentDomain, "Instance", worker);
							  });
		}

		private static InfinniPlatformServiceHost GetWorkerServiceHost()
		{
			var domain = AppDomain.CurrentDomain;
			var worker = GetDomainData<Lazy<InfinniPlatformServiceHost>>(domain, "Instance");
			return worker.Value;
		}


		private static void SetDomainData(AppDomain domain, string name, object data)
		{
			var dataKey = GetDomainDataKey(name);
			domain.SetData(dataKey, data);
		}

		private static T GetDomainData<T>(AppDomain domain, string name)
		{
			var dataKey = GetDomainDataKey(name);
			return (T)domain.GetData(dataKey);
		}


		private static string GetDomainDataKey(string name)
		{
			return string.Format("InfinniPlatformServiceHostDomain.{0}", name);
		}


		internal sealed class DomainAssemblyResolver
		{
			static DomainAssemblyResolver()
			{
				AppDomain.CurrentDomain.AssemblyResolve += (s, e) => typeof(DomainAssemblyResolver).Assembly;
			}

			public static void Setup(AppDomain domain)
			{
				domain.CreateInstanceFrom(typeof(DomainAssemblyResolver).Assembly.Location, typeof(DomainAssemblyResolver).FullName);
			}
		}
	}
}
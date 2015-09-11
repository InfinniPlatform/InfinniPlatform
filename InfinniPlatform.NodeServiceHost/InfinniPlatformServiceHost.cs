﻿using System;
using System.ComponentModel.Composition;
using System.Threading;

using InfinniPlatform.Api.Settings;
using InfinniPlatform.BlobStorage;
using InfinniPlatform.Cassandra.Client;
using InfinniPlatform.EventStorage;
using InfinniPlatform.Hosting;
using InfinniPlatform.Modules;
using InfinniPlatform.NodeServiceHost.Properties;
using InfinniPlatform.SystemConfig.Initializers;

namespace InfinniPlatform.NodeServiceHost
{
	[Export("Infinni.NodeWorker.ServiceHost.IWorkerServiceHost")]
	public sealed class InfinniPlatformServiceHost
	{
		private volatile Status _status = Status.Stopped;
		private readonly object _statusSync = new object();
		private readonly Lazy<IHostingService> _hostingService = new Lazy<IHostingService>(CreateHostingService, LazyThreadSafetyMode.ExecutionAndPublication);


		public string GetStatus()
		{
			return _status.ToString();
		}

		public void Start(TimeSpan timeout)
		{
			if (_status != Status.Running && _status != Status.StartPending)
			{
				lock (_statusSync)
				{
					if (_status != Status.Running && _status != Status.StartPending)
					{
						var prevStatus = _status;

						_status = Status.StartPending;

						try
						{
							_hostingService.Value.Start();
						}
						catch
						{
							_status = prevStatus;

							throw;
						}

						_status = Status.Running;
					}
				}
			}
		}

		public void Stop(TimeSpan timeout)
		{
			if (_status != Status.Stopped && _status != Status.StopPending)
			{
				lock (_statusSync)
				{
					if (_status != Status.Stopped && _status != Status.StopPending)
					{
						var prevStatus = _status;

						_status = Status.StopPending;

						try
						{
							_hostingService.Value.Stop();
						}
						catch
						{
							_status = prevStatus;

							throw;
						}

						_status = Status.Stopped;
					}
				}
			}
		}


		private static IHostingService CreateHostingService()
		{
			var configurations = AppSettings.GetValue("ConfigurationList");

			if (string.IsNullOrWhiteSpace(configurations))
			{
				throw new ArgumentException(Resources.ConfigurationListIsEmpty);
			}

			CreateBlobStorage();
			CreateEventStorage();

			var assemblies = ModuleExtension.LoadModulesAssemblies(configurations);

			var factory = new OwinHostingServiceFactory(assemblies);
			var server = factory.CreateHostingService();

			// Заполняем глобальный контекст исполнения скриптов
			factory.InfinniPlatformHostServer.RegisterServerInitializer<GlobalContextInitializer>();
			// Устанавливаем системные конфигурации
			factory.InfinniPlatformHostServer.RegisterServerInitializer<SystemConfigurationsInitializer>();
			// Устанавливаем конфигурации из JSON-описаний
			factory.InfinniPlatformHostServer.RegisterServerInitializer<JsonConfigurationsInitializer>();
			// Пользовательские обработчики бизнес-логики старта сервера
			factory.InfinniPlatformHostServer.RegisterServerInitializer<UserLogicInitializer>();
			// Обработчик настройки хранилища пользователей
			factory.InfinniPlatformHostServer.RegisterServerInitializer<UserStorageInitializer>();

			return server;
		}

		private static void CreateBlobStorage()
		{
			var cassandraFactory = new CassandraDatabaseFactory();
			var blobStorageFactory = new CassandraBlobStorageFactory(cassandraFactory);
			var blobStorageManager = blobStorageFactory.CreateBlobStorageManager();

			try
			{
				blobStorageManager.CreateStorage();
			}
			catch
			{
			}
		}

		private static void CreateEventStorage()
		{
			var cassandraFactory = new CassandraDatabaseFactory();
			var eventStorageFactory = new CassandraEventStorageFactory(cassandraFactory);
			var eventStorageManager = eventStorageFactory.CreateEventStorageManager();

			try
			{
				eventStorageManager.CreateStorage();
			}
			catch
			{
			}
		}


		public enum Status
		{
			Stopped,
			StartPending,
			Running,
			StopPending
		}
	}
}
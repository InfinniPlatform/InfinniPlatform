﻿using System;

using Autofac;
using Autofac.Configuration;
using InfinniPlatform.Api.Context;
using InfinniPlatform.Api.Hosting;
using InfinniPlatform.Api.LocalRouting;
using InfinniPlatform.Api.RestApi.CommonApi;
using InfinniPlatform.Api.RestQuery;

namespace InfinniPlatform.Api.TestEnvironment
{
	/// <summary>
	/// Вспомогательный класс для отладки пользовательских конфигураций.
	/// </summary>
	public static class TestApi
	{
		public static IDisposable StartServer(Action<TestServerParametersBuilder> parameters)
		{
			var server = new TestServer();
			server.Start(parameters);
			return server;
		}

		public static IGlobalContext CreateContext()
		{
			var containerBuilder = new ContainerBuilder();
			containerBuilder.RegisterModule(new XmlFileReader("Autofac.xml"));
			var container = containerBuilder.Build();
			return container.Resolve<IGlobalContext>();
		}

		public static void InitClientRouting(HostingConfig hostingConfig)
		{
			ControllerRoutingFactory.Instance = new ControllerRoutingFactory(hostingConfig);
		}

		/// <summary>
		/// Вставить тестовые данные.
		/// </summary>
		/// <param name="configId">Идентификатор конфигурации.</param>
		/// <param name="documentId">Идентификатор документа.</param>
		/// <param name="timesCount">Количество вставок необходимых для создания базы документов данного типа.</param>
		/// <param name="prefillItems">Параметры автоматической генерации документов.</param>
		public static dynamic SetTestDocument(string configId, string documentId, int timesCount, dynamic prefillItems)
		{
			return RestQueryApi.QueryPostJsonRaw("RestfulApi", "configuration", "settestdocument", null,
												 new
												 {
													 Configuration = configId,
													 Metadata = documentId,
													 PrefillItems = prefillItems,
													 TimesCount = timesCount
												 }).ToDynamic();
		}
	}
}
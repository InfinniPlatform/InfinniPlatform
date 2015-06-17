using System;
using System.Threading;

using InfinniPlatform.Api.Dynamic;
using InfinniPlatform.Api.Metadata;
using InfinniPlatform.Api.Metadata.ConfigurationManagers.Standard.Factories;

namespace InfinniPlatform.UserInterface.Services.Metadata
{
	/// <summary>
	/// Сервис для работы с метаданными отчетов.
	/// </summary>
	sealed class ReportMetadataService : BaseMetadataService
	{
		public ReportMetadataService(string version, string configId)
		{
			_factory = new Lazy<ManagerFactoryConfiguration>(() => new ManagerFactoryConfiguration(version, configId), LazyThreadSafetyMode.ExecutionAndPublication);
		}


		private readonly Lazy<ManagerFactoryConfiguration> _factory;


		protected override IDataReader CreateDataReader()
		{
			return _factory.Value.BuildReportMetadataReader();
		}

		protected override IDataManager CreateDataManager()
		{
			return _factory.Value.BuildReportManager();
		}


		public override object CreateItem()
		{
			dynamic item = base.CreateItem();

			item.Content = new DynamicWrapper();

			item.Content.Title = new DynamicWrapper();
			item.Content.Title.Name = "ReportTitle1";
			item.Content.Title.Height = 10;

			item.Content.Page = new DynamicWrapper();
			item.Content.Page.Footer = new DynamicWrapper();
			item.Content.Page.Footer.Name = "PageFooter1";
			item.Content.Page.Footer.Height = 10;

			return item;
		}
	}
}
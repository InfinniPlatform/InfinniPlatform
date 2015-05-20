using System.Collections.Generic;
using InfinniPlatform.Api.Deprecated;
using InfinniPlatform.Api.Dynamic;
using InfinniPlatform.Api.Metadata;
using InfinniPlatform.Api.Metadata.ConfigurationManagers.Standard.Factories;

namespace InfinniPlatform.Api.Tests.Builders
{
	public static class ConfigurationBuilder
	{
		private const string ConfigurationCaption = "Тестовая конфигурация";


		public const string ConfigurationName = "TestConfiguration";

		public const string OrganizationGuid = "3091FE4D-D715-46FD-8BBD-E9D57B1AD558";

		
		public static IEnumerable<dynamic> CreateReferencedMenuObjects()
		{
			return new[]
				       {
					       BuildMainMenu()
				       };
		} 

		private static dynamic BuildMainMenuShortMetadata()
		{
			var metadataObject = new DynamicWrapper();
			var metadata = metadataObject
				.BuildProperty("Id",MenuGuid)
				.BuildProperty("Name", "NSI")
				.BuildProperty("Caption", "Справочники НСИ")
				.BuildProperty("Description", "Главное меню конфигурации справочников НСИ");
			return metadata;
		}

		private const string MenuGuid = "0B3717D3-5C03-49E0-9FB2-1FDE93880D4E";

		private static dynamic BuildMainMenu()
		{
			
			var metadataObject = new DynamicWrapper();
			var metadata = metadataObject
					.BuildProperty("Id", MenuGuid)
					.BuildProperty("Name", "NSI")
					.BuildProperty("Caption", "Справочники НСИ")
					.BuildProperty("Description", "Главное меню конфигурации справочников НСИ")
					.BuildMenuItem("Основное", mi => mi.BuildMenuItem("Просмотр справочников НСИ", min => min.BuildOpenViewAction(ov => ov.BuildSelectListView(
						slv => slv
								.BuildText("Выберите классификатор")
								.BuildDataSourcesList(dsl => dsl.BuildDataSource(ds => ds.BuildMetadataDataSource("SelectDataSource", "ClassifierStorage","Classifier")))
								.BuildStackLayoutPanel("StackPanel1", sl => sl
									.BuildDataElement(de => de.BuildSearchPanel("SearchPanel1", sp => sp.BuildDataBinding(db => db.BuildPropertyBinding("SelectDataSource"))))
									.BuildDataElement(de => de.BuildDataGrid("DataGrid1", dg => dg
										.BuildDataGridColumn(dgc => dgc.BuildDataGridColumnProperties("ColumnId", "Идентификатор элемента", "Id"))
										.BuildDataGridColumn(dgc => dgc.BuildDataGridColumnProperties("ColumnCode", "Код элемента", "Code"))
										.BuildDataGridColumn(dgc => dgc.BuildDataGridColumnProperties("ColumnName", "Наименование элемента", "Name"))
										.BuildDataBinding(db => db.BuildPropertyBinding("SelectDataSource"))
										)))))));
			return metadata;
		}
	}
}

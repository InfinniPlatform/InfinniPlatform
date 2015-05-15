using System;
using System.Collections.Generic;
using System.Linq;
using InfinniPlatform.Api.Context;
using InfinniPlatform.Api.ContextTypes;
using InfinniPlatform.Api.Dynamic;
using InfinniPlatform.Api.Metadata;
using InfinniPlatform.Api.RestQuery;
using InfinniPlatform.Index;

namespace InfinniConfiguration.SystemConfig.Tests.Builders
{
	public sealed class SampleConfigurationBuilder
	{
		private static readonly string _organizationGuid = new Guid("3091FE4D-D715-46FD-8BBD-E9D57B1AD558").ToString();

		public static string OrganizationGuid
		{
			get { return _organizationGuid; }
		}


		public SampleConfigurationBuilder BuildDocumentPatient(object config)
		{
			var patientGuid = "3091FE4D-D715-46FD-8BBD-E9D57B1AD559";
			config.BuildId(patientGuid).BuildName("Patient");
			return this;
		}

		public SampleConfigurationBuilder BuildDocumentOrganization(object config)
		{
			//получаем список типов зарегистрированных сервисов платформы
			var builder = new RestQueryBuilder("SystemConfig", "metadata", "getservicemetadata");
			var response = builder.QueryGet(null, 0, 1000);

			var publishFilterWorkflowGuid = "9164AB65-7703-4F6F-862C-6F50CE8FFED2";
			var publishMoveWorkflowGuid = "2C17FE23-C467-4B8B-A146-F6AB853276E9";
			var publishGetResultWorkflowGuid = "95CCB4A9-9949-4F91-9274-B1A816787B97";
			var searchSearchModelWorkflowGuid = "478CF020-63E8-4907-BCED-D0C04E3F8FA8";
			var searchValidateFilterWorkflowGuid = "3091FE4D-D715-46FD-8BBD-E9D57B1AD558";

			IEnumerable<dynamic> services = response.ToDynamicList().ToList();

			IEnumerable<dynamic> applyServiceExtensionPoints = DynamicInstanceExtensions.ToEnumerable(services.FirstOrDefault(s => s.Name.ToLowerInvariant() == "applyevents").WorkflowExtensionPoints);
			IEnumerable<dynamic> searchServiceExtensionPoints = DynamicInstanceExtensions.ToEnumerable(services.FirstOrDefault(s => s.Name.ToLowerInvariant() == "search").WorkflowExtensionPoints);

			//получаем список стандартных точек расширения потока ("action","onsuccess" и т.д)
			IEnumerable<string> standardExtensionPoints = new RestQueryBuilder("SystemConfig", "metadata", "getstandardextensionpoints").QueryGet(null, 0, 1000).ToDynamicList().Select(r => (string)r).ToList();

			config
				.BuildId(OrganizationGuid)
				.BuildName("Organization")
				.BuildCaption("Организация")
				.BuildProperty("Versioning", (int)VersionProviderType.HistoryStrategyTwoIndexHistory)
			#region Сценарии
					.BuildScenario(sc => sc
						.BuildId("SaveOrganization")
						.BuildName("ActionUnitSaveOrganization")
						.BuildCaption("Сохранение организации")
						.BuildDescription("Модуль сохранения данных организации")
						.BuildProperty("ContextType", (int)ContextTypeKind.ApplyMove)
						.BuildProperty("ScriptUnitType", (int)ScriptUnitType.Action)
						)
					.BuildScenario(sc => sc
						.BuildId("SearchOrganization")
						.BuildName("ActionUnitOrganizationSearchModel")
						.BuildCaption("Поиск организации")
						.BuildDescription("Модуль поиска организации")
						.BuildProperty("ContextType", (int)ContextTypeKind.SearchContext)
						.BuildProperty("ScriptUnitType", (int)ScriptUnitType.Action)
						)
					.BuildScenario(sc => sc
						.BuildId("OrganizationFilterEvents")
						.BuildName("ActionUnitOrganizationFilterEvents")
						.BuildCaption("Предобработка данных организации")
						.BuildDescription("Модуль предобработки данных организации")
						.BuildProperty("ContextType", (int)ContextTypeKind.ApplyFilter)
						.BuildProperty("ScriptUnitType", (int)ScriptUnitType.Action)
						)
					.BuildScenario(sc => sc
						.BuildId("PublishResultOrganization")
						.BuildName("ActionUnitPublishResultOrganization")
						.BuildCaption("Постобработка результата публикации данных организации")
						.BuildDescription("Модуль подготовки результата публикации данных организации")
						.BuildProperty("ContextType", (int)ContextTypeKind.ApplyFilter)
						.BuildProperty("ScriptUnitType", (int)ScriptUnitType.Action)
						)
					.BuildScenario(sc => sc
						.BuildId("OrganizationSearchResult")
						.BuildName("ActionUnitOrganizationSearchModel")
						.BuildCaption("Получение данных об организациях по фильтру")
						.BuildDescription("Модуль получения данных об организациях по фильтру")
						.BuildProperty("ContextType", (int)ContextTypeKind.SearchContext)
						.BuildProperty("ScriptUnitType", (int)ScriptUnitType.Action)
						)
					.BuildScenario(sc => sc
						.BuildId("OrganizationSearch")
						.BuildName("ValidationUnitOrganizationSearch")
						.BuildCaption("Проверка корректности данных для поиска по организации")
						.BuildDescription("Модуль проверки корректности данных об организациях")
						.BuildProperty("ContextType", (int)ContextTypeKind.SearchContext)
						.BuildProperty("ScriptUnitType", (int)ScriptUnitType.Action)
						)
					.BuildScenario(sc => sc
						.BuildId("organization")
						.BuildName("ValidationUnitOrganizationSave")
						.BuildCaption("Проверка корректности данных при сохранении организации")
						.BuildDescription("Модуль проверки корректности")
						.BuildProperty("ContextType", (int)ContextTypeKind.ApplyMove)
						.BuildProperty("ScriptUnitType", (int)ScriptUnitType.Validator)
						)
					.BuildScenario(sc => sc
						.BuildId("saveversion")
						.BuildName("ActionUnitSaveVersion")
						.BuildCaption("Сохранение данных ")
						.BuildDescription("Модуль сохранения данных ")
						.BuildProperty("ContextType", (int)ContextTypeKind.ApplyMove)
						.BuildProperty("ScriptUnitType", (int)ScriptUnitType.Action)
						)
			#endregion
			#region Сервисы
					.BuildService(sv => sv
						.BuildId(Guid.NewGuid().ToString())
						.BuildName("Search")
						.BuildProperty("Type", (object)services.FirstOrDefault(s => s.Name.ToLowerInvariant() == "search"))
						.BuildCollectionItem("ExtensionPoints", pt => pt
																		  .BuildProperty("TypeName", (object)searchServiceExtensionPoints.FirstOrDefault(s => s.Name.ToLowerInvariant() == "validatefilter"))
																		  .BuildProperty("ScenarioId", "ValidateFilter"))
						.BuildCollectionItem("ExtensionPoints", pt => pt
																		  .BuildProperty("TypeName", (object)searchServiceExtensionPoints.FirstOrDefault(s => s.Name.ToLowerInvariant() == "searchmodel"))
																		  .BuildProperty("ScenarioId", "SearchModel")))
					.BuildService(sv => sv
						.BuildId(Guid.NewGuid().ToString())
						.BuildName("Publish")
						.BuildProperty("Type", (object)services.FirstOrDefault(s => s.Name.ToLowerInvariant() == "applyevents"))
						.BuildCollectionItem("ExtensionPoints", pt => pt
																		  .BuildProperty("TypeName", (object)applyServiceExtensionPoints.FirstOrDefault(s => s.Name.ToLowerInvariant() == "filterevents"))
																		  .BuildProperty("ScenarioId", "PublishFilterEvents"))
						.BuildCollectionItem("ExtensionPoints", pt => pt
																		  .BuildProperty("TypeName", (object)applyServiceExtensionPoints.FirstOrDefault(s => s.Name.ToLowerInvariant() == "move"))
																		  .BuildProperty("ScenarioId", "Publish"))
						.BuildCollectionItem("ExtensionPoints", pt => pt
																		  .BuildProperty("TypeName", (object)applyServiceExtensionPoints.FirstOrDefault(s => s.Name.ToLowerInvariant() == "getresult"))
																		  .BuildProperty("ScenarioId", "PublishResult"))

						)
			#endregion
			#region Процессы
					.BuildProcess(bp => bp
						.BuildId(searchSearchModelWorkflowGuid)
						.BuildName("searchmodel")
						.BuildCaption("Получение данных об организациях")
						.BuildProperty("Type", (int)WorkflowTypes.WithoutState)
						.BuildCollectionItem("Transitions", tr => tr
							.BuildId(searchSearchModelWorkflowGuid)
							.BuildName("SearchModelTransition")
							.BuildCaption("Получение данных")
							.BuildContainer("ActionPoint", prop => prop
								.BuildProperty("TypeName", standardExtensionPoints.FirstOrDefault(s => s.ToLowerInvariant() == "action"))
								.BuildProperty("ScenarioId", "OrganizationSearchResult"))
							))
					.BuildProcess(bp => bp
						.BuildId(searchValidateFilterWorkflowGuid)
						.BuildName("validatefilter")
						.BuildCaption("Проверка корректности переданного фильтра данных")
						.BuildProperty("Type", (int)WorkflowTypes.WithoutState)
						.BuildCollectionItem("Transitions", tr => tr
							.BuildId("444AF5AA-09ED-4D70-825C-540FAC1949D6")
							.BuildName("ValidateFilterTransition")
							.BuildCaption("Валидация фильтра данных")
							.BuildContainer("ValidationPoint", prop => prop
								.BuildProperty("TypeName", standardExtensionPoints.FirstOrDefault(s => s.ToLowerInvariant() == "validation"))
								.BuildProperty("ScenarioId", "OrganizationSearch"))
							))
					.BuildProcess(bp => bp
						.BuildId(publishFilterWorkflowGuid)
						.BuildName("publishFilterEvents")
						.BuildCaption("Фильтрация входных данных")
						.BuildProperty("Type", (int)WorkflowTypes.WithoutState)
						.BuildCollectionItem("Transitions", tr => tr
							.BuildId("59F0A4FA-239E-4960-B3F1-8640B59863BC")
							.BuildName("DocumentEventsFilter")
							.BuildCaption("Фильтр событий")
							.BuildContainer("ActionPoint", prop => prop
								.BuildProperty("TypeName", standardExtensionPoints.FirstOrDefault(s => s.ToLowerInvariant() == "action"))
								.BuildProperty("ScenarioId", "organizationfilterevents"))
							))
					.BuildProcess(bp => bp
						.BuildId(publishMoveWorkflowGuid)
						.BuildName("publish")
						.BuildCaption("Опубликовать")
						.BuildProperty("Type", (int)WorkflowTypes.WithState)
						.BuildCollectionItem("Transitions", tr => tr
							.BuildId("4A5CF4DF-6928-4739-843A-DA34BA12831B")
							.BuildName("Temporary-Saved")
							.BuildProperty("StateFrom", "Temporary")
							.BuildProperty("StateTo", "Saved")
							.BuildCaption("Временный-сохранен")
							.BuildDescription("Переход из несохраненного в сохраненное состояние")
							.BuildContainer("ValidationPoint", prop => prop
								.BuildProperty("TypeName", standardExtensionPoints.FirstOrDefault(s => s.ToLowerInvariant() == "validation"))
								.BuildProperty("ScenarioId", "organization"))
							.BuildContainer("FailPoint", prop => prop
								.BuildProperty("TypeName", standardExtensionPoints.FirstOrDefault(s => s.ToLowerInvariant() == "onfail"))
								.BuildProperty("ScenarioId", "saveversion")))
						.BuildCollectionItem("Transitions", tr => tr
							.BuildId("8F752913-2D70-4FBD-92C5-BFAD4506AA32")
							.BuildName("Saved-Published")
							.BuildProperty("StateFrom", "Saved")
							.BuildProperty("StateTo", "Published")
							.BuildCaption("Сохранен-Опубликован")
							.BuildDescription("Переход из сохраненного в опубликованное состояние")
							.BuildContainer("SuccessPoint", prop => prop
								.BuildProperty("TypeName", standardExtensionPoints.FirstOrDefault(s => s.ToLowerInvariant() == "onsuccess"))
								.BuildProperty("ScenarioId", "saveversion"))
							.BuildContainer("ActionPoint", prop => prop
								.BuildProperty("TypeName", standardExtensionPoints.FirstOrDefault(s => s.ToLowerInvariant() == "action"))
								.BuildProperty("ScenarioId", "saveorganization"))
							)
						)
						.BuildProcess(bp => bp
							.BuildId(publishGetResultWorkflowGuid)
							.BuildName("publishResult")
							.BuildCaption("Публикация результатов проведения документа")
							.BuildDescription("Получить результирующий документ")
							.BuildProperty("Type", (int)WorkflowTypes.WithoutState)
							.BuildCollectionItem("Transitions", tr => tr
								.BuildId("41EC76CA-FD3F-4F64-82AA-175745B56A9F")
								.BuildName("DocumentGetResult")
								.BuildCaption("Результат публикаци")
								.BuildDescription("Возврат результата после публикации")
								.BuildContainer("ActionPoint", prop => prop
									.BuildProperty("TypeName", standardExtensionPoints.FirstOrDefault(s => s.ToLowerInvariant() == "action"))
									.BuildProperty("ScenarioId", "publishresultorganization"))
							)
						);
				#endregion

			return this;
		}

		public SampleConfigurationBuilder BuildClassifier(object config)
		{
			var classifierDocumentTypeId = "EHR-RUS-ClinicalDocumentType";
			config.BuildClassifier(bp => bp
					.BuildId(classifierDocumentTypeId)
					.BuildName("ClassifierDocumentType")
					.BuildCaption("Классификатор типов медицинских документов")
					.BuildProperty("CodeSystem", classifierDocumentTypeId)
					.BuildProperty("CodeSystemName", "Классификатор типов медицинских документов")
					.BuildProperty("CodeSystemVersion", "1.0")
					.BuildProperty("CodePropertyRef", "Id")
					.BuildProperty("DisplayNamePropertyRef", "Name")
					.BuildProperty("ImportSource", "Other")
					.BuildModel(model => model
						.BuildCaption("Основные реквизиты справочника")
						.BuildDescription("Значения основных реквизитов справочника")
						.BuildCollectionItem("Properties", prop => prop
							.BuildProperty("Type", "string")
							.BuildProperty("Name", "Id")
							.BuildProperty("Caption", "Идентификатор")
						)
						.BuildCollectionItem("Properties", prop => prop
							.BuildProperty("Type", "string")
							.BuildProperty("Name", "Name")
							.BuildProperty("Caption", "Наименование типа медицинского документа")
						)
						.BuildCollectionItem("Properties", prop => prop
							.BuildProperty("Type", "string")
							.BuildProperty("Name", "Description")
							.BuildProperty("Caption", "Описание типа медицинского документа")
						)
					));
			return this;
		}

		public SampleConfigurationBuilder BuildMenu(object config)
		{
			config.BuildMenu(m => m.BuildProperty("Name", "NSI")
					.BuildProperty("Caption", "Справочники НСИ")
					.BuildProperty("Description", "Главное меню конфигурации справочников НСИ")
					.BuildMenuItem("Основное", mi => mi.BuildMenuItem("Просмотр справочников НСИ", min => min.BuildOpenViewAction(ov => ov.BuildSelectListView(
						slv => slv
								.BuildText("Выберите классификатор")
								.BuildDataSourcesList(dsl => dsl.BuildDataSource(ds => ds.BuildMetadataDataSource("SelectDataSource", "Classifier")))
								.BuildStackLayoutPanel("StackPanel1", sl => sl
									.BuildDataElement(de => de.BuildSearchPanel("SearchPanel1", sp => sp.BuildDataBinding(db => db.BuildPropertyBinding("SelectDataSource"))))
									.BuildDataElement(de => de.BuildDataGrid("DataGrid1", dg => dg
										.BuildDataGridColumn(dgc => dgc.BuildDataGridColumnProperties("ColumnId", "Идентификатор элемента", "Id"))
										.BuildDataGridColumn(dgc => dgc.BuildDataGridColumnProperties("ColumnCode", "Код элемента", "Code"))
										.BuildDataGridColumn(dgc => dgc.BuildDataGridColumnProperties("ColumnName", "Наименование элемента", "Name"))
										.BuildDataBinding(db => db.BuildPropertyBinding("SelectDataSource"))
										))))))));
			return this;
		}


	}
}

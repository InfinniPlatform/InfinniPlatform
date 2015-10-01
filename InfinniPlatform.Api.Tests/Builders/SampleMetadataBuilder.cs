using InfinniPlatform.Api.Deprecated;
using InfinniPlatform.Sdk.Dynamic;

namespace InfinniPlatform.Api.Tests.Builders
{
    public static class SampleMetadataBuilder
    {
        public static object BuildSampleConfiguration(string configName, string configCaption, string configDescription)
        {
            dynamic config = MetadataBuilderExtensions.BuildConfiguration(configName, configCaption, configDescription,
                                                                          null);
            config.Id = "ConfigIdentifier";
            return config;
        }

        public static object BuildSampleAssembly(this object instance, string assemblyName, string assemblyId)
        {
            var metadataObject = new DynamicWrapper();
            var metadata = metadataObject
                .BuildProperty("Id", assemblyId)
                .BuildProperty("Name", assemblyName);

            return metadata;
        }

        public static object BuildSampleReport(this object instance, string reportName, string reportId)
        {
            return new DynamicWrapper()
                .BuildId(reportId)
                .BuildName(reportName)
                .BuildCaption("Тестовый отчет");
        }

        public static object BuildSampleRegister(this object instance, string registerName, string registerId)
        {
            var metadataObject = new DynamicWrapper();
            var metadata = metadataObject
                .BuildProperty("Id", registerId)
                .BuildProperty("Name", registerName)
                .BuildProperty("Caption", "Тестовый регистр")
                .BuildProperty("Description", "Описание тестового регистра")
                .BuildProperty("DocumentId", "RegisterDocument")
                .BuildProperty("Period", "Day")
                .BuildProperty("Asynchronous", false);
            return metadata;
        }

        public static object BuildSampleMenu(this object instance, string menuName, string menuId)
        {
            var metadataObject = new DynamicWrapper();
            var metadata = metadataObject
                .BuildProperty("Id", menuId)
                .BuildProperty("Name", menuName)
                .BuildProperty("Caption", "Тестовая группа")
                .BuildProperty("Description", "Описание тестовой группы")
                .BuildMenuItem("Основное", mi => mi
                                                     .BuildMenuItem("Вложенный menuitem",
                                                                    min =>
                                                                    min.BuildOpenViewAction(
                                                                        ov => ov.BuildSelectListView(
                                                                            slv => slv
                                                                                       .BuildText("Выберите что-то")
                                                                                       .BuildDataSourcesList(
                                                                                           dsl =>
                                                                                           dsl.BuildDataSource(
                                                                                               ds =>
                                                                                               ds
                                                                                                   .BuildMetadataDataSource
                                                                                                   ("SelectDataSource",
                                                                                                    "ClassifierStorage",
                                                                                                    "Classifier")))
                                                                                       .BuildStackLayoutPanel(
                                                                                           "StackPanel1", sl => sl
                                                                                                                    .BuildDataElement
                                                                                                                    (de
                                                                                                                     =>
                                                                                                                     de
                                                                                                                         .BuildSearchPanel
                                                                                                                         ("SearchPanel1",
                                                                                                                          sp
                                                                                                                          =>
                                                                                                                          sp
                                                                                                                              .BuildDataBinding
                                                                                                                              (db
                                                                                                                               =>
                                                                                                                               db
                                                                                                                                   .BuildPropertyBinding
                                                                                                                                   ("SelectDataSource"))))
                                                                                                                    .BuildDataElement
                                                                                                                    (de
                                                                                                                     =>
                                                                                                                     de
                                                                                                                         .BuildDataGrid
                                                                                                                         ("DataGrid1",
                                                                                                                          dg
                                                                                                                          =>
                                                                                                                          dg
                                                                                                                              .BuildDataGridColumn
                                                                                                                              (dgc
                                                                                                                               =>
                                                                                                                               dgc
                                                                                                                                   .BuildDataGridColumnProperties
                                                                                                                                   ("ColumnId",
                                                                                                                                    "Идентификатор элемента",
                                                                                                                                    "Id"))
                                                                                                                              .BuildDataGridColumn
                                                                                                                              (dgc
                                                                                                                               =>
                                                                                                                               dgc
                                                                                                                                   .BuildDataGridColumnProperties
                                                                                                                                   ("ColumnCode",
                                                                                                                                    "Код элемента",
                                                                                                                                    "Code"))
                                                                                                                              .BuildDataGridColumn
                                                                                                                              (dgc
                                                                                                                               =>
                                                                                                                               dgc
                                                                                                                                   .BuildDataGridColumnProperties
                                                                                                                                   ("ColumnName",
                                                                                                                                    "Наименование элемента",
                                                                                                                                    "Name"))
                                                                                                                              .BuildDataBinding
                                                                                                                              (db
                                                                                                                               =>
                                                                                                                               db
                                                                                                                                   .BuildPropertyBinding
                                                                                                                                   ("SelectDataSource"))
                                                                                                                         )))
                                                                                  ))));
            return metadata;
        }

        public static object BuildSampleView(string viewName, string viewId)
        {
            dynamic classifierMetadata = new DynamicWrapper();
            classifierMetadata.CodeSystemName = "Аллергены";
            classifierMetadata.Id = "1.2.643.5.1.13.2.7.1.55_4";

            var metadata = new DynamicWrapper()
                .BuildProperty("Id", viewId)
                .BuildText((string) classifierMetadata.CodeSystemName)
                .BuildName(viewName)
                .BuildDataSourcesList(
                    dsl =>
                    dsl.BuildDataSource(
                        ds =>
                        ds.BuildClassifierDataSource("ListDataSource", "ClassifierStorage",
                                                     (string) classifierMetadata.Id)))
                .BuildStackLayoutPanel("StackPanel1", sl => sl
                                                                .BuildDataElement(
                                                                    de =>
                                                                    de.BuildSearchPanel("SearchPanel1",
                                                                                        sp =>
                                                                                        sp.BuildDataBinding(
                                                                                            db =>
                                                                                            db.BuildPropertyBinding(
                                                                                                "ListDataSource"))))
                                                                .BuildDataElement(
                                                                    de => de.BuildDataGrid("DataGrid1", dg => dg
                                                                                                                  .BuildDataGridColumn
                                                                                                                  (dgc
                                                                                                                   =>
                                                                                                                   dgc
                                                                                                                       .BuildDataGridColumnProperties
                                                                                                                       ("ColumnId",
                                                                                                                        "Идентификатор элемента",
                                                                                                                        "Id"))
                                                                                                                  .BuildDataGridColumn
                                                                                                                  (dgc
                                                                                                                   =>
                                                                                                                   dgc
                                                                                                                       .BuildDataGridColumnProperties
                                                                                                                       ("ColumnCode",
                                                                                                                        "Код элемента",
                                                                                                                        "Code"))
                                                                                                                  .BuildDataGridColumn
                                                                                                                  (dgc
                                                                                                                   =>
                                                                                                                   dgc
                                                                                                                       .BuildDataGridColumnProperties
                                                                                                                       ("ColumnName",
                                                                                                                        "Наименование элемента",
                                                                                                                        "Name"))
                                                                                                                  .BuildDataBinding
                                                                                                                  (db =>
                                                                                                                   db
                                                                                                                       .BuildPropertyBinding
                                                                                                                       ("ListDataSource"))
                                                                              ))
                                                                .BuildDataElement(
                                                                    de => de.BuildToolBar("ToolBar1", dt => dt
                                                                                                                .BuildButton
                                                                                                                (bt =>
                                                                                                                 bt
                                                                                                                     .BuildButtonProperties
                                                                                                                     ("AddButton",
                                                                                                                      "Добавить",
                                                                                                                      bp
                                                                                                                      =>
                                                                                                                      bp
                                                                                                                          .BuildButtonActionAddDefault
                                                                                                                          ("ListDataSource",
                                                                                                                           ds
                                                                                                                           =>
                                                                                                                           ds
                                                                                                                               .BuildClassifierDataSource
                                                                                                                               ("EditDataSource",
                                                                                                                                "ClassifierStorage",
                                                                                                                                (
                                                                                                                                string
                                                                                                                                )
                                                                                                                                classifierMetadata
                                                                                                                                    .Id)))
                                                                                                                )
                                                                                                                .BuildButton
                                                                                                                (bt =>
                                                                                                                 bt
                                                                                                                     .BuildButtonProperties
                                                                                                                     ("ChangeButton",
                                                                                                                      "Изменить",
                                                                                                                      bp
                                                                                                                      =>
                                                                                                                      bp
                                                                                                                          .BuildButtonActionEditDefault
                                                                                                                          ("ListDataSource",
                                                                                                                           "ListDataSource",
                                                                                                                           ds
                                                                                                                           =>
                                                                                                                           ds
                                                                                                                               .BuildClassifierDataSource
                                                                                                                               ("EditDataSource",
                                                                                                                                "ClassifierStorage",
                                                                                                                                (
                                                                                                                                string
                                                                                                                                )
                                                                                                                                classifierMetadata
                                                                                                                                    .Id)))
                                                                                                                )
                                                                                                                .BuildButtonSeparator
                                                                                                                ()
                                                                                                                .BuildButton
                                                                                                                (bt =>
                                                                                                                 bt
                                                                                                                     .BuildButtonProperties
                                                                                                                     ("DeleteButton",
                                                                                                                      "Удалить",
                                                                                                                      bp
                                                                                                                      =>
                                                                                                                      bp
                                                                                                                          .BuildButtonActionDelete
                                                                                                                          ("ListDataSource")))
                                                                              )));
            return metadata;
        }

        public static object BuildEmptyDocument(string documentName, string documentId)
        {
            dynamic doc = MetadataBuilderExtensions.BuildDocument(documentName, documentName, documentName, documentName,
                                                                  null);
            doc.Id = documentId;
            return doc;
        }

        public static object BuildEmptyProcess(string processName, string processId)
        {
            return new DynamicWrapper()
                .BuildId(processId)
                .BuildName(processName)
                .BuildCaption("Тестовый процесс");
        }

        public static object BuildEmptyGenerator(string generatorName, string metadataId)
        {
            return new DynamicWrapper()
                .BuildId(metadataId)
                .BuildName(generatorName)
                .BuildCaption("Тестовый генератор");
        }

        public static object BuildEmptyScenario(string scenarioName, string metadataId)
        {
            return new DynamicWrapper()
                .BuildId(metadataId)
                .BuildName(scenarioName)
                .BuildCaption("Тестовый сценарий");
        }

        public static object BuildEmptyService(string serviceName, string metadataId)
        {
            return new DynamicWrapper()
                .BuildId(metadataId)
                .BuildName(serviceName)
                .BuildCaption("Тестовый сервис");
        }
    }
}
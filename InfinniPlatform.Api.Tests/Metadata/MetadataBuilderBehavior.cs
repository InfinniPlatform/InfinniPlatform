using System;
using System.Text;
using InfinniPlatform.Api.Deprecated;
using InfinniPlatform.Sdk.Dynamic;
using NUnit.Framework;

namespace InfinniPlatform.Api.Tests.Metadata
{
    [TestFixture]
    [Category(TestCategories.UnitTest)]
    public sealed class MetadataBuilderBehavior
    {
        /// <summary>
        ///     Получить строку обработчика выбора элемента формы журнала в формате base64
        /// </summary>
        /// <returns>Строка обработчика в формате base64</returns>
        private string GetJavaScriptOnSelectedHandler()
        {
            string converted =
                "	   context.OpenView({										" +
                "			View : {											" +
                "			DefaultListView : {									" +
                "				DataSource : {									" +
                "				ClassifierDataSource : {						" +
                "					Name : \"ListDataSource\",					" +
                "					ConfigId : \"System\",						" +
                "					ClassifierId : context.SelectedValue.Id		" +
                "				}												" +
                "				}												" +
                "			}													" +
                "			}													" +
                "		}														" +
                "	});															";

            byte[] toEncodeAsBytes = Encoding.UTF8.GetBytes(converted);
            return Convert.ToBase64String(toEncodeAsBytes);
        }

        [Test]
        public void ShouldBuildCollection()
        {
            var metadataObject = new DynamicWrapper();
            var metadata = metadataObject.BuildCollectionProperty("SomeCollection");

            Assert.AreEqual(metadata.ToString(), "{\r\n  \"SomeCollection\": []\r\n}".Replace("\r\n", Environment.NewLine));
        }

        [Test]
        public void ShouldBuildHomePage()
        {
            var metadataObject = new DynamicWrapper();
            var metadata = metadataObject
                .BuildText("Конфигурация: Справочники НСИ")
                .BuildTabPanel("Рanel1", pn => pn.BuildTabPage("Конфигурация: справочники НСИ", pa => pa
                                                                                                          .BuildDisableClose
                                                                                                          ()
                                                                                                          .BuildStackLayoutPanel
                                                                                                          ("StackPanel1",
                                                                                                           slp => slp
                                                                                                                      .BuildActionElement
                                                                                                                      (ae
                                                                                                                       =>
                                                                                                                       ae
                                                                                                                           .BuildMenuBar
                                                                                                                           ("Menu1",
                                                                                                                            "ClassifierStorage")))
                                                   ));
            Assert.AreEqual(metadata.ToString(),
                            "{\r\n  \"Text\": \"Конфигурация: Справочники НСИ\",\r\n  \"LayoutPanel\": {\r\n    \"TabPanel\": {\r\n      \"Name\": \"Рanel1\",\r\n      \"Pages\": [\r\n        {\r\n          \"Text\": \"Конфигурация: справочники НСИ\",\r\n          \"CanClose\": false,\r\n          \"LayoutPanel\": {\r\n            \"StackPanel\": {\r\n              \"Name\": \"StackPanel1\",\r\n              \"Items\": [\r\n                {\r\n                  \"MenuBar\": {\r\n                    \"Name\": \"Menu1\",\r\n                    \"ConfigId\": \"ClassifierStorage\"\r\n                  }\r\n                }\r\n              ]\r\n            }\r\n          }\r\n        }\r\n      ]\r\n    }\r\n  }\r\n}".Replace("\r\n", Environment.NewLine)
                );
        }

        [Test]
        public void ShouldBuildMetadataEditView()
        {
            string configId = "System";

            dynamic classifierMetadata = new DynamicWrapper();
            classifierMetadata.CodeSystemName = "Аллергены";
            classifierMetadata.Id = "1.2.643.5.1.13.2.7.1.55_4";

            var metadataObject = new DynamicWrapper();
            var metadata = metadataObject
                .BuildText(string.Format("Редактирование справочника '{0}'", (string)classifierMetadata.CodeSystemName))
                .BuildDataSourcesList(
                    dsl =>
                    dsl.BuildDataSource(
                        ds => ds.BuildClassifierDataSource("EditDataSource", configId, (string)classifierMetadata.Id)))
                .BuildStackLayoutPanel("StackPanel1",
                                       slp => slp.BuildDataElement(de => de.BuildPropertyGrid("PropertyGrid1",
                                                                                              pg =>
                                                                                              pg
                                                                                                  .BuildPropertyGridCategory
                                                                                                  ("Основные реквизиты справочника",
                                                                                                   pgc => pgc
                                                                                                              .BuildPropertyGridTextBox
                                                                                                              ("Идентификатор записи",
                                                                                                               "ID",
                                                                                                               ed =>
                                                                                                               ed
                                                                                                                   .BuildEditValueBinding
                                                                                                                   (evb
                                                                                                                    =>
                                                                                                                    evb
                                                                                                                        .BuildPropertyBinding
                                                                                                                        ("EditDataSource",
                                                                                                                         "ID")))
                                                                                                              .BuildPropertyGridTextBox
                                                                                                              ("Код элемента",
                                                                                                               "Code",
                                                                                                               ed =>
                                                                                                               ed
                                                                                                                   .BuildEditValueBinding
                                                                                                                   (evb
                                                                                                                    =>
                                                                                                                    evb
                                                                                                                        .BuildPropertyBinding
                                                                                                                        ("EditDataSource",
                                                                                                                         "Code")))
                                                                                                              .BuildPropertyGridTextBox
                                                                                                              ("Наименование элемента",
                                                                                                               "Name",
                                                                                                               ed =>
                                                                                                               ed
                                                                                                                   .BuildEditValueBinding
                                                                                                                   (evb
                                                                                                                    =>
                                                                                                                    evb
                                                                                                                        .BuildPropertyBinding
                                                                                                                        ("EditDataSource",
                                                                                                                         "Name")))
                                                                                                  ))))
                .BuildToolBar("Toolbar1", tb => tb
                                                    .BuildButton(
                                                        bt =>
                                                        bt.BuildButtonProperties("SaveButton", "Сохранить",
                                                                                 ab =>
                                                                                 ab.BuildButtonActionSave(
                                                                                     "EditDataSource")))
                                                    .BuildButton(
                                                        bt =>
                                                        bt.BuildButtonProperties("CancelButton", "Отменить",
                                                                                 cb =>
                                                                                 cb.BuildButtonActionCancel(
                                                                                     "EditDataSource")))
                );

            Assert.AreEqual(metadata.ToString(),
                            "{\r\n  \"Text\": \"Редактирование справочника 'Аллергены'\",\r\n  \"DataSources\": [\r\n    {\r\n      \"ClassifierDataSource\": {\r\n        \"Name\": \"EditDataSource\",\r\n        \"ConfigId\": \"System\",\r\n        \"ClassifierMetadataId\": \"1.2.643.5.1.13.2.7.1.55_4\"\r\n      }\r\n    }\r\n  ],\r\n  \"LayoutPanel\": {\r\n    \"StackPanel\": {\r\n      \"Name\": \"StackPanel1\",\r\n      \"Items\": [\r\n        {\r\n          \"PropertyGrid\": {\r\n            \"Name\": \"PropertyGrid1\",\r\n            \"Categories\": [\r\n              {\r\n                \"Text\": \"Основные реквизиты справочника\",\r\n                \"Properties\": [\r\n                  {\r\n                    \"Text\": \"Идентификатор записи\",\r\n                    \"Property\": \"ID\",\r\n                    \"Editor\": {\r\n                      \"TextBox\": {\r\n                        \"Value\": {\r\n                          \"PropertyBinding\": {\r\n                            \"DataSource\": \"EditDataSource\",\r\n                            \"Property\": \"ID\"\r\n                          }\r\n                        }\r\n                      }\r\n                    }\r\n                  },\r\n                  {\r\n                    \"Text\": \"Код элемента\",\r\n                    \"Property\": \"Code\",\r\n                    \"Editor\": {\r\n                      \"TextBox\": {\r\n                        \"Value\": {\r\n                          \"PropertyBinding\": {\r\n                            \"DataSource\": \"EditDataSource\",\r\n                            \"Property\": \"Code\"\r\n                          }\r\n                        }\r\n                      }\r\n                    }\r\n                  },\r\n                  {\r\n                    \"Text\": \"Наименование элемента\",\r\n                    \"Property\": \"Name\",\r\n                    \"Editor\": {\r\n                      \"TextBox\": {\r\n                        \"Value\": {\r\n                          \"PropertyBinding\": {\r\n                            \"DataSource\": \"EditDataSource\",\r\n                            \"Property\": \"Name\"\r\n                          }\r\n                        }\r\n                      }\r\n                    }\r\n                  }\r\n                ]\r\n              }\r\n            ]\r\n          }\r\n        }\r\n      ]\r\n    }\r\n  },\r\n  \"ToolBar\": {\r\n    \"Name\": \"Toolbar1\",\r\n    \"Items\": [\r\n      {\r\n        \"Button\": {\r\n          \"Name\": \"SaveButton\",\r\n          \"Text\": \"Сохранить\",\r\n          \"Action\": {\r\n            \"SaveAction\": {\r\n              \"Items\": {\r\n                \"PropertyBinding\": {\r\n                  \"DataSource\": \"EditDataSource\"\r\n                }\r\n              }\r\n            }\r\n          }\r\n        }\r\n      },\r\n      {\r\n        \"Button\": {\r\n          \"Name\": \"CancelButton\",\r\n          \"Text\": \"Отменить\",\r\n          \"Action\": {\r\n            \"CancelAction\": {\r\n              \"Items\": {\r\n                \"PropertyBinding\": {\r\n                  \"DataSource\": \"EditDataSource\"\r\n                }\r\n              }\r\n            }\r\n          }\r\n        }\r\n      }\r\n    ]\r\n  }\r\n}".Replace("\r\n", Environment.NewLine));
        }

        [Test]
        public void ShouldBuildMetadataListView()
        {
            string configId = "System";

            dynamic classifierMetadata = new DynamicWrapper();
            classifierMetadata.CodeSystemName = "Аллергены";
            classifierMetadata.Id = "1.2.643.5.1.13.2.7.1.55_4";

            var metadataObject = new DynamicWrapper();
            var metadata = metadataObject
                .BuildText((string)classifierMetadata.CodeSystemName)
                .BuildDataSourcesList(
                    dsl =>
                    dsl.BuildDataSource(
                        ds => ds.BuildClassifierDataSource("ListDataSource", configId, (string)classifierMetadata.Id)))
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
                                                                                                                                configId,
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
                                                                                                                                configId,
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

            Assert.AreEqual(metadata.ToString(),
                            "{\r\n  \"Text\": \"Аллергены\",\r\n  \"DataSources\": [\r\n    {\r\n      \"ClassifierDataSource\": {\r\n        \"Name\": \"ListDataSource\",\r\n        \"ConfigId\": \"System\",\r\n        \"ClassifierMetadataId\": \"1.2.643.5.1.13.2.7.1.55_4\"\r\n      }\r\n    }\r\n  ],\r\n  \"LayoutPanel\": {\r\n    \"StackPanel\": {\r\n      \"Name\": \"StackPanel1\",\r\n      \"Items\": [\r\n        {\r\n          \"SearchPanel\": {\r\n            \"Name\": \"SearchPanel1\",\r\n            \"Items\": {\r\n              \"PropertyBinding\": {\r\n                \"DataSource\": \"ListDataSource\"\r\n              }\r\n            }\r\n          }\r\n        },\r\n        {\r\n          \"DataGrid\": {\r\n            \"Name\": \"DataGrid1\",\r\n            \"Columns\": [\r\n              {\r\n                \"Name\": \"ColumnId\",\r\n                \"Text\": \"Идентификатор элемента\",\r\n                \"Property\": \"Id\"\r\n              },\r\n              {\r\n                \"Name\": \"ColumnCode\",\r\n                \"Text\": \"Код элемента\",\r\n                \"Property\": \"Code\"\r\n              },\r\n              {\r\n                \"Name\": \"ColumnName\",\r\n                \"Text\": \"Наименование элемента\",\r\n                \"Property\": \"Name\"\r\n              }\r\n            ],\r\n            \"Items\": {\r\n              \"PropertyBinding\": {\r\n                \"DataSource\": \"ListDataSource\"\r\n              }\r\n            }\r\n          }\r\n        },\r\n        {\r\n          \"ToolBar\": {\r\n            \"Name\": \"ToolBar1\",\r\n            \"Items\": [\r\n              {\r\n                \"Button\": {\r\n                  \"Name\": \"AddButton\",\r\n                  \"Text\": \"Добавить\",\r\n                  \"Action\": {\r\n                    \"AddAction\": {\r\n                      \"View\": {\r\n                        \"DefaultEditView\": {\r\n                          \"DataSource\": {\r\n                            \"ClassifierDataSource\": {\r\n                              \"Name\": \"EditDataSource\",\r\n                              \"ConfigId\": \"System\",\r\n                              \"ClassifierMetadataId\": \"1.2.643.5.1.13.2.7.1.55_4\"\r\n                            }\r\n                          }\r\n                        }\r\n                      },\r\n                      \"Items\": {\r\n                        \"PropertyBinding\": {\r\n                          \"DataSource\": \"ListDataSource\"\r\n                        }\r\n                      }\r\n                    }\r\n                  }\r\n                }\r\n              },\r\n              {\r\n                \"Button\": {\r\n                  \"Name\": \"ChangeButton\",\r\n                  \"Text\": \"Изменить\",\r\n                  \"Action\": {\r\n                    \"EditAction\": {\r\n                      \"View\": {\r\n                        \"DefaultEditView\": {\r\n                          \"DataSource\": {\r\n                            \"ClassifierDataSource\": {\r\n                              \"Name\": \"EditDataSource\",\r\n                              \"ConfigId\": \"System\",\r\n                              \"ClassifierMetadataId\": \"1.2.643.5.1.13.2.7.1.55_4\"\r\n                            }\r\n                          },\r\n                          \"Value\": {\r\n                            \"DataSource\": \"ListDataSource\"\r\n                          }\r\n                        }\r\n                      },\r\n                      \"Items\": {\r\n                        \"PropertyBinding\": {\r\n                          \"DataSource\": \"ListDataSource\"\r\n                        }\r\n                      }\r\n                    }\r\n                  }\r\n                }\r\n              },\r\n              {\r\n                \"Button\": {\r\n                  \"Name\": \"DeleteButton\",\r\n                  \"Text\": \"Удалить\",\r\n                  \"Action\": {\r\n                    \"DeleteAction\": {\r\n                      \"Items\": {\r\n                        \"PropertyBinding\": {\r\n                          \"DataSource\": \"ListDataSource\"\r\n                        }\r\n                      }\r\n                    }\r\n                  }\r\n                }\r\n              }\r\n            ],\r\n            \"Separator\": {\r\n              \"Name\": \"Separator1\"\r\n            }\r\n          }\r\n        }\r\n      ]\r\n    }\r\n  }\r\n}".Replace("\r\n", Environment.NewLine));
        }

        [Test]
        public void ShouldBuildMetadataMenu()
        {
            var metadataObject = new DynamicWrapper();
            var metadata = metadataObject
                .BuildProperty("Name", "NSI")
                .BuildProperty("Caption", "Справочники НСИ")
                .BuildProperty("Description", "Главное меню конфигурации справочников НСИ")
                .BuildMenuItem("Основное", mi => mi
                                                     .BuildMenuItem("Просмотр справочников НСИ",
                                                                    min =>
                                                                    min.BuildOpenViewAction(
                                                                        ov => ov.BuildSelectListView(
                                                                            slv => slv
                                                                                       .BuildText(
                                                                                           "Выберите классификатор")
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
                                                                                       .BuildScript(
                                                                                           sc =>
                                                                                           sc.BuildScriptProperties(
                                                                                               "RunForm",
                                                                                               GetJavaScriptOnSelectedHandler
                                                                                                   ()))
                                                                                       .BuildSelectedValueEvent(
                                                                                           "RunForm")
                                                                                  ))));
            Assert.AreEqual(metadata.ToString(),
                            "{\r\n  \"Name\": \"NSI\",\r\n  \"Caption\": \"Справочники НСИ\",\r\n  \"Description\": \"Главное меню конфигурации справочников НСИ\",\r\n  \"Items\": [\r\n    {\r\n      \"MenuItem\": {\r\n        \"Text\": \"Основное\",\r\n        \"Items\": [\r\n          {\r\n            \"MenuItem\": {\r\n              \"Text\": \"Просмотр справочников НСИ\",\r\n              \"Action\": {\r\n                \"OpenViewAction\": {\r\n                  \"View\": {\r\n                    \"SelectListView\": {\r\n                      \"Text\": \"Выберите классификатор\",\r\n                      \"DataSources\": [\r\n                        {\r\n                          \"MetadataDataSource\": {\r\n                            \"Name\": \"SelectDataSource\",\r\n                            \"MetadataType\": \"Classifier\"\r\n                          }\r\n                        }\r\n                      ],\r\n                      \"LayoutPanel\": {\r\n                        \"StackPanel\": {\r\n                          \"Name\": \"StackPanel1\",\r\n                          \"Items\": [\r\n                            {\r\n                              \"SearchPanel\": {\r\n                                \"Name\": \"SearchPanel1\",\r\n                                \"Items\": {\r\n                                  \"PropertyBinding\": {\r\n                                    \"DataSource\": \"SelectDataSource\"\r\n                                  }\r\n                                }\r\n                              }\r\n                            },\r\n                            {\r\n                              \"DataGrid\": {\r\n                                \"Name\": \"DataGrid1\",\r\n                                \"Columns\": [\r\n                                  {\r\n                                    \"Name\": \"ColumnId\",\r\n                                    \"Text\": \"Идентификатор элемента\",\r\n                                    \"Property\": \"Id\"\r\n                                  },\r\n                                  {\r\n                                    \"Name\": \"ColumnCode\",\r\n                                    \"Text\": \"Код элемента\",\r\n                                    \"Property\": \"Code\"\r\n                                  },\r\n                                  {\r\n                                    \"Name\": \"ColumnName\",\r\n                                    \"Text\": \"Наименование элемента\",\r\n                                    \"Property\": \"Name\"\r\n                                  }\r\n                                ],\r\n                                \"Items\": {\r\n                                  \"PropertyBinding\": {\r\n                                    \"DataSource\": \"SelectDataSource\"\r\n                                  }\r\n                                }\r\n                              }\r\n                            }\r\n                          ]\r\n                        }\r\n                      },\r\n                      \"Scripts\": [\r\n                        {\r\n                          \"Name\": \"RunForm\",\r\n                          \"Body\": \"CSAgIGNvbnRleHQuT3BlblZpZXcoewkJCQkJCQkJCQkJCQlWaWV3IDogewkJCQkJCQkJCQkJCQkJRGVmYXVsdExpc3RWaWV3IDogewkJCQkJCQkJCQkJCQlEYXRhU291cmNlIDogewkJCQkJCQkJCQkJCQlDbGFzc2lmaWVyRGF0YVNvdXJjZSA6IHsJCQkJCQkJCQkJCU5hbWUgOiAiTGlzdERhdGFTb3VyY2UiLAkJCQkJCQkJCQlDb25maWdJZCA6ICJTeXN0ZW0iLAkJCQkJCQkJCQkJQ2xhc3NpZmllcklkIDogY29udGV4dC5TZWxlY3RlZFZhbHVlLklkCQkJCQkJfQkJCQkJCQkJCQkJCQkJCQl9CQkJCQkJCQkJCQkJCQkJfQkJCQkJCQkJCQkJCQkJCQl9CQkJCQkJCQkJCQkJCQkJfQkJCQkJCQkJCQkJCQkJCX0pOwkJCQkJCQkJCQkJCQkJCQ==\"\r\n                        }\r\n                      ],\r\n                      \"OnValueSelected\": {\r\n                        \"Name\": \"RunForm\"\r\n                      }\r\n                    }\r\n                  }\r\n                }\r\n              }\r\n            }\r\n          }\r\n        ]\r\n      }\r\n    }\r\n  ]\r\n}".Replace("\r\n", Environment.NewLine));
        }

        [Test]
        public void ShouldBuildMetadataSelectView()
        {
            dynamic classifierMetadata = new DynamicWrapper();
            classifierMetadata.CodeSystemName = "Аллергены";
            classifierMetadata.Id = "1.2.643.5.1.13.2.7.1.55_4";

            var metadataObject = new DynamicWrapper();
            var metadata = metadataObject
                .BuildText("Выберите классификатор")
                .BuildDataSourcesList(
                    dsl =>
                    dsl.BuildDataSource(
                        ds => ds.BuildMetadataDataSource("SelectDataSource", "ClassifierStorage", "Classifier")))
                .BuildStackLayoutPanel("StackPanel1", sl => sl
                                                                .BuildDataElement(
                                                                    de =>
                                                                    de.BuildSearchPanel("SearchPanel1",
                                                                                        sp =>
                                                                                        sp.BuildDataBinding(
                                                                                            db =>
                                                                                            db.BuildPropertyBinding(
                                                                                                "SelectDataSource"))))
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
                                                                                                                       ("SelectDataSource"))
                                                                              )))
                .BuildScript(sc => sc.BuildScriptProperties("RunForm", GetJavaScriptOnSelectedHandler()))
                .BuildSelectedValueEvent("RunForm");

            Assert.AreEqual(metadata.ToString(),
                            "{\r\n  \"Text\": \"Выберите классификатор\",\r\n  \"DataSources\": [\r\n    {\r\n      \"MetadataDataSource\": {\r\n        \"Name\": \"SelectDataSource\",\r\n        \"MetadataType\": \"Classifier\"\r\n      }\r\n    }\r\n  ],\r\n  \"LayoutPanel\": {\r\n    \"StackPanel\": {\r\n      \"Name\": \"StackPanel1\",\r\n      \"Items\": [\r\n        {\r\n          \"SearchPanel\": {\r\n            \"Name\": \"SearchPanel1\",\r\n            \"Items\": {\r\n              \"PropertyBinding\": {\r\n                \"DataSource\": \"SelectDataSource\"\r\n              }\r\n            }\r\n          }\r\n        },\r\n        {\r\n          \"DataGrid\": {\r\n            \"Name\": \"DataGrid1\",\r\n            \"Columns\": [\r\n              {\r\n                \"Name\": \"ColumnId\",\r\n                \"Text\": \"Идентификатор элемента\",\r\n                \"Property\": \"Id\"\r\n              },\r\n              {\r\n                \"Name\": \"ColumnCode\",\r\n                \"Text\": \"Код элемента\",\r\n                \"Property\": \"Code\"\r\n              },\r\n              {\r\n                \"Name\": \"ColumnName\",\r\n                \"Text\": \"Наименование элемента\",\r\n                \"Property\": \"Name\"\r\n              }\r\n            ],\r\n            \"Items\": {\r\n              \"PropertyBinding\": {\r\n                \"DataSource\": \"SelectDataSource\"\r\n              }\r\n            }\r\n          }\r\n        }\r\n      ]\r\n    }\r\n  },\r\n  \"Scripts\": [\r\n    {\r\n      \"Name\": \"RunForm\",\r\n      \"Body\": \"CSAgIGNvbnRleHQuT3BlblZpZXcoewkJCQkJCQkJCQkJCQlWaWV3IDogewkJCQkJCQkJCQkJCQkJRGVmYXVsdExpc3RWaWV3IDogewkJCQkJCQkJCQkJCQlEYXRhU291cmNlIDogewkJCQkJCQkJCQkJCQlDbGFzc2lmaWVyRGF0YVNvdXJjZSA6IHsJCQkJCQkJCQkJCU5hbWUgOiAiTGlzdERhdGFTb3VyY2UiLAkJCQkJCQkJCQlDb25maWdJZCA6ICJTeXN0ZW0iLAkJCQkJCQkJCQkJQ2xhc3NpZmllcklkIDogY29udGV4dC5TZWxlY3RlZFZhbHVlLklkCQkJCQkJfQkJCQkJCQkJCQkJCQkJCQl9CQkJCQkJCQkJCQkJCQkJfQkJCQkJCQkJCQkJCQkJCQl9CQkJCQkJCQkJCQkJCQkJfQkJCQkJCQkJCQkJCQkJCX0pOwkJCQkJCQkJCQkJCQkJCQ==\"\r\n    }\r\n  ],\r\n  \"OnValueSelected\": {\r\n    \"Name\": \"RunForm\"\r\n  }\r\n}".Replace("\r\n", Environment.NewLine));
        }
    }
}
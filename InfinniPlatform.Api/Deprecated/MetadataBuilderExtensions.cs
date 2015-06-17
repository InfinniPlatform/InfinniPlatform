using System;
using System.Collections.Generic;
using InfinniPlatform.Api.Context;
using InfinniPlatform.Api.ContextTypes;
using InfinniPlatform.Api.Dynamic;
using InfinniPlatform.Api.Metadata;

namespace InfinniPlatform.Api.Deprecated
{
	public static class MetadataBuilderExtensions
	{
		public static object InvokeDynamicOperation(this object buildObject, Action<dynamic> dynamicObjectAction)
		{
			if (dynamicObjectAction != null)
			{
				dynamicObjectAction((dynamic)buildObject);
			}
			return buildObject;
		}

		private static void InvokeAction(this object buildObject, Action<object> initObjectAction)
		{
			if (initObjectAction != null)
			{
				initObjectAction((dynamic)buildObject);
			}
		}




		#region Common

		public static object BuildId(this object buildObject, string id)
		{
			buildObject.InvokeDynamicOperation(dynamicObject =>
			{
				dynamicObject.Id = id;
			});
			return buildObject;
		}

		public static object BuildText(this object buildObject, string text)
		{
			buildObject.InvokeDynamicOperation(dynamicObject =>
			{
				dynamicObject.Text = text;
			});
			return buildObject;
		}

		public static object BuildName(this object buildObject, string name)
		{
			buildObject.InvokeDynamicOperation(dynamicObject =>
			{
				dynamicObject.Name = name;
			});
			return buildObject;
		}

		public static object BuildCaption(this object buildObject, string caption)
		{
			buildObject.InvokeDynamicOperation(dynamicObject =>
			{
				dynamicObject.Caption = caption;
			});
			return buildObject;
		}

		public static object BuildDescription(this object buildObject, string description)
		{
			buildObject.InvokeDynamicOperation(dynamicObject =>
			{
				dynamicObject.Description = description;
			});
			return buildObject;
		}

		public static object BuildContainer(this object buildObject, string name, Action<object> propertyAction = null)
		{
			return buildObject.InvokeDynamicOperation(dynamicObject =>
			{
				var instance = new DynamicWrapper();
				dynamicObject[name] = instance;
				if (propertyAction != null)
				{
					propertyAction(instance);
				}
			});
		}

		public static object BuildProperty(this object buildObject, string name, object value)
		{
			return buildObject.InvokeDynamicOperation(dynamicObject => dynamicObject[name] = value);
		}


		public static object BuildCollectionProperty(this object buildObject, string collectionName, Action<dynamic> collectionAction = null)
		{
			buildObject.InvokeDynamicOperation(dynamicObject =>
			{
				dynamic dynamicCollection = dynamicObject[collectionName];
				if (dynamicCollection == null)
				{
					dynamicCollection = new List<dynamic>();
					dynamicObject[collectionName] = dynamicCollection;
				}
				InvokeAction(dynamicCollection, collectionAction);
			});
			return buildObject;
		}

		public static object BuildCollectionItem(this object buildObject, string collectionName,
												 Action<object> collectionItemAction = null)
		{
			BuildCollectionProperty(buildObject, collectionName, items =>
																			{
																				dynamic item = new DynamicWrapper();
																				items.Add(item);
																				InvokeAction(item, collectionItemAction);
																			});
			return buildObject;
		}

        public static object BuildCollectionItem(this object buildObject, string collectionName,
                                                 object collectionItem)
        {
            BuildCollectionProperty(buildObject, collectionName, items => items.Add(collectionItem));
            return buildObject;
        }

		private static object BuildItemsList(this object buildObject, Action<dynamic> buildItemsListAction)
		{
			return buildObject.BuildCollectionProperty("Items", buildItemsListAction);
		}

		public static object BuildScripts(this object buildObject, Action<dynamic> scriptsAction)
		{
			return buildObject.BuildCollectionProperty("Scripts", scriptsAction);
		}


		#endregion

		#region Создание Layout
		public static object BuildStackLayoutPanel(this object buildObject, string panelName, Action<object> layoutPanelAction)
		{
			buildObject.InvokeDynamicOperation(dynamicObject =>
			{
				dynamicObject.LayoutPanel = new DynamicWrapper();
				dynamicObject.LayoutPanel.StackPanel = new DynamicWrapper();
				dynamicObject.LayoutPanel.StackPanel.Name = panelName;
				InvokeAction(dynamicObject.LayoutPanel.StackPanel, layoutPanelAction);
			});
			return buildObject;
		}


		public static object BuildTabPanel(this object buildObject, string panelName, Action<object> layoutPanelAction)
		{
			buildObject.InvokeDynamicOperation(dynamicObject =>
			{
				dynamicObject.LayoutPanel = new DynamicWrapper();
				dynamicObject.LayoutPanel.TabPanel = new DynamicWrapper();
				dynamicObject.LayoutPanel.TabPanel.Name = panelName;
				InvokeAction(dynamicObject.LayoutPanel.TabPanel, layoutPanelAction);
			});
			return buildObject;
		}

        public static object BuildPanel(this object buildObject, string panelName, Action<object> layoutPanelAction)
        {
            buildObject.InvokeDynamicOperation(dynamicObject =>
            {
                dynamicObject.LayoutPanel = new DynamicWrapper();
                dynamicObject.LayoutPanel.Panel = new DynamicWrapper();
                dynamicObject.LayoutPanel.Panel.Name = panelName;
                InvokeAction(dynamicObject.LayoutPanel.Panel, layoutPanelAction);
            });
            return buildObject;
        }


		private static object BuildTabPages(this object buildObject, Action<dynamic> tabPagesAction)
		{
			buildObject.InvokeDynamicOperation(dynamicObject =>
			{
				if (dynamicObject.Pages == null)
				{
					dynamicObject.Pages = new List<dynamic>();
				}
				InvokeAction(dynamicObject.Pages, tabPagesAction);
			});
			return buildObject;
		}

		public static object BuildTabPage(this object buildObject, string text, Action<object> tabPageAction)
		{
			buildObject.BuildTabPages(dynamicObject =>
										  {
											  dynamic page = new DynamicWrapper();
											  page.Text = text;
											  dynamicObject.Add(page);
											  InvokeAction(page, tabPageAction);
										  });
			return buildObject;
		}


		public static object BuildDisableClose(this object buildObject)
		{
			buildObject.InvokeDynamicOperation(dynamicObject =>
			{
				dynamicObject.CanClose = false;
			});
			return buildObject;

		}

		#endregion

		#region Создание контролов

		#region searchpanel
		public static object BuildSearchPanel(this object buildObject, string name, Action<object> searchPanelAction)
		{
			buildObject.InvokeDynamicOperation(dynamicObject =>
			{
				dynamic searchPanel = new DynamicWrapper();
				searchPanel.Name = name;
				dynamicObject.SearchPanel = searchPanel;
				InvokeAction(searchPanel, searchPanelAction);
			});
			return buildObject;
		}



		#endregion

		#region datagrid
		public static object BuildDataGrid(this object buildObject, string name, Action<object> dataGridAction)
		{
			buildObject.InvokeDynamicOperation(dynamicObject =>
				{
					dynamicObject.DataGrid = new DynamicWrapper();
					dynamicObject.DataGrid.Name = name;
					InvokeAction(dynamicObject.DataGrid, dataGridAction);
				});
			return buildObject;
		}

		private static object BuildDataGridColumns(this object buildObject, Action<dynamic> columnsAction)
		{
			return buildObject.BuildCollectionProperty("Columns", columnsAction);
		}


		public static object BuildDataGridColumn(this object buildObject, Action<object> columnAction)
		{
			buildObject.BuildDataGridColumns(dynamicObject =>
				{
					var gridColumn = new DynamicWrapper();
					dynamicObject.Add(gridColumn);
					InvokeAction(gridColumn, columnAction);
				});
			return buildObject;
		}

		public static object BuildDataGridColumnProperties(this object buildObject, string name, string text,
														   string property)
		{
			buildObject.InvokeDynamicOperation(dynamicObject =>
				{
					dynamicObject.Name = name;
					dynamicObject.Text = text;
					dynamicObject.Property = property;
				});
			return buildObject;
		}
		#endregion

		#region treeList
		public static object BuildTreeView(this object buildObject, string name, Action<object> treeViewAction)
		{
			buildObject.InvokeDynamicOperation(dynamicObject =>
			{
				dynamicObject.TreeView = new DynamicWrapper();
				dynamicObject.TreeView.Name = name;
				InvokeAction(dynamicObject.TreeView, treeViewAction);
			});
			return buildObject;
		}

		public static object BuildTreeViewKeyProperty(this object buildObject, string propertyName)
		{
			buildObject.InvokeDynamicOperation(dynamicObject =>
			{
				dynamicObject.KeyProperty = propertyName;
			});
			return buildObject;
		}

		public static object BuildTreeViewParentProperty(this object buildObject, string propertyName)
		{
			buildObject.InvokeDynamicOperation(dynamicObject =>
			{
				dynamicObject.ParentProperty = propertyName;
			});
			return buildObject;
		}

		public static object BuildTreeViewDisplayProperty(this object buildObject, string propertyName)
		{
			buildObject.InvokeDynamicOperation(dynamicObject =>
			{
				dynamicObject.DisplayProperty = propertyName;
			});
			return buildObject;
		}
		#endregion

		#region toolbar
		public static object BuildToolBar(this object buildObject, string name, Action<object> toolbarAction)
		{
			buildObject.InvokeDynamicOperation(dynamicObject =>
			{
				dynamic toolbar = new DynamicWrapper();
				toolbar.Name = name;
				dynamicObject.ToolBar = toolbar;
				InvokeAction(toolbar, toolbarAction);

			});
			return buildObject;
		}

		public static object BuildButton(this object buildObject, Action<object> buttonAction)
		{
			buildObject.BuildItemsList(dynamicObject =>
			{
				dynamic button = new DynamicWrapper();
				dynamicObject.Add(button);
				button.Button = new DynamicWrapper();
				InvokeAction(button.Button, buttonAction);
			});
			return buildObject;
		}

		public static object BuildButtonProperties(this object buildObject, string name, string text, Action<object> actionButtonAction)
		{
			buildObject.InvokeDynamicOperation(dynamicObject =>
				{
					dynamicObject.Name = name;
					dynamicObject.Text = text;
					InvokeAction(dynamicObject, actionButtonAction);
				});
			return buildObject;
		}

		public static object BuildButtonActionAddDefault(this object buildObject, string dataSourceName, Action<object> actionDefaultEditFormDataSource)
		{
			buildObject.InvokeDynamicOperation(dynamicObject =>
				{
					dynamicObject.Action = new DynamicWrapper();
					dynamicObject.Action.AddAction = new DynamicWrapper();
					dynamicObject.Action.AddAction.View = new DynamicWrapper();
					dynamicObject.Action.AddAction.View.DefaultEditView = new DynamicWrapper();
					BuildDataSourceReference(dynamicObject.Action.AddAction.View.DefaultEditView, actionDefaultEditFormDataSource);
					Action<object> action = db => db.BuildPropertyBinding(dataSourceName);
					BuildDataBinding(dynamicObject.Action.AddAction, action);
				});
			return buildObject;
		}

		public static object BuildButtonActionEditDefault(this object buildObject, string dataSourceName, string editValuePropertyBinding,
														  Action<object> actionDefaultEditFormDataSource)
		{
			buildObject.InvokeDynamicOperation(dynamicObject =>
			{
				dynamicObject.Action = new DynamicWrapper();
				dynamicObject.Action.EditAction = new DynamicWrapper();
				dynamicObject.Action.EditAction.View = new DynamicWrapper();
				dynamicObject.Action.EditAction.View.DefaultEditView = new DynamicWrapper();
				BuildDataSourceReference(dynamicObject.Action.EditAction.View.DefaultEditView, actionDefaultEditFormDataSource);
				dynamicObject.Action.EditAction.View.DefaultEditView.Value = new DynamicWrapper();
				BuildPropertyBinding(dynamicObject.Action.EditAction.View.DefaultEditView.Value,
									 editValuePropertyBinding);
				Action<object> action = db => db.BuildPropertyBinding(dataSourceName);
				BuildDataBinding(dynamicObject.Action.EditAction, action);

			});
			return buildObject;
		}

		public static object BuildButtonActionDelete(this object buildObject, string dataSourceName)
		{
			buildObject.InvokeDynamicOperation(dynamicObject =>
			{
				dynamicObject.Action = new DynamicWrapper();
				dynamicObject.Action.DeleteAction = new DynamicWrapper();
				Action<object> action = db => db.BuildPropertyBinding(dataSourceName);
				BuildDataBinding(dynamicObject.Action.DeleteAction, action);
			});
			return buildObject;
		}


		public static object BuildButtonSeparator(this object buildObject)
		{
			buildObject.InvokeDynamicOperation(dynamicObject =>
			{
				dynamicObject.Separator = new DynamicWrapper();
				dynamicObject.Separator.Name = "Separator1";
			});
			return buildObject;
		}

		public static object BuildButtonActionSave(this object buildObject, string dataSourceName)
		{
			buildObject.InvokeDynamicOperation(dynamicObject =>
			{
				dynamicObject.Action = new DynamicWrapper();
				dynamicObject.Action.SaveAction = new DynamicWrapper();
				Action<object> action = db => db.BuildPropertyBinding(dataSourceName);
				BuildDataBinding(dynamicObject.Action.SaveAction, action);
			});
			return buildObject;
		}

		public static object BuildButtonActionCancel(this object buildObject, string dataSourceName)
		{
			buildObject.InvokeDynamicOperation(dynamicObject =>
			{
				dynamicObject.Action = new DynamicWrapper();
				dynamicObject.Action.CancelAction = new DynamicWrapper();
				Action<object> action = db => db.BuildPropertyBinding(dataSourceName);
				BuildDataBinding(dynamicObject.Action.CancelAction, action);
			});
			return buildObject;
		}

		#endregion


		#region propertygrid
		public static object BuildPropertyGrid(this object buildObject, string name, Action<object> propertyGridAction)
		{
			buildObject.InvokeDynamicOperation(dynamicObject =>
			{
				dynamic propertyGrid = new DynamicWrapper();
				propertyGrid.Name = name;
				dynamicObject.PropertyGrid = propertyGrid;
				InvokeAction(propertyGrid, propertyGridAction);
			});
			return buildObject;
		}

		private static object BuildPropertyGridCategories(this object buildObject, Action<dynamic> categoriesAction)
		{
			return buildObject.BuildCollectionProperty("Categories", categoriesAction);
		}

		private static object BuildCategoryProperties(this object buildObject, Action<dynamic> propertiesAction)
		{
			return buildObject.BuildCollectionProperty("Properties", propertiesAction);
		}

		public static object BuildPropertyGridCategory(this object buildObject, string text,
													   Action<object> propertyGridAction)
		{
			buildObject.BuildPropertyGridCategories(items =>
				{
					dynamic category = new DynamicWrapper();
					category.Text = text;
					items.Add(category);
					InvokeAction(category, propertyGridAction);
				});
			return buildObject;
		}

		public static object BuildPropertyGridTextBox(this object buildObject, string text, string propertyName, Action<object> textBoxAction)
		{
			buildObject.BuildCategoryProperties(properties =>
			{
				dynamic editor = new DynamicWrapper();
				editor.Text = text;
				editor.Property = propertyName;
				editor.Editor = new DynamicWrapper();
				editor.Editor.TextBox = new DynamicWrapper();
				properties.Add(editor);
				InvokeAction(editor.Editor.TextBox, textBoxAction);
			});
			return buildObject;
		}

        public static object BuildPropertyGridLabel(this object buildObject, string text, string propertyName, Action<object> labelAction)
        {
            buildObject.BuildCategoryProperties(properties =>
            {
                dynamic label = new DynamicWrapper();
                label.Text = text;
                label.Property = propertyName;
                label.Editor = new DynamicWrapper();
                label.Editor.Label = new DynamicWrapper();
                properties.Add(label);
                InvokeAction(label.Editor.Label, labelAction);
            });
            return buildObject;
        }

		#endregion

		#region menuControl
		public static object BuildMenuBar(this object buildObject, string name, string configId)
		{
			buildObject.InvokeDynamicOperation(dynamicObject =>
			{
				dynamicObject.MenuBar = new DynamicWrapper();
				dynamicObject.MenuBar.Name = name;
				dynamicObject.MenuBar.ConfigId = configId;
			});
			return buildObject;

		}

		#endregion

		#region filterPanel
		public static object BuildFilterPanel(this object buildObject, string name, Action<object> filterPanelAction)
		{
			buildObject.InvokeDynamicOperation(dynamicObject =>
			{
				dynamic filterPanel = new DynamicWrapper();
				filterPanel.Name = name;
				dynamicObject.FilterPanel = filterPanel;
				InvokeAction(filterPanel, filterPanelAction);
			});
			return buildObject;
		}

		public static object BuildFilterPanelGeneralProperties(this object buildObject, Action<object> generalPropertiesAction)
		{
			return buildObject.BuildCollectionProperty("GeneralProperties",generalPropertiesAction);
		}

		public static object BuildFilterPanelAdditionalProperties(this object buildObject, Action<object> additionalPropertiesAction)
		{
            return buildObject.BuildCollectionProperty("AdditionalProperties", additionalPropertiesAction);
		}


		public static object BuildStringFilterProperty(this object buildObject, string text, string property, Action<object> propertyBindingAction)
		{
			buildObject.InvokeDynamicOperation(dynamicObject =>
			{
				dynamic filterProperty = new DynamicWrapper();
				filterProperty.Property = property;
				filterProperty.Text = text;
				filterProperty.DefaultOperator = 1;

			    var operators = new[] {1, 2, 64, 128, 256, 512, 1024, 2048, 4096, 8192};

			    int i = 0;
                foreach (var @operator in operators)
                {
                    dynamic operatorInstance = new DynamicWrapper();
                    operatorInstance.Operator = @operator;
                    operatorInstance.Editor = new DynamicWrapper();
                    operatorInstance.Editor.TextBox = new DynamicWrapper();
                    operatorInstance.Editor.TextBox.Name = string.Format("TextBox_{0}_{1}", property, i);
                    InvokeAction(operatorInstance.Editor.TextBox, propertyBindingAction);
                    i++;
                    BuildCollectionItem(filterProperty, "Operators", operatorInstance);
                }
			    dynamicObject.Add(filterProperty);

			});
			return buildObject;
		}

		#endregion
        #endregion

        #region Источники данных

        public static object BuildDataSource(this object buildObject, Action<object> initDataSource)
		{
			buildObject.InvokeDynamicOperation(dynamicObject =>
			{
				dynamic dynamicDataSource = new DynamicWrapper();
				dynamicObject.Add(dynamicDataSource);
				InvokeAction(dynamicDataSource, initDataSource);
			});

			return buildObject;
		}

		private static object BuildDataSourceReference(this object buildObject, Action<object> dataSourceAction)
		{
			buildObject.InvokeDynamicOperation(dynamicObject =>
			{
				dynamic dynamicDataSource = new DynamicWrapper();
				dynamicObject.DataSource = dynamicDataSource;
				InvokeAction(dynamicDataSource, dataSourceAction);
			});

			return buildObject;
		}

		public static object BuildDataSourcesList(this object buildObject, Action<object> dataSourcesListAction)
		{
			return buildObject.BuildCollectionProperty("DataSources", dataSourcesListAction);
		}


		public static object BuildClassifierDataSource(this object buildObject, string name, string configId, string classifierMetadataId, Action<object> dataSourcePropertiesAction = null)
		{
			buildObject.InvokeDynamicOperation(dynamicObject =>
			{
				dynamicObject.ClassifierDataSource = new DynamicWrapper();

				dynamicObject.ClassifierDataSource.Name = name;
				dynamicObject.ClassifierDataSource.ConfigId = configId;
				dynamicObject.ClassifierDataSource.ClassifierMetadataId = classifierMetadataId;
                InvokeAction(dynamicObject.ClassifierDataSource,dataSourcePropertiesAction);
			});

			return buildObject;
		}

        public static object BuildDocumentDataSource(this object buildObject, string name, string configId, string documentId, Action<object> dataSourcePropertiesAction = null)
        {
            buildObject.InvokeDynamicOperation(dynamicObject =>
            {
                dynamicObject.DocumentDataSource = new DynamicWrapper();

                dynamicObject.DocumentDataSource.Name = name;
                dynamicObject.DocumentDataSource.ConfigId = configId;
                dynamicObject.DocumentDataSource.DocumentId = documentId;
                InvokeAction(dynamicObject.DocumentDataSource, dataSourcePropertiesAction);
            });

            return buildObject;
        }

        public static object BuildObjectDataSource(this object buildObject, string name, object[] items)
        {
            buildObject.InvokeDynamicOperation(dynamicObject =>
            {
                dynamicObject.ObjectDataSource = new DynamicWrapper();

                dynamicObject.ObjectDataSource.Name = name;
                dynamicObject.ObjectDataSource.Value = items;                
            });

            return buildObject;
        }

        public static object BuildMetadataDataSource(this object buildObject, string name, string configId, string metadataType)
		{
			buildObject.InvokeDynamicOperation(dynamicObject =>
			{
				dynamicObject.MetadataDataSource = new DynamicWrapper();
				dynamicObject.MetadataDataSource.Name = name;
				dynamicObject.MetadataDataSource.MetadataType = metadataType;
			});

			return buildObject;
		}

		public static object BuildPropertyBinding(this object buildObject, string dataSourceName, string propertyName = "")
		{
			buildObject.InvokeDynamicOperation(dynamicObject =>
				{
					dynamicObject.DataSource = dataSourceName;
					if (!string.IsNullOrEmpty(propertyName))
					{
						dynamicObject.Property = propertyName;
					}
				});

			return buildObject;
		}

		public static object BuildDataBinding(this object buildObject, Action<object> dataBindingAction)
		{
			buildObject.InvokeDynamicOperation(dynamicObject =>
			{
				if (dynamicObject.Items == null)
				{
					dynamicObject.Items = new DynamicWrapper();
				}
				dynamic propertyBindingElement = new DynamicWrapper();
				dynamicObject.Items.PropertyBinding = propertyBindingElement;
				InvokeAction(propertyBindingElement, dataBindingAction);
			});
			return buildObject;
		}


        public static object BuildObjectBinding(this object buildObject, object bindingValue)
        {
            buildObject.InvokeDynamicOperation(dynamicObject =>
                {
                    dynamicObject.ObjectBinding = new DynamicWrapper();
                    dynamicObject.ObjectBinding.Value = bindingValue;
                });
            return buildObject;
        }

		public static object BuildEditValueBinding(this object buildObject, Action<object> editValueBindingAction)
		{
			buildObject.InvokeDynamicOperation(dynamicObject =>
			{
				dynamic value = new DynamicWrapper();
				dynamicObject.Value = value;
				dynamicObject.Value.PropertyBinding = new DynamicWrapper();
				InvokeAction(dynamicObject.Value.PropertyBinding, editValueBindingAction);
			});
			return buildObject;
		}


        public static object BuildDefaultTreeFilter(this object buildObject)
        {
            buildObject.BuildCollectionProperty("DefaultFilter", items =>
                {
                    dynamic instance = new DynamicWrapper();
                    instance.Property = "ParentId";
                    instance.Value = null;
                    instance.CriteriaType = 1;
                    items.Add(instance);
                });
            return buildObject;
        }
		#endregion

		#region Создание элементов Layout
		public static object BuildDataElement(this object buildObject, Action<object> dataElementAction)
		{
			buildObject.BuildItemsList(items =>
				{
					dynamic dataElement = new DynamicWrapper();
					items.Add(dataElement);
					InvokeAction(dataElement, dataElementAction);
				});
			return buildObject;
		}

		public static object BuildActionElement(this object buildObject, Action<object> actionElementAction)
		{
			buildObject.BuildItemsList(items =>
			{
				dynamic actionElement = new DynamicWrapper();
				items.Add(actionElement);
				InvokeAction(actionElement, actionElementAction);
			});
			return buildObject;
		}
		#endregion

		#region Скрипты
		public static object BuildScript(this object buildObject, Action<object> scriptAction)
		{
			buildObject.BuildScripts(items =>
			{
				dynamic scriptElement = new DynamicWrapper();
				items.Add(scriptElement);
				InvokeAction(scriptElement, scriptAction);
			});
			return buildObject;
		}

		public static object BuildScriptProperties(this object buildObject, string scriptName, string base64scriptBody)
		{
			buildObject.InvokeDynamicOperation(dynamicObject =>
				{
					dynamicObject.Name = scriptName;
					dynamicObject.Body = base64scriptBody;
				});
			return buildObject;
		}

		#endregion

		#region events
		public static object BuildSelectedValueEvent(this object buildObject, string linkedScriptName)
		{
			buildObject.InvokeDynamicOperation(dynamicObject =>
				{
					dynamicObject.OnValueSelected = new DynamicWrapper();
					dynamicObject.OnValueSelected.Name = linkedScriptName;
				});
			return buildObject;
		}
		#endregion

		#region menu
		public static object BuildMenuItem(this object buildObject, string itemText, Action<object> menuItemAction)
		{
			buildObject.BuildItemsList(dynamicObject =>
				{
					dynamic menuItem = new DynamicWrapper();
					menuItem.MenuItem = new DynamicWrapper();
					dynamicObject.Add(menuItem);
					menuItem.MenuItem.Text = itemText;
					InvokeAction(menuItem.MenuItem, menuItemAction);
				});
			return buildObject;
		}

		public static object BuildOpenViewAction(this object buildObject, Action<object> openViewAction)
		{
			buildObject.InvokeDynamicOperation(dynamicObject =>
			{
				dynamic action = new DynamicWrapper();
				action.OpenViewAction = new DynamicWrapper();
				action.OpenViewAction.View = new DynamicWrapper();
				dynamicObject.Action = action;
				InvokeAction(action.OpenViewAction.View, openViewAction);
			});
			return buildObject;
		}

		#endregion

        #region LinkViews
        public static object BuildSelectListView(this object buildObject, Action<object> selectListViewAction)
        {
            buildObject.InvokeDynamicOperation(dynamicObject =>
            {
                dynamic selectListView = new DynamicWrapper();
                dynamicObject.SelectListView = selectListView;
                InvokeAction(selectListView, selectListViewAction);
            });
            return buildObject;
        }

        public static object BuildInlineView(this object buildObject, Action<object> inlineViewAction)
        {
            buildObject.InvokeDynamicOperation(dynamicObject =>
            {
                dynamic inlineView = new DynamicWrapper();
                dynamicObject.InlineView = inlineView;
                InvokeAction(inlineView, inlineViewAction);
            });
            return buildObject;
        }

        public static object BuildInlineViewContainer(this object buildObject, Action<object> inlineViewContainerAction)
        {
            buildObject.InvokeDynamicOperation(dynamicObject =>
            {
                dynamic inlineViewContainer = new DynamicWrapper();
                dynamicObject.View = inlineViewContainer;
                InvokeAction(inlineViewContainer, inlineViewContainerAction);
            });
            return buildObject;
        }

        #endregion


        #region Создание элементов метаданных конфигурации

        public static dynamic BuildConfiguration(string name, string caption, string description, string version)
        {
			var result = new DynamicWrapper();

	        result
		        .BuildName(name)
		        .BuildCaption(caption)
		        .BuildDescription(description)
                .BuildProperty("Version",version)
				.BuildCollectionProperty(MetadataType.MenuContainer)
				.BuildCollectionProperty(MetadataType.DocumentContainer)
		        .BuildCollectionProperty(MetadataType.RegisterContainer)
				.BuildCollectionProperty(MetadataType.AssemblyContainer)
				.BuildCollectionProperty(MetadataType.ReportContainer);

			return result;
		}

		public static dynamic BuildDocument(string name, string caption, string description, string indexName, string version)
		{
		    dynamic document = new DynamicWrapper();

		    document.Id = Guid.NewGuid().ToString();
		    document.Name = name;
		    document.Caption =caption;
		    document.Description = description;
		    document.Versioning = 2;
		    document.Version = version;
            document.MetadataIndex = string.IsNullOrEmpty(indexName) ? name : indexName;
		    document.Services = new List<dynamic>();
		    document.Processes = new List<dynamic>();
		    document.Scenarios = new List<dynamic>();
		    document.Generators = new List<dynamic>();
		    document.Views = new List<dynamic>();
		    document.PrintViews = new List<dynamic>();
		    document.ValidationWarnings = new List<dynamic>();
		    document.ValidationErrors = new List<dynamic>();
            return document;
		}

		public static dynamic BuildScenario(string name, string scenarioId, ContextTypeKind contextType,
		                                    ScriptUnitType scriptUnitType)
		{
			dynamic result = new DynamicWrapper();
			result.Id = name;
			result.Name = name;
			result.ScenarioId = scenarioId;
			result.Description = name + "_GeneratedScenario";
			result.ContextType = (int) contextType;
			result.ScriptUnitType = (int) scriptUnitType;
			return result;
		}

		public static dynamic BuildProcessOneAction(string name, dynamic scenario)
		{
			dynamic result = new DynamicWrapper();
			result.Id = Guid.NewGuid().ToString();
			result.Name = name;
			result.Caption = scenario.Caption;
			result.Description = scenario.Name + "_GeneratedProcess";
			result.Type = 2;
			result.Transitions = new List<dynamic>();
			result.Transitions.Add(new DynamicWrapper());
			result.Transitions[0].Id = Guid.NewGuid().ToString();
			result.Transitions[0].Name = "GeneratedTransition";
			result.Transitions[0].ActionPoint = new DynamicWrapper();
			result.Transitions[0].ActionPoint.TypeName = "Action";
			result.Transitions[0].ActionPoint.ScenarioId = scenario.Id;
			return result;
		}

		public static dynamic BuildServiceApplyJson(string actionName, dynamic process, ContextTypeKind contextTypeKind)
		{
			dynamic result = new DynamicWrapper();
			result.Id = Guid.NewGuid().ToString();
			result.Name = string.IsNullOrEmpty(actionName) ? process.Name : actionName;
			result.Caption = process.Caption;
			result.Description = process.Name + "_GeneratedService";
			result.Type = new DynamicWrapper();
			result.Type.Name = "applyjson";
			result.ExtensionPoints = new List<dynamic>();
			result.ExtensionPoints.Add(new DynamicWrapper());
			result.ExtensionPoints[0].TypeName = new DynamicWrapper();
			result.ExtensionPoints[0].TypeName.Name = "move";
			result.ExtensionPoints[0].TypeName.ContextType = (int) contextTypeKind;
			result.ExtensionPoints[0].ScenarioId = process.Name;
			return result;
		}

		public static dynamic BuildGenerator(string generatorName, string service, string actionUnit, string metadataType)
		{
			dynamic instance = new DynamicWrapper();
			instance.Id = Guid.NewGuid().ToString();
			instance.Name = generatorName;
			instance.Service = service;
			instance.MetadataType = metadataType;
			instance.ActionUnit = actionUnit;
			return instance;
		}


		#region Common
		public static object BuildModel(this object buildObject, Action<object> modelAction)
		{
			return buildObject.BuildContainer("Model", c =>
				                                           {
					                                           if (modelAction != null)
					                                           {
						                                           modelAction(c);
					                                           }
				                                           });
		}

		#endregion

		public static object BuildClassifier(this object buildObject, Action<object> classifierAction)
		{
			return buildObject.BuildCollectionItem(MetadataType.ClassifierContainer, classifierAction);
		}



		#endregion


		#region Создание заголовков метаданных для ссылок

		public static dynamic BuildMetadataHeader(dynamic buildObject)
		{
			dynamic headerInstance = new DynamicWrapper();
			headerInstance.Id = buildObject.Id;
			headerInstance.Name = buildObject.Name;
			headerInstance.Caption = buildObject.Caption;
			headerInstance.Description = buildObject.Description;
		    headerInstance.Version = buildObject.Version;
			if (buildObject.IsAutogenerated != null)
			{
				headerInstance.IsAutogenerated = buildObject.IsAutogenerated;
			}
			return headerInstance;
		}

		#endregion

		

	}
}

using System;
using InfinniPlatform.Api.Dynamic;

namespace InfinniConfiguration.SystemConfig.Metadata
{
    public static class MetadataBuilder
    {

        private static void InvokeDynamicOperation(this object buildObject, Action<dynamic> dynamicObjectAction)
        {
            if (dynamicObjectAction != null)
            {
                dynamicObjectAction((dynamic)buildObject);
            }
        }

        private static void InvokeAction(this object buildObject, Action<object> initObjectAction)
        {
            if (initObjectAction != null)
            {
                initObjectAction((dynamic)buildObject);
            }
        }


        #region Common

        public static object BuildText(this object buildObject, string text)
        {
            buildObject.InvokeDynamicOperation(dynamicObject =>
            {
                dynamicObject.Text = text;
            });
            return buildObject;
        }

        public static object BuildProperty(this object buildObject, string name, string value)
        {
            buildObject.InvokeDynamicOperation(dynamicObject => dynamicObject.AddProperty(name,value));
            return buildObject;
        }
        
        private static object BuildItems(this object buildObject, Action<dynamic> buildItemsAction)
        {
            var itemsObject = (dynamic)buildObject;
            if (itemsObject != null)
            {
                var items = itemsObject.Items;
                if (items == null)
                {
                    itemsObject.Items = new DynamicInstance();
                }
                InvokeAction(itemsObject.Items, buildItemsAction);
            }
            return itemsObject;
        }


        private static object BuildItemsList(this object buildObject, Action<dynamic> buildItemsListAction)
        {
            var itemsObject = (dynamic)buildObject;
            if (itemsObject != null)
            {
                var items = itemsObject.Items;
                if (items == null)
                {
                    itemsObject.Items = DynamicInstanceExtensions.CreateArrayInstance();
                }
                InvokeAction(itemsObject.Items, buildItemsListAction);
            }
            return itemsObject;
        }

        public static object BuildScripts(this object buildObject, Action<dynamic> scriptsAction)
        {
            var itemsObject = (dynamic)buildObject;
            if (itemsObject != null)
            {
                var scripts = itemsObject.Scripts;
                if (scripts == null)
                {
                    itemsObject.Scripts = DynamicInstanceExtensions.CreateArrayInstance();
                }
                InvokeAction(itemsObject.Scripts, scriptsAction);
            }
            return itemsObject;
        }


        #endregion

        #region Создание Layout
        public static object BuildStackLayoutPanel(this object buildObject, string panelName, Action<object> layoutPanelAction)
        {
            buildObject.InvokeDynamicOperation(dynamicObject =>
            {
                dynamicObject.LayoutPanel = new DynamicInstance();
                dynamicObject.LayoutPanel.StackPanel = new DynamicInstance();
                dynamicObject.LayoutPanel.StackPanel.Name = panelName;
                InvokeAction(dynamicObject.LayoutPanel.StackPanel, layoutPanelAction);
            });
            return buildObject;
        }


		public static object BuildTabPanel(this object buildObject, string panelName, Action<object> layoutPanelAction)
		{
			buildObject.InvokeDynamicOperation(dynamicObject =>
			{
				dynamicObject.LayoutPanel = new DynamicInstance();
				dynamicObject.LayoutPanel.TabPanel = new DynamicInstance();
				dynamicObject.LayoutPanel.TabPanel.Name = panelName;
				InvokeAction(dynamicObject.LayoutPanel.TabPanel, layoutPanelAction);
			});
			return buildObject;
		}

		private static object BuildTabPages(this object buildObject, Action<dynamic> tabPagesAction)
		{
			buildObject.InvokeDynamicOperation(dynamicObject =>
			{
				if (dynamicObject.Pages == null)
				{
					dynamicObject.Pages = DynamicInstanceExtensions.CreateArrayInstance();
				}
				InvokeAction(dynamicObject.Pages, tabPagesAction);
			});
			return buildObject;
		}

		public static object BuildTabPage(this object buildObject, string text, Action<object> tabPageAction)
		{
			buildObject.BuildTabPages(dynamicObject =>
				                          {
					                          dynamic page = new DynamicInstance();
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
                dynamic searchPanel = new DynamicInstance();
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
                    dynamicObject.DataGrid = new DynamicInstance();
                    dynamicObject.DataGrid.Name = name;
                    InvokeAction(dynamicObject.DataGrid, dataGridAction);
                });
            return buildObject;
        }

        private static object BuildDataGridColumns(this object buildObject, Action<dynamic> columnsAction)
        {
            var itemsObject = (dynamic)buildObject;
            if (itemsObject != null)
            {
                var items = itemsObject.Columns;
                if (items == null)
                {
                    itemsObject.Columns = DynamicInstanceExtensions.CreateArrayInstance();
                }
                InvokeAction(itemsObject.Columns, columnsAction);
            }
            return itemsObject;
        }


        public static object BuildDataGridColumn(this object buildObject, Action<object> columnAction)
        {
            buildObject.BuildDataGridColumns(dynamicObject =>
                {
                    var gridColumn = new DynamicInstance();
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


        #region toolbar
        public static object BuildToolBar(this object buildObject, string name, Action<object> toolbarAction)
        {
            buildObject.InvokeDynamicOperation(dynamicObject =>
            {
                dynamic toolbar = new DynamicInstance();
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
                dynamic button = new DynamicInstance();
                dynamicObject.Add(button);
                button.Button = new DynamicInstance();
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
                    dynamicObject.Action = new DynamicInstance();
                    dynamicObject.Action.AddAction = new DynamicInstance();
                    dynamicObject.Action.AddAction.View = new DynamicInstance();
                    dynamicObject.Action.AddAction.View.DefaultEditView = new DynamicInstance();
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
                dynamicObject.Action = new DynamicInstance();
                dynamicObject.Action.EditAction = new DynamicInstance();
                dynamicObject.Action.EditAction.View = new DynamicInstance();
                dynamicObject.Action.EditAction.View.DefaultEditView = new DynamicInstance();
                BuildDataSourceReference(dynamicObject.Action.EditAction.View.DefaultEditView, actionDefaultEditFormDataSource);
                dynamicObject.Action.EditAction.View.DefaultEditView.Value = new DynamicInstance();
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
                dynamicObject.Action = new DynamicInstance();
                dynamicObject.Action.DeleteAction = new DynamicInstance();
                Action<object> action = db => db.BuildPropertyBinding(dataSourceName);
                BuildDataBinding(dynamicObject.Action.DeleteAction, action);
            });
            return buildObject;
        }


        public static object BuildButtonSeparator(this object buildObject)
        {
            buildObject.InvokeDynamicOperation(dynamicObject =>
            {
                dynamicObject.Separator = new DynamicInstance();
                dynamicObject.Separator.Name = "Separator1";
            });
            return buildObject;
        }

        public static object BuildButtonActionSave(this object buildObject, string dataSourceName)
        {
            buildObject.InvokeDynamicOperation(dynamicObject =>
            {
                dynamicObject.Action = new DynamicInstance();
                dynamicObject.Action.SaveAction = new DynamicInstance();
                Action<object> action = db => db.BuildPropertyBinding(dataSourceName);
                BuildDataBinding(dynamicObject.Action.SaveAction, action);
            });
            return buildObject;
        }

        public static object BuildButtonActionCancel(this object buildObject, string dataSourceName)
        {
            buildObject.InvokeDynamicOperation(dynamicObject =>
            {
                dynamicObject.Action = new DynamicInstance();
                dynamicObject.Action.CancelAction = new DynamicInstance();
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
                dynamic propertyGrid = new DynamicInstance();
                propertyGrid.Name = name;
                dynamicObject.PropertyGrid = propertyGrid;
                InvokeAction(propertyGrid, propertyGridAction);
            });
            return buildObject;
        }

        private static object BuildPropertyGridCategories(this object buildObject, Action<dynamic> categoriesAction)
        {
            var itemsObject = (dynamic)buildObject;
            if (itemsObject != null)
            {
                var items = itemsObject.Categories;
                if (items == null)
                {
                    itemsObject.Categories = DynamicInstanceExtensions.CreateArrayInstance();
                }
                InvokeAction(itemsObject.Categories, categoriesAction);
            }
            return itemsObject;
        }

        private static object BuildCategoryProperties(this object buildObject, Action<dynamic> propertiesAction)
        {
            var itemsObject = (dynamic)buildObject;
            if (itemsObject != null)
            {
                var items = itemsObject.Properties;
                if (items == null)
                {
                    itemsObject.Properties = DynamicInstanceExtensions.CreateArrayInstance();
                }
                InvokeAction(itemsObject.Properties, propertiesAction);
            }
            return itemsObject;
        }

        public static object BuildPropertyGridCategory(this object buildObject, string text,
                                                       Action<object> propertyGridAction)
        {
            buildObject.BuildPropertyGridCategories(items =>
                {
                    dynamic category = new DynamicInstance();
                    category.Text = text;
                    items.Add(category);
                    InvokeAction(category, propertyGridAction);
                });
            return buildObject;
        }

        public static object BuildPropertyGridTextBox(this object buildObject, string name, Action<object> editValueBindingAction)
        {
            buildObject.BuildCategoryProperties(properties =>
            {
                dynamic editor = new DynamicInstance();
                editor.Name = name;
                properties.Add(editor);
                InvokeAction(editor, editValueBindingAction);
            });
            return buildObject;
        }


        #endregion

		#region menuControl
		public static object BuildMenuBar(this object buildObject, string name, string configId)
		{
			buildObject.InvokeDynamicOperation(dynamicObject =>
			{
				dynamicObject.MenuBar = new DynamicInstance();
				dynamicObject.MenuBar.Name = name;
				dynamicObject.MenuBar.ConfigId = configId;
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
                dynamic dynamicDataSource = new DynamicInstance();
                dynamicObject.Add(dynamicDataSource);
                InvokeAction(dynamicDataSource, initDataSource);
            });

            return buildObject;
        }

        private static object BuildDataSourceReference(this object buildObject, Action<object> dataSourceAction)
        {
            buildObject.InvokeDynamicOperation(dynamicObject =>
            {
                dynamic dynamicDataSource = new DynamicInstance();
                dynamicObject.DataSource = dynamicDataSource;
                InvokeAction(dynamicDataSource, dataSourceAction);
            });

            return buildObject;
        }

        public static object BuildDataSourcesList(this object buildObject, Action<object> dataSourcesListAction)
        {
            var o = (dynamic)buildObject;
            if (o != null)
            {
                if (o.DataSources == null)
                {
                    o.DataSources = DynamicInstanceExtensions.CreateArrayInstance();
                }
                if (dataSourcesListAction != null)
                {
                    InvokeAction(o.DataSources, dataSourcesListAction);
                }

            }

            return o;
        }


        public static object BuildClassifierDataSource(this object buildObject, string name, string configId, string classifierMetadataId)
        {
            buildObject.InvokeDynamicOperation(dynamicObject =>
            {
				dynamicObject.ClassifierDataSource = new DynamicInstance();

				dynamicObject.ClassifierDataSource.Name = name;
				dynamicObject.ClassifierDataSource.ConfigId = configId;
				dynamicObject.ClassifierDataSource.ClassifierMetadataId = classifierMetadataId;
            });

            return buildObject;
        }

        public static object BuildMetadataDataSource(this object buildObject, string name, string metadataType)
        {
            buildObject.InvokeDynamicOperation(dynamicObject =>
            {
				dynamicObject.MetadataDataSource = new DynamicInstance();
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
            buildObject.BuildItems(items =>
            {
                dynamic propertyBindingElement = new DynamicInstance();
                items.PropertyBinding = propertyBindingElement;
                InvokeAction(propertyBindingElement, dataBindingAction);
            });
            return buildObject;
        }

        public static object BuildEditValueBinding(this object buildObject, Action<object> editValueBindingAction)
        {
            buildObject.InvokeDynamicOperation(dynamicObject =>
            {
                dynamic value = new DynamicInstance();
                dynamicObject.Value = value;
                dynamicObject.Value.PropertyBinding = new DynamicInstance();
                InvokeAction(dynamicObject.Value.PropertyBinding, editValueBindingAction);
            });
            return buildObject;
        }
        #endregion

        #region Создание элементов Layout
        public static object BuildDataElement(this object buildObject, Action<object> dataElementAction)
        {
            buildObject.BuildItemsList(items =>
                {
                    dynamic dataElement = new DynamicInstance();
                    items.Add(dataElement);
                    InvokeAction(dataElement, dataElementAction);
                });
            return buildObject;
        }

		public static object BuildActionElement(this object buildObject, Action<object> actionElementAction)
		{
			buildObject.BuildItemsList(items =>
			{
				dynamic actionElement = new DynamicInstance();
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
                dynamic scriptElement = new DynamicInstance();
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
                    dynamicObject.OnValueSelected = new DynamicInstance();
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
                    dynamic menuItem = new DynamicInstance();
	                menuItem.MenuItem = new DynamicInstance();
                    dynamicObject.Add(menuItem);
                    menuItem.MenuItem.Text = itemText;
                    InvokeAction(menuItem.MenuItem,menuItemAction);
            });
            return buildObject;            
        }

        public static object BuildOpenViewAction(this object buildObject, Action<object> openViewAction)
        {
            buildObject.InvokeDynamicOperation(dynamicObject =>
            {
                dynamic action = new DynamicInstance();
	            action.OpenViewAction = new DynamicInstance();
	            action.OpenViewAction.View = new DynamicInstance();
                dynamicObject.Action = action;               
                InvokeAction(action.OpenViewAction.View, openViewAction);
            });
            return buildObject;    
        }

        public static object BuildSelectListView(this object buildObject, Action<object> selectListViewAction)
        {
            buildObject.InvokeDynamicOperation(dynamicObject =>
            {
                dynamic selectListView = new DynamicInstance();
                dynamicObject.SelectListView = selectListView;
                InvokeAction(selectListView, selectListViewAction);
            });
            return buildObject;
        }


        #endregion



    }
}

using System.Windows;
using DevExpress.Xpf.Core;
using InfinniPlatform.UserInterface.ViewBuilders;
using InfinniPlatform.UserInterface.ViewBuilders.ActionElements.Buttons;
using InfinniPlatform.UserInterface.ViewBuilders.ActionElements.ContextMenu;
using InfinniPlatform.UserInterface.ViewBuilders.ActionElements.MenuBar;
using InfinniPlatform.UserInterface.ViewBuilders.ActionElements.ToolBar;
using InfinniPlatform.UserInterface.ViewBuilders.Actions;
using InfinniPlatform.UserInterface.ViewBuilders.Data.DataBindings;
using InfinniPlatform.UserInterface.ViewBuilders.Data.DataSources;
using InfinniPlatform.UserInterface.ViewBuilders.DataElements.CheckBox;
using InfinniPlatform.UserInterface.ViewBuilders.DataElements.CodeEditor;
using InfinniPlatform.UserInterface.ViewBuilders.DataElements.DataNavigation;
using InfinniPlatform.UserInterface.ViewBuilders.DataElements.FindAndReplace;
using InfinniPlatform.UserInterface.ViewBuilders.DataElements.Label;
using InfinniPlatform.UserInterface.ViewBuilders.DataElements.TextBox;
using InfinniPlatform.UserInterface.ViewBuilders.DataElements.ToggleButton;
using InfinniPlatform.UserInterface.ViewBuilders.DataElements.TreeView;
using InfinniPlatform.UserInterface.ViewBuilders.Designers.ConfigDeployDesigner;
using InfinniPlatform.UserInterface.ViewBuilders.Designers.ConfigDesigner;
using InfinniPlatform.UserInterface.ViewBuilders.Designers.ConfigSelector;
using InfinniPlatform.UserInterface.ViewBuilders.Designers.ConfigTree.Elements;
using InfinniPlatform.UserInterface.ViewBuilders.Designers.ConfigVerifyDesigner;
using InfinniPlatform.UserInterface.ViewBuilders.Designers.DeployDesigner;
using InfinniPlatform.UserInterface.ViewBuilders.Designers.DocumentDesigner;
using InfinniPlatform.UserInterface.ViewBuilders.Designers.DocumentSchemaDesigner;
using InfinniPlatform.UserInterface.ViewBuilders.Designers.DocumentSelector;
using InfinniPlatform.UserInterface.ViewBuilders.Designers.GeneratorDesigner;
using InfinniPlatform.UserInterface.ViewBuilders.Designers.MenuDesigner;
using InfinniPlatform.UserInterface.ViewBuilders.Designers.PrintViewDesigner;
using InfinniPlatform.UserInterface.ViewBuilders.Designers.ProcessDesigner;
using InfinniPlatform.UserInterface.ViewBuilders.Designers.RegisterDesigner;
using InfinniPlatform.UserInterface.ViewBuilders.Designers.ReportDesigner;
using InfinniPlatform.UserInterface.ViewBuilders.Designers.ScenarioDesigner;
using InfinniPlatform.UserInterface.ViewBuilders.Designers.ServiceDesigner;
using InfinniPlatform.UserInterface.ViewBuilders.Designers.StatusDesigner;
using InfinniPlatform.UserInterface.ViewBuilders.Designers.ValidationErrorsDesigner;
using InfinniPlatform.UserInterface.ViewBuilders.Designers.ValidationWarningDesigner;
using InfinniPlatform.UserInterface.ViewBuilders.Designers.ViewDesigner;
using InfinniPlatform.UserInterface.ViewBuilders.Designers.ViewPropertyEditor;
using InfinniPlatform.UserInterface.ViewBuilders.DisplayFormats;
using InfinniPlatform.UserInterface.ViewBuilders.LayoutPanels.DockTabPanel;
using InfinniPlatform.UserInterface.ViewBuilders.LayoutPanels.GridPanel;
using InfinniPlatform.UserInterface.ViewBuilders.LayoutPanels.Panel;
using InfinniPlatform.UserInterface.ViewBuilders.LayoutPanels.ScrollPanel;
using InfinniPlatform.UserInterface.ViewBuilders.LayoutPanels.StackPanel;
using InfinniPlatform.UserInterface.ViewBuilders.LayoutPanels.TabPanel;
using InfinniPlatform.UserInterface.ViewBuilders.LayoutPanels.ViewPanel;
using InfinniPlatform.UserInterface.ViewBuilders.LinkViews;
using InfinniPlatform.UserInterface.ViewBuilders.LinkViews.ExistsView;
using InfinniPlatform.UserInterface.ViewBuilders.Parameter;
using InfinniPlatform.UserInterface.ViewBuilders.Scripts;
using InfinniPlatform.UserInterface.ViewBuilders.Views;

namespace InfinniPlatform.UserInterface.AppHost
{
    internal static class AppRunner
    {
        public const string ApplicationThemeName = "MetropolisLight";
        
		public static void Run(dynamic appViewMetadata)
        {
            ThemeManager.ApplicationThemeName = ApplicationThemeName;

            var application = new Application();

            //application.DispatcherUnhandledException += (s, e) =>
            //{
            //    MessageBox.Show(e.Exception.ToString());
            //    e.Handled = true;
            //};

            application.Startup += (s, e) => OpenAppView(appViewMetadata);

            application.Run();
        }

        private static void OpenAppView(dynamic appViewMetadata)
        {
            // Создание контекста для построения представления
            var context = CreateBuilderContext();

            // Построение главного представления приложения
            context.AppView = context.Build(null, "View", appViewMetadata);
            
            // Открытие главного представления приложения
            var linkView = new LinkView(context.AppView, null, () => context.AppView);
            linkView.SetOpenMode(OpenMode.Dialog);

            var view = linkView.CreateView();
            view.Open();
        }

        private static ObjectBuilderContext CreateBuilderContext()
        {
            var context = new ObjectBuilderContext();

            // View
            context.Register("View", new ViewBuilder());
            context.Register("Scripts", new ScriptsBuilder());
            context.Register("Parameter", new ParameterElementBuilder());

            // DisplayFormat
            context.Register("BooleanFormat", new BooleanFormatBuilder());
            context.Register("NumberFormat", new NumberFormatBuilder());
            context.Register("DateTimeFormat", new DateTimeFormatBuilder());
            context.Register("ObjectFormat", new ObjectFormatBuilder());

            // DataBinding
            context.Register("ObjectBinding", new ObjectBindingBuilder());
            context.Register("PropertyBinding", new PropertyBindingBuilder());
            context.Register("ParameterBinding", new ParameterBindingBuilder());

            // DataSources
            context.Register("ObjectDataSource", new ObjectDataSourceBuilder());
            context.Register("MetadataDataSource", new MetadataDataSourceBuilder());

            // Actions
            context.Register("AddAction", new AddActionBuilder());
            context.Register("EditAction", new EditActionBuilder());
            context.Register("DeleteAction", new DeleteActionBuilder());
            context.Register("SaveAction", new SaveActionBuilder());
            context.Register("UpdateAction", new UpdateActionBuilder());
            context.Register("AddItemAction", new AddItemActionBuilder());
            context.Register("EditItemAction", new EditItemActionBuilder());
            context.Register("DeleteItemAction", new DeleteItemActionBuilder());
            context.Register("SaveItemAction", new SaveItemActionBuilder());
            context.Register("OpenViewAction", new OpenViewActionBuilder());
            context.Register("CancelAction", new CancelActionBuilder());

            // LinkViews
            context.Register("ExistsView", new ExistsViewBuilder());

            // LayoutPanels
            context.Register("Panel", new PanelElementBuilder());
            context.Register("GridPanel", new GridPanelElementBuilder());
            context.Register("StackPanel", new StackPanelElementBuilder());
            context.Register("ScrollPanel", new ScrollPanelElementBuilder());
            context.Register("TabPage", new TabPageElementBuilder());
            context.Register("TabPanel", new TabPanelElementBuilder());
            context.Register("DockTabPage", new DockTabPageElementBuilder());
            context.Register("DockTabPanel", new DockTabPanelElementBuilder());
            context.Register("ViewPanel", new ViewPanelElementBuilder());

            // ActionElements
            context.Register("MenuBar", new MenuBarElementBuilder());
            context.Register("ToolBar", new ToolBarElementBuilder());
            context.Register("ToolBarButton", new ToolBarButtonItemBuilder());
            context.Register("ToolBarPopupButton", new ToolBarPopupButtonItemBuilder());
            context.Register("ToolBarSeparator", new ToolBarSeparatorItemBuilder());
            context.Register("Button", new ButtonElementBuilder());
            context.Register("PopupButton", new PopupButtonElementBuilder());
            context.Register("ContextMenu", new ContextMenuElementBuilder());
            context.Register("ContextMenuItem", new ContextMenuItemBuilder());
            context.Register("ContextMenuItemSeparator", new ContextMenuItemSeparatorBuilder());

            // DataElements
            context.Register("CodeEditor", new CodeEditorElementBuilder());
            context.Register("DataNavigation", new DataNavigationElementBuilder());
            context.Register("TreeView", new TreeViewElementBuilder());
            context.Register("Label", new LabelElementBuilder());
            context.Register("TextBox", new TextBoxElementBuilder());
            context.Register("CheckBox", new CheckBoxElementBuilder());
            context.Register("ToggleButton", new ToggleButtonElementBuilder());

            // Dialogs
            context.Register("FindAndReplace", new FindAndReplaceElementBuilder());

            // Designers
            context.Register("DeployDesigner", new DeployDesignerElementBuilder());
            context.Register("ConfigDesigner", new ConfigDesignerElementBuilder());
            context.Register("ConfigVerifyDesigner", new ConfigVerifyDesignerElementBuilder());
            context.Register("ConfigDeployDesigner", new ConfigDeployDesignerElementBuilder());
            context.Register("ConfigSelector", new ConfigSelectorElementBuilder());
            context.Register("MenuDesigner", new MenuDesignerElementBuilder());
            context.Register("DocumentDesigner", new DocumentDesignerElementBuilder());
            context.Register("DocumentSchemaDesigner", new DocumentSchemaDesignerElementBuilder());
            context.Register("DocumentSelector", new DocumentSelectorElementBuilder());
            context.Register("PrintViewDesigner", new PrintViewDesignerElementBuilder());
            context.Register("ReportDesigner", new ReportDesignerElementBuilder());
            context.Register("GeneratorDesigner", new GeneratorDesignerElementBuilder());
            context.Register("RegisterDesigner", new RegisterDesignerElementBuilder());
            context.Register("ProcessDesigner", new ProcessDesignerElementBuilder());
            context.Register("ScenarioDesigner", new ScenarioDesignerElementBuilder());
            context.Register("ServiceDesigner", new ServiceDesignerElementBuilder());
            context.Register("ViewDesigner", new ViewDesignerElementBuilder());
            context.Register("ViewPropertyEditor", new ViewPropertyEditorElementBuilder());
            context.Register("ValidationWarningDesigner", new ValidationWarningDesignerElementBuilder());
            context.Register("ValidationErrorDesigner", new ValidationErrorDesignerElementBuilder());
            context.Register("StatusDesigner", new StatusDesignerElementBuilder());
            context.Register("ConfigEditor", new ConfigEditorElementBuilder());

            return context;
        }
    }
}
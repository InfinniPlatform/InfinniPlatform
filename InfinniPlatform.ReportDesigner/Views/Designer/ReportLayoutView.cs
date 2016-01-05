using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Windows.Forms;
using FastReport;
using FastReport.Data;
using FastReport.Design.StandardDesigner;
using FastReport.Design.ToolWindows;
using FastReport.DevComponents.DotNetBar;
using FastReport.Table;
using FastReport.Utils;

using InfinniPlatform.Core.Schema;
using InfinniPlatform.FastReport.Templates.Data;
using InfinniPlatform.ReportDesigner.Properties;
using InfinniPlatform.ReportDesigner.Views.Bands;
using ParameterInfo = InfinniPlatform.FastReport.Templates.Data.ParameterInfo;

namespace InfinniPlatform.ReportDesigner.Views.Designer
{
    /// <summary>
    ///     Представление для отображения и редактирования содержимого отчета.
    /// </summary>
    sealed partial class ReportLayoutView : UserControl
    {
        private static readonly Type[] SupportedObjects
            =
        {
            typeof (TextObject),
            typeof (TableObject)
        };

        private static readonly Dictionary<SchemaDataType, Type> DataTypes
            = new Dictionary<SchemaDataType, Type>
            {
                {SchemaDataType.String, typeof (string)},
                {SchemaDataType.Float, typeof (double)},
                {SchemaDataType.Integer, typeof (int)},
                {SchemaDataType.Boolean, typeof (bool)},
                {SchemaDataType.DateTime, typeof (DateTime)}
            };

        private static readonly Dictionary<TotalFunc, TotalType> TotalTypes
            = new Dictionary<TotalFunc, TotalType>
            {
                {TotalFunc.Sum, TotalType.Sum},
                {TotalFunc.Min, TotalType.Min},
                {TotalFunc.Max, TotalType.Max},
                {TotalFunc.Avg, TotalType.Avg},
                {TotalFunc.Count, TotalType.Count}
            };

        // REPORT EDITOR

        private ReportBandConfigView _bandConfigView;
        private DictionaryWindow _dataPanel;
        private BaseItem _insertTableButton;
        private BaseItem _insertTextButton;
        private Report _report;
        private DesignerControl _reportEditor;

        public ReportLayoutView()
        {
            InitializeComponent();
            CreateReportEditor();
            SetAllowedActions();
        }

        /// <summary>
        ///     Шаблон отчета FastReport.
        /// </summary>
        [Browsable(false)]
        [DefaultValue(null)]
        [EditorBrowsable(EditorBrowsableState.Never)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public Report Report
        {
            get { return _report; }
            set
            {
                if (ReferenceEquals(_report, value) == false)
                {
                    _reportEditor.Report = value;
                    _report = value;

                    PrepareReportEditor();
                    SetAllowedActions();
                }
            }
        }

        /// <summary>
        ///     Панель данных отчета.
        /// </summary>
        public Control DataPanel
        {
            get
            {
                if (_dataPanel != null)
                {
                    return _dataPanel.Control;
                }

                return null;
            }
            set
            {
                if (_dataPanel != null)
                {
                    _dataPanel.Control = value;
                }
            }
        }

        /// <summary>
        ///     Отчет был изменен.
        /// </summary>
        public bool IsModified
        {
            get { return _reportEditor.Modified; }
            set { _reportEditor.Modified = value; }
        }

        /// <summary>
        ///     Событие изменения отчета.
        /// </summary>
        public event EventHandler ReportModified;

        private void OnReportModified(object sender, EventArgs e)
        {
            var handler = ReportModified;

            if (handler != null && IsModified)
            {
                handler(sender, e);
            }
        }

        private void CreateReportEditor()
        {
            DeleteDesignerConfiguration();
            ResetDesignerSettings();
            DisableNotSupportedObjects();

            _reportEditor = new DesignerControl();

            MainPanel.Controls.Add(_reportEditor);

            _reportEditor.Dock = DockStyle.Fill;
            _reportEditor.BorderStyle = BorderStyle.None;
            _reportEditor.UIStyle = UIStyle.VisualStudio2005;
            _reportEditor.ShowMainMenu = false;
            _reportEditor.UIStateChanged += OnReportModified;

            PrepareReportEditor();

            _bandConfigView = new ReportBandConfigView();
            _bandConfigView.ReportChanged += (s, e) => RefreshReport();
        }

        private static void DeleteDesignerConfiguration()
        {
            try
            {
                File.Delete(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
                    "FastReport", "FastReport.config"));
            }
            catch
            {
            }
        }

        private static void ResetDesignerSettings()
        {
            Config.DesignerSettings.Restrictions.DontChangeReportOptions = true;
            Config.DesignerSettings.Restrictions.DontCreateData = true;
            Config.DesignerSettings.Restrictions.DontCreatePage = true;
            Config.DesignerSettings.Restrictions.DontCreateReport = true;
            Config.DesignerSettings.Restrictions.DontDeletePage = true;
            Config.DesignerSettings.Restrictions.DontEditCode = true;
            Config.DesignerSettings.Restrictions.DontEditData = true;
            Config.DesignerSettings.Restrictions.DontLoadReport = true;
            Config.DesignerSettings.Restrictions.DontPreviewReport = true;
            Config.DesignerSettings.Restrictions.DontSaveReport = true;
            Config.DesignerSettings.Restrictions.DontShowRecentFiles = true;
        }

        private void DisableNotSupportedObjects()
        {
            Config.DesignerSettings.ObjectInserted
                += (s, e) =>
                {
                    if (e.Object != null)
                    {
                        var objectType = e.Object.GetType();

                        if (SupportedObjects.Contains(objectType) == false)
                        {
                            Resources.ObjectIsNotSupported.ShowError(objectType.Name);
                            SendKeys.Send("{ESC}");
                        }
                    }
                };
        }

        private void PrepareReportEditor()
        {
            // Скрытие неподдерживаемых элементов

            var insertOperations = RegisteredObjects.FindObject(typeof (ReportPage));

            if (insertOperations != null)
            {
                foreach (var insertOperation in insertOperations.Items)
                {
                    if (insertOperation.Object == null || SupportedObjects.Contains(insertOperation.Object) == false)
                    {
                        insertOperation.Enabled = false;
                    }
                }
            }

            // Настройка элементов редактора

            PrepareControl(new Func<Control, bool>[]
            {
                DisableStandardToolBar,
                DisableStyleToolBar,
                EnableTextToolBar,
                EnableBorderToolBar,
                EnableLayoutToolBar,
                SetObjectsToolBar,
                FindDataWindow,
                FindPropertiesWindow,
                FindConfigBandsButton
            }, new List<Control>(), _reportEditor);
        }

        private static void PrepareControl(Func<Control, bool>[] handlers, ICollection<Control> controls,
            Control control)
        {
            if (controls.Contains(control) == false)
            {
                controls.Add(control);

                foreach (var handler in handlers)
                {
                    if (handler(control))
                    {
                        break;
                    }
                }

                var childControls = control.Controls.Cast<Control>().ToArray();

                foreach (var child in childControls)
                {
                    PrepareControl(handlers, controls, child);
                }
            }
        }

        private static bool DisableStandardToolBar(Control control)
        {
            if (control.GetType().Name == "StandardToolbar")
            {
                control.Visible = false;
                control.Parent = null;
                return true;
            }

            return false;
        }

        private static bool DisableStyleToolBar(Control control)
        {
            if (control.GetType().Name == "StyleToolbar")
            {
                control.Visible = false;
                control.Parent = null;
                return true;
            }

            return false;
        }

        private static bool EnableTextToolBar(Control control)
        {
            if (control.GetType().Name == "TextToolbar")
            {
                control.Visible = true;
                return true;
            }

            return false;
        }

        private static bool EnableBorderToolBar(Control control)
        {
            if (control.GetType().Name == "BorderToolbar")
            {
                control.Visible = true;
                return true;
            }

            return false;
        }

        private static bool EnableLayoutToolBar(Control control)
        {
            if (control.GetType().Name == "LayoutToolbar")
            {
                control.Visible = true;
                return true;
            }

            return false;
        }

        private bool SetObjectsToolBar(Control control)
        {
            if (control.GetType().Name == "ObjectsToolbar")
            {
                var bar = control as Bar;

                if (bar != null)
                {
                    if (bar.Items != null)
                    {
                        var barItems = bar.Items.Cast<BaseItem>().ToArray();

                        foreach (var item in barItems)
                        {
                            var itemInfo = item.Tag as ObjectInfo;

                            if (itemInfo != null)
                            {
                                if (itemInfo.Object == typeof (TextObject))
                                {
                                    _insertTextButton = item;
                                }
                                else if (itemInfo.Object == typeof (TableObject))
                                {
                                    _insertTableButton = item;
                                }
                                else
                                {
                                    bar.Items.Remove(item);
                                }
                            }
                            else if (item.SubItems.Count > 0)
                            {
                                bar.Items.Remove(item);
                            }
                        }
                    }

                    return true;
                }
            }

            return false;
        }

        private bool FindDataWindow(Control control)
        {
            if (control.Name == "DictionaryWindowBar")
            {
                var bar = control as Bar;

                if (bar != null)
                {
                    bar.CanHide = false;
                    bar.CanAutoHide = false;
                    bar.CanCustomize = false;
                    bar.CanDockTop = false;
                    bar.CanDockBottom = false;
                    bar.CanDockLeft = true;
                    bar.CanDockRight = true;
                    bar.CanReorderTabs = false;

                    foreach (DockContainerItem item in bar.Items)
                    {
                        var dataWindow = item as DictionaryWindow;

                        if (dataWindow != null)
                        {
                            _dataPanel = dataWindow;

                            return true;
                        }
                    }
                }
            }

            return false;
        }

        private static bool FindPropertiesWindow(Control control)
        {
            if (control.Name == "PropertiesWindowBar")
            {
                var bar = control as Bar;

                if (bar != null)
                {
                    bar.CanHide = false;
                    bar.CanAutoHide = false;
                    bar.CanCustomize = false;
                    bar.CanDockTop = false;
                    bar.CanDockBottom = false;
                    bar.CanDockLeft = true;
                    bar.CanDockRight = true;
                    bar.CanReorderTabs = false;

                    return true;
                }
            }

            return false;
        }

        private bool FindConfigBandsButton(Control control)
        {
            if (control.GetType().Name == "BandStructure")
            {
                var bandConfigBtn = control.Controls.OfType<Button>().FirstOrDefault();

                if (bandConfigBtn != null)
                {
                    ReplaceButtonClick(bandConfigBtn, OnConfigureBands);
                }

                return true;
            }

            return false;
        }

        private static void ReplaceButtonClick(Button button, EventHandler newHandler)
        {
            var eventsField = typeof (Button).GetProperty("Events", BindingFlags.NonPublic | BindingFlags.Instance);
            var eventClickField = typeof (Control).GetField("EventClick", BindingFlags.Static | BindingFlags.NonPublic);

            if (eventsField != null && eventClickField != null)
            {
                var events = eventsField.GetValue(button, null) as EventHandlerList;
                var eventClick = eventClickField.GetValue(button);

                if (events != null && eventClick != null)
                {
                    events.RemoveHandler(eventClick, events[eventClick]);
                }
            }

            button.Click += newHandler;
        }

        // DATA SOURCES

        public void CreateDataSource(DataSourceInfo dataSourceInfo)
        {
            if (Report != null)
            {
                UpdateDataSource(null, dataSourceInfo);
            }
        }

        public void ChangeDataSource(DataSourceInfo dataSourceInfo, DataSourceInfo newDataSourceInfo)
        {
            if (Report != null)
            {
                var dataSource = Report.Dictionary.DataSources.FindByName(dataSourceInfo.Name);
                UpdateDataSource(dataSource, newDataSourceInfo);
            }
        }

        public void UpdateDataSource(DataSourceBase dataSource, DataSourceInfo newDataSourceInfo)
        {
            var newDataSource = new BusinessObjectDataSource
            {
                Name = newDataSourceInfo.Name,
                ReferenceName = newDataSourceInfo.Name
            };
            CreateDataSourceProperties(newDataSource, newDataSourceInfo.Schema);
            UpdateCollectionItem(Report.Dictionary.DataSources, dataSource, newDataSource);
            RefreshReport();
        }

        public void DeleteDataSource(DataSourceInfo dataSourceInfo)
        {
            if (Report != null)
            {
                var dataSource = Report.Dictionary.DataSources.FindByName(dataSourceInfo.Name);
                RemoveCollectionItem(Report.Dictionary.DataSources, dataSource);
                RefreshReport();
            }
        }

        private static void CreateDataSourceProperties(Column parent, DataSchema schema)
        {
            foreach (var property in schema.Properties)
            {
                var propertyName = property.Key;
                var propertySchema = property.Value;

                Column propertyColumn;

                if (propertySchema.Type == SchemaDataType.Object)
                {
                    propertyColumn = new Column
                    {
                        Name = propertyName,
                        ReferenceName = propertyName,
                        DataType = typeof (object)
                    };

                    CreateDataSourceProperties(propertyColumn, propertySchema);
                }
                else if (propertySchema.Type == SchemaDataType.Array)
                {
                    propertyColumn = new BusinessObjectDataSource {Name = propertyName, ReferenceName = propertyName};

                    CreateDataSourceProperties(propertyColumn, propertySchema.Items);
                }
                else
                {
                    propertyColumn = new Column
                    {
                        Name = propertyName,
                        ReferenceName = propertyName,
                        DataType = DataTypes[propertySchema.Type]
                    };
                }

                parent.Columns.Add(propertyColumn);
            }
        }

        // PARAMETERS

        public void CreateParameter(ParameterInfo parameterInfo)
        {
            if (Report != null)
            {
                UpdateParameter(null, parameterInfo);
            }
        }

        public void ChangeParameter(ParameterInfo parameterInfo, ParameterInfo newParameterInfo)
        {
            if (Report != null)
            {
                var parameter = Report.Dictionary.Parameters.FindByName(parameterInfo.Name);
                UpdateParameter(parameter, newParameterInfo);
            }
        }

        public void UpdateParameter(Parameter parameter, ParameterInfo newParameterInfo)
        {
            var newParameter = new Parameter(newParameterInfo.Name) {DataType = DataTypes[newParameterInfo.Type]};
            UpdateCollectionItem(Report.Dictionary.Parameters, parameter, newParameter);
            RefreshReport();
        }

        public void DeleteParameter(ParameterInfo parameterInfo)
        {
            if (Report != null)
            {
                var parameter = Report.Dictionary.Parameters.FindByName(parameterInfo.Name);
                RemoveCollectionItem(Report.Dictionary.Parameters, parameter);
                RefreshReport();
            }
        }

        // TOTALS

        public void CreateTotal(TotalInfo totalInfo)
        {
            if (Report != null)
            {
                UpdateTotal(null, totalInfo);
            }
        }

        public void ChangeTotal(TotalInfo totalInfo, TotalInfo newTotalInfo)
        {
            if (Report != null)
            {
                var total = Report.Dictionary.Totals.FindByName(totalInfo.Name);
                UpdateTotal(total, newTotalInfo);
            }
        }

        public void UpdateTotal(Total total, TotalInfo newTotalInfo)
        {
            var newTotal = new Total
            {
                Name = newTotalInfo.Name,
                Evaluator =
                    Report.AllObjects.Cast<object>()
                        .OfType<DataBand>()
                        .FirstOrDefault(i => i.Name == newTotalInfo.DataBand),
                PrintOn =
                    Report.AllObjects.Cast<object>()
                        .OfType<BandBase>()
                        .FirstOrDefault(i => i.Name == newTotalInfo.PrintBand),
                TotalType = TotalTypes[newTotalInfo.TotalFunc],
                Expression = BuildExpression(newTotalInfo.Expression)
            };

            UpdateCollectionItem(Report.Dictionary.Totals, total, newTotal);
            RefreshReport();
        }

        private static string BuildExpression(IDataBind dataBind)
        {
            // Todo: Нужно заменить на функцию формирования выражения

            var totalDataBind = dataBind as ConstantBind;

            if (totalDataBind != null)
            {
                return totalDataBind.Value as string;
            }

            return null;
        }

        public void DeleteTotal(TotalInfo totalInfo)
        {
            if (Report != null)
            {
                var total = Report.Dictionary.Totals.FindByName(totalInfo.Name);
                RemoveCollectionItem(Report.Dictionary.Totals, total);
                RefreshReport();
            }
        }

        // BANDS

        public IEnumerable<DesignerDataBand> GetDataBands()
        {
            var result = new List<DesignerDataBand>();

            var page = GetReportPage();

            if (page != null)
            {
                result.AddRange(page.AllObjects.Cast<object>().OfType<DataBand>().Select(i => new DesignerDataBand(i)));
            }

            return result;
        }

        public IEnumerable<DesignerPrintBand> GetPrintBands()
        {
            var result = new List<DesignerPrintBand>();

            var page = GetReportPage();

            if (page != null)
            {
                result.AddRange(
                    page.AllObjects.Cast<object>().OfType<GroupFooterBand>().Select(i => new DesignerPrintBand(i)));

                if (page.ReportSummary != null)
                {
                    result.Add(new DesignerPrintBand(page.ReportSummary));
                }

                if (page.PageFooter != null)
                {
                    result.Add(new DesignerPrintBand(page.PageFooter));
                }
            }

            return result;
        }

        // MENU

        public event EventHandler InvokeImportReport;
        public event EventHandler InvokeExportReport;
        public event EventHandler InvokePreviewReport;
        public event EventHandler InvokeCreateDataSource;
        public event EventHandler InvokeCreateParameter;
        public event EventHandler InvokeCreateTotal;

        private void RaiseEvent(EventHandler handler)
        {
            if (handler != null)
            {
                handler(this, EventArgs.Empty);
            }
        }

        private void SetAllowedActions()
        {
            var hasOpenedReport = (Report != null);

            ImportMenuItem.Enabled = hasOpenedReport;
            ExportMenuItem.Enabled = hasOpenedReport;
            PageSetupMenuItem.Enabled = hasOpenedReport;
            PreviewMenuItem.Enabled = hasOpenedReport;

            EditMenuItem.Visible = hasOpenedReport;
            InsertMenuItem.Visible = hasOpenedReport;
            LayoutMenuItem.Visible = hasOpenedReport;
            DataMenuItem.Visible = hasOpenedReport;
        }

        // Report menu

        private void OnImportReport(object sender, EventArgs e)
        {
            RaiseEvent(InvokeImportReport);
        }

        private void OnExportReport(object sender, EventArgs e)
        {
            RaiseEvent(InvokeExportReport);
        }

        private void OnPageSetup(object sender, EventArgs e)
        {
            if (_reportEditor.cmdPageSetup.Enabled)
            {
                _reportEditor.cmdPageSetup.Invoke();
            }
        }

        private void OnPreviewReport(object sender, EventArgs e)
        {
            RaiseEvent(InvokePreviewReport);
        }

        // Edit menu

        private void OnEditMenuOpening(object sender, EventArgs e)
        {
            UndoMenuItem.Enabled = _reportEditor.cmdUndo.Enabled;
            RedoMenuItem.Enabled = _reportEditor.cmdRedo.Enabled;
            CutMenuItem.Enabled = _reportEditor.cmdCut.Enabled;
            CopyMenuItem.Enabled = _reportEditor.cmdCopy.Enabled;
            PasteMenuItem.Enabled = _reportEditor.cmdPaste.Enabled;
            DeleteMenuItem.Enabled = _reportEditor.cmdDelete.Enabled;
        }

        private void OnEditUndo(object sender, EventArgs e)
        {
            if (_reportEditor.cmdUndo.Enabled)
            {
                _reportEditor.cmdUndo.Invoke();
            }
        }

        private void OnEditRedo(object sender, EventArgs e)
        {
            if (_reportEditor.cmdRedo.Enabled)
            {
                _reportEditor.cmdRedo.Invoke();
            }
        }

        private void OnEditCut(object sender, EventArgs e)
        {
            if (_reportEditor.cmdCut.Enabled)
            {
                _reportEditor.cmdCut.Invoke();
            }
        }

        private void OnEditCopy(object sender, EventArgs e)
        {
            if (_reportEditor.cmdCopy.Enabled)
            {
                _reportEditor.cmdCopy.Invoke();
            }
        }

        private void OnEditPaste(object sender, EventArgs e)
        {
            if (_reportEditor.cmdPaste.Enabled)
            {
                _reportEditor.cmdPaste.Invoke();
            }
        }

        private void OnEditDelete(object sender, EventArgs e)
        {
            if (_reportEditor.cmdDelete.Enabled)
            {
                _reportEditor.cmdDelete.Invoke();
            }
        }

        // Insert menu

        private void OnInsertTextElement(object sender, EventArgs e)
        {
            if (_insertTextButton != null)
            {
                _insertTextButton.RaiseClick();
            }
        }

        private void OnInsertTableElement(object sender, EventArgs e)
        {
            if (_insertTableButton != null)
            {
                _insertTableButton.RaiseClick();
            }
        }

        // Layout menu

        private void OnReportMenuOpening(object sender, EventArgs e)
        {
            var page = GetReportPage();

            ReportTitleMenuItem.Checked = (page.ReportTitle != null);
            ReportSummaryMenuItem.Checked = (page.ReportSummary != null);
            PageHeaderMenuItem.Checked = (page.PageHeader != null);
            PageFooterMenuItem.Checked = (page.PageFooter != null);
        }

        private void OnSetReportTitleBand(object sender, EventArgs e)
        {
            var page = GetReportPage();
            page.ReportTitle = (page.ReportTitle == null)
                ? new ReportTitleBand {Name = "ReportTitle1", Height = Units.Centimeters}
                : null;
            RefreshReport();
        }

        private void OnSetReportSummaryBand(object sender, EventArgs e)
        {
            var page = GetReportPage();
            page.ReportSummary = (page.ReportSummary == null)
                ? new ReportSummaryBand {Name = "ReportSummary1", Height = Units.Centimeters}
                : null;
            RefreshReport();
        }

        private void OnSetReportPageHeaderBand(object sender, EventArgs e)
        {
            var page = GetReportPage();
            page.PageHeader = (page.PageHeader == null)
                ? new PageHeaderBand {Name = "PageHeader1", Height = Units.Centimeters}
                : null;
            RefreshReport();
        }

        private void OnReportPageFooterBand(object sender, EventArgs e)
        {
            var page = GetReportPage();
            page.PageFooter = (page.PageFooter == null)
                ? new PageFooterBand {Name = "PageFooter1", Height = Units.Centimeters}
                : null;
            RefreshReport();
        }

        private void OnConfigureBands(object sender, EventArgs e)
        {
            _bandConfigView.ReportPage = GetReportPage();
            _bandConfigView.ShowDialog(this);
        }

        // Data menu

        private void OnCreateDataSource(object sender, EventArgs e)
        {
            RaiseEvent(InvokeCreateDataSource);
        }

        private void OnCreateParameter(object sender, EventArgs e)
        {
            RaiseEvent(InvokeCreateParameter);
        }

        private void OnCreateTotal(object sender, EventArgs e)
        {
            RaiseEvent(InvokeCreateTotal);
        }

        // HELPERS

        private void RefreshReport()
        {
            _reportEditor.Report = Report;
        }

        private ReportPage GetReportPage()
        {
            return (Report != null) ? Report.Pages[0] as ReportPage : null;
        }

        private static void UpdateCollectionItem(FRCollectionBase collection, Base oldItem, Base newItem)
        {
            if (oldItem != null)
            {
                var index = collection.IndexOf(oldItem);
                collection.RemoveAt(index);
                collection.Insert(index, newItem);
            }
            else
            {
                collection.Add(newItem);
            }
        }

        private static void RemoveCollectionItem(FRCollectionBase collection, Base item)
        {
            if (item != null)
            {
                collection.Remove(item);
            }
        }
    }
}
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using DevExpress.XtraEditors.Controls;
using InfinniPlatform.Api.Properties;
using InfinniPlatform.Api.Validation;
using InfinniPlatform.DesignControls.Controls.Properties;
using InfinniPlatform.DesignControls.Layout;
using InfinniPlatform.DesignControls.ObjectInspector;
using InfinniPlatform.DesignControls.PropertyDesigner;
using InfinniPlatform.DesignControls.PropertyEditors;
using InfinniPlatform.Sdk.Dynamic;
using InfinniPlatform.Sdk.Environment;
using InfinniPlatform.Sdk.Environment.Validations;

namespace InfinniPlatform.DesignControls.Controls.LayoutPanels.GridPanels
{
    public partial class GridPanel : UserControl, IPropertiesProvider, ILayoutProvider, IAlignment, IInspectedItem,
        IClientHeightProvider
    {
        private const int DefaultColumnCount = 12;
        private FocusedNodeMemento _memento;
        private ObjectInspectorTree _objectInspector;

        private readonly Dictionary<string, CollectionProperty> _collectionProperties =
            new Dictionary<string, CollectionProperty>();

        private readonly List<CompositPanel> _panels = new List<CompositPanel>();
        private readonly List<PropertiesNode> _propertiesNodes = new List<PropertiesNode>();

        private readonly CollectionProperty _rows = new CollectionProperty(new Dictionary<string, IControlProperty>
        {
            {
                "Cells", new CollectionProperty(new Dictionary<string, IControlProperty>
                {
                    {"ColumnSpan", new SimpleProperty(1)}
                })
            }
        });

        private readonly Dictionary<string, IControlProperty> _simpleProperties =
            new Dictionary<string, IControlProperty>();

        public GridPanel()
        {
            InitializeComponent();

            InitProperties();
        }

        public List<PropertiesNode> PropertiesNodes
        {
            get { return _propertiesNodes; }
        }

        public List<CompositPanel> Panels
        {
            get { return _panels; }
        }

        public int RowCount { get; private set; }
        public int ColumnCount { get; private set; }

        public void AlignControls()
        {
            foreach (var panel in Panels)
            {
                panel.BringToFront();
            }

            if (Height == 0)
            {
                return;
            }

            var sizeCalculator = new SizeCalculator(ObjectInspector, Height);
            var sizes = sizeCalculator.Calculate(Panels.ToArray());

            foreach (var compositPanel in Panels)
            {
                compositPanel.Height = Convert.ToInt32(sizes[compositPanel]);
                compositPanel.AlignControls();
            }
        }

        public int GetClientHeight()
        {
            var sizeCalculator = new SizeCalculator(ObjectInspector, Height);
            sizeCalculator.Calculate(Panels.ToArray());
            return sizeCalculator.FullSize;
        }

        public bool IsFixedHeight()
        {
            return false;
        }

        public ObjectInspectorTree ObjectInspector
        {
            get { return _objectInspector; }
            set
            {
                _objectInspector = value;
                _memento = new FocusedNodeMemento(_objectInspector);
            }
        }

        public void SetLayout(dynamic value)
        {
            _memento.BeginUpdate();

            var rows = _collectionProperties["Rows"].Items;
            var gridConstructor = new GridPanelRowConstructor(rows.ToArray());
            for (var rowIndex = 0; rowIndex < rows.Count; rowIndex++)
            {
                var row = rows[rowIndex];

                for (var columnIndex = 0; columnIndex < row.Cells.Count; columnIndex++)
                {
                    var gridCellMap = gridConstructor.GetCellMap(rowIndex, columnIndex);

                    ObjectInspector.FocusedPropertiesNode =
                        PropertiesNodes[gridCellMap.InnerRowIndex].Children[gridCellMap.InnerColumnIndex];

                    Panels[gridCellMap.InnerRowIndex].GetCell(gridCellMap.InnerColumnIndex)
                        .SetLayout(row.Cells[columnIndex].Items);
                }
            }
            _memento.EndUpdate();
        }

        public dynamic GetLayout()
        {
            var instanceLayout = new DynamicWrapper();

            this.SetSimplePropertiesToInstance(instanceLayout);

            var rowIndex = 0;

            var rows = _collectionProperties["Rows"].Items.ToArray();
            var rowsContainer = new GridPanelRowConstructor(rows);

            foreach (var row in _collectionProperties["Rows"].Items)
            {
                row.Cells = DesignerExtensions.GetCollection(row, "Cells");

                var cellsList = new List<dynamic>();
                for (var i = 0; i < row.Cells.Count(); i++)
                {
                    var gridCellMap = rowsContainer.GetCellMap(rowIndex, i);

                    var controlName =
                        Panels[gridCellMap.InnerRowIndex].GetCell(gridCellMap.InnerColumnIndex).GetPropertyName();
                    var controlLayout =
                        Panels[gridCellMap.InnerRowIndex].GetCell(gridCellMap.InnerColumnIndex).GetLayout();

                    if (!String.IsNullOrEmpty(controlName) && controlLayout != null)
                    {
                        var cell = row.Cells[i];
                        cell.ColumnSpan = Convert.ToInt32(cell.ColumnSpan);
                        cell[controlName] = controlLayout;
                        cellsList.Add(cell);
                    }
                }
                row.Cells = cellsList;
                rowIndex++;
            }

            instanceLayout["Rows"] = _collectionProperties["Rows"].Items;

            return instanceLayout;
        }

        public string GetPropertyName()
        {
            return "GridPanel";
        }

        public void ApplySimpleProperties()
        {
        }

        public void ApplyCollections()
        {
            if (PropertiesNodes.Any())
            {
                if (MessageBox.Show("Attention! If proceed, all inner layout will be destroyed. Continue?",
                    Resources.NeedConfirm,
                    MessageBoxButtons.OKCancel, MessageBoxIcon.Question) == DialogResult.Cancel)
                {
                    return;
                }
            }
            Clear();
            CreateGridCells();
            AlignControls();
        }

        public Dictionary<string, IControlProperty> GetSimpleProperties()
        {
            return _simpleProperties;
        }

        public Dictionary<string, CollectionProperty> GetCollections()
        {
            return _collectionProperties;
        }

        public void LoadProperties(dynamic value)
        {
            DesignerExtensions.SetSimplePropertiesFromInstance(_simpleProperties, value);

            var rows = (value.Rows as IEnumerable);
            if (rows != null)
            {
                _collectionProperties["Rows"].Items = rows.OfType<dynamic>().ToList();
            }
        }

        public Dictionary<string, Func<IPropertyEditor>> GetPropertyEditors()
        {
            return new Dictionary<string, Func<IPropertyEditor>>()
                .InheritBaseElementPropertyEditors(ObjectInspector);
        }

        public Dictionary<string, Func<Func<string, dynamic>, ValidationResult>> GetValidationRules()
        {
            return
                new Dictionary<string, Func<Func<string, dynamic>, ValidationResult>>().InheritBaseElementValidators(
                    "GridPanel");
        }

        private void InitProperties()
        {
            _simpleProperties.Clear();
            _collectionProperties.Clear();
            _rows.Items.Clear();

            _simpleProperties.InheritBaseElementSimpleProperties();
            _simpleProperties.Add("Columns", new SimpleProperty(DefaultColumnCount));

            _collectionProperties.Add("Rows", _rows);
        }

        public void Clear()
        {
            if (ObjectInspector != null)
            {
                foreach (var propertiesNode in PropertiesNodes)
                {
                    ObjectInspector.DeleteNode(propertiesNode, true);
                }
            }
            PropertiesNodes.Clear();
            Panels.Clear();
            Controls.Clear();
        }

        public void SetGrid(int rowCount, int columnCount)
        {
            RowCount = rowCount;
            ColumnCount = columnCount;

            _rows.Items.Clear();
            for (var i = 0; i < rowCount; i++)
            {
                dynamic row = new DynamicWrapper();
                row.Cells = new List<dynamic>();
                for (var j = 0; j < columnCount; j++)
                {
                    dynamic cell = new DynamicWrapper();
                    cell.ColumnSpan = Math.Floor((decimal) 12/columnCount);
                    row.Cells.Add(cell);
                }
                _rows.Items.Add(row);
            }

            Clear();
            CreateGridCells();
            AlignControls();
        }

        private void CreateGridCells()
        {
            var rows = _collectionProperties["Rows"].Items.ToArray();

            PropertiesNodes.Clear();

            var rowsContainer = new GridPanelRowConstructor(rows);


            for (var i = 0; i < rowsContainer.GetRowCount(); i++)
            {
                Panels.Add(CreatePanel());
                //регистрируем строку грида в инспекторе объектов
                //создаем контейнер объектов
                RegisterGridPanelRowInInspector(i);
            }

            for (var i = 0; i < Panels.Count(); i++)
            {
                _memento.BeginUpdate();
                ObjectInspector.FocusedPropertiesNode = PropertiesNodes[i];

                var cells = rowsContainer.GetCells(i); //DesignerExtensions.GetCollection(rows[i], "Cells").ToList();

                //устанавливаем количество столбцов, соответствующее суммарном количеству столбцов в строке
                var columnSpan = cells.Select(s => Convert.ToInt32(s.ColumnSpan)).Cast<int>().ToList();

                //устанавливаем для строки количество столбцов
                Panels[i].SetColumnSpan(columnSpan);

                _memento.EndUpdate();
            }
        }

        private void RegisterGridPanelRowInInspector(int i)
        {
            _memento.BeginUpdate();

            var propertyNode = new PropertiesNode(Panels[i])
            {
                ControlName = ControlRepository.Instance(ObjectInspector).GetName("GridPanelRow"),
                EnabledLayoutTypes = new List<EnabledItems>(),
                //запрещено удалять ячейки строки
                OnRemoveChild = control => { return false; }
            };

            PropertiesNodes.Add(propertyNode);

            ObjectInspector.AddNode(propertyNode);

            _memento.EndUpdate();
        }

        private CompositPanel CreatePanel()
        {
            var panel = new CompositPanel();
            panel.PanelContainer.BorderStyle = BorderStyles.NoBorder;

            panel.Dock = DockStyle.Top;
            panel.ObjectInspector = ObjectInspector;
            Controls.Add(panel);
            return panel;
        }
    }
}
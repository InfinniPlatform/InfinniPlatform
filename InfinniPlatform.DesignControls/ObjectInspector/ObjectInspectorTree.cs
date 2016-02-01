using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using DevExpress.XtraBars;
using DevExpress.XtraTab;
using DevExpress.XtraTreeList;
using DevExpress.XtraTreeList.Nodes;
using InfinniPlatform.DesignControls.Controls.Alignment;
using InfinniPlatform.DesignControls.DesignerSurface;
using InfinniPlatform.DesignControls.Layout;
using InfinniPlatform.DesignControls.PropertyDesigner;
using InfinniPlatform.Sdk.Dynamic;

namespace InfinniPlatform.DesignControls.ObjectInspector
{
    public partial class ObjectInspectorTree : UserControl
    {
        private ILayoutProvider _clipboardNode;
        private ObjectInspectorPopupMenu _objectInspectorPopupMenu;
        private PropertiesNode _propertiesRootNode;

        private readonly Dictionary<PropertiesNode, TreeListNode> _nodes =
            new Dictionary<PropertiesNode, TreeListNode>();

        private readonly List<ResizerFixed> _resizers = new List<ResizerFixed>();

        public ObjectInspectorTree()
        {
            InitializeComponent();
        }

        public PropertiesNode PropertiesRootNode
        {
            get { return _propertiesRootNode; }
            set
            {
                _propertiesRootNode = value;
                ControlsTree.Nodes[0].Tag = _propertiesRootNode;
            }
        }

        public ObjectInspectorPopupMenu ObjectInspectorPopupMenu
        {
            get { return _objectInspectorPopupMenu; }
            set
            {
                _objectInspectorPopupMenu = value;
                if (_objectInspectorPopupMenu != null)
                {
                    _objectInspectorPopupMenu.AfterCreateControlHandler = AddNode;
                }
            }
        }

        public ControlRepository ControlRepository { get; set; }
        public PropertiesNode FocusedPropertiesNode { get; set; }
        public Action<Control> OnSetFocus { get; set; }
        public IEnumerable<DataSourceObject> DataSources { get; set; }
        public IEnumerable<ScriptSourceObject> Scripts { get; set; }
        public IEnumerable<ChildViewObject> ChildViews { get; set; }

        public void SelectElement(Control controlToSelect)
        {
            ControlsTree.FocusedNode = FindElement(controlToSelect, ControlsTree.Nodes);
        }

        public void RemoveAllElements()
        {
            var rootNode = ControlsTree.Nodes.Cast<TreeListNode>().FirstOrDefault(n => n.ParentNode == null);
            ((PropertiesNode) rootNode.Tag).RemoveChildren();


            rootNode.Nodes.Clear();
            _nodes.Clear();
            FocusedPropertiesNode = null;
            _resizers.Clear();
        }

        private TreeListNode FindElement(Control controlToFind, TreeListNodes nodeList)
        {
            foreach (TreeListNode treeListNode in nodeList)
            {
                var nodeSettings = (PropertiesNode) treeListNode.Tag;
                if (nodeSettings.GetControl() == controlToFind)
                {
                    return treeListNode;
                }
                var childNode = FindElement(controlToFind, treeListNode.Nodes);
                if (childNode != null)
                {
                    return childNode;
                }
            }
            return null;
        }

        private void ControlsTree_focusedNodeChanged(object sender, FocusedNodeChangedEventArgs e)
        {
            if (ControlsTree.FocusedNode != null)
            {
                if (ControlsTree.FocusedNode != ControlsTree.Nodes.FirstNode)
                {
                    var control = ((PropertiesNode) ControlsTree.FocusedNode.Tag).ControlWrapper;
                    if (control != null)
                    {
                        FocusControl(control);
                    }
                    else
                    {
                        FocusControl(((PropertiesNode) ControlsTree.FocusedNode.Tag).GetControl());
                    }
                }
                FocusedPropertiesNode = ((PropertiesNode) ControlsTree.FocusedNode.Tag);
            }
        }

        private void FocusControl(Control control)
        {
            if (control.Parent != null && !control.Parent.Focused)
            {
                FocusControl(control.Parent);
            }

            var page = control as XtraTabPage;
            if (page != null && page.TabControl != null)
            {
                page.TabControl.SelectedTabPage = page;
            }
            else
            {
                OnSetFocus(control);
            }
        }

        private void ControlsTree_mouseUp(object sender, MouseEventArgs e)
        {
            var tree = sender as TreeList;

            if (e.Button == MouseButtons.Right && ModifierKeys == Keys.None && tree.State == TreeListState.Regular)
            {
                var pt = tree.PointToClient(MousePosition);

                var info = tree.CalcHitInfo(pt);

                if (info.HitInfoType == HitInfoType.Cell)
                {
                    tree.FocusedNode = info.Node;
                    FocusedPropertiesNode = (PropertiesNode) tree.FocusedNode.Tag;

                    ShowPopup(MousePosition);
                }
            }
        }

        public void ShowPopup(Point coordinates)
        {
            if (ControlsTree.FocusedNode == null)
            {
                return;
            }


            _objectInspectorPopupMenu.Prepare((PropertiesNode) ControlsTree.FocusedNode.Tag);
            _objectInspectorPopupMenu.ShowPopup(coordinates);
        }

        private void ControlsTree_Click(object sender, EventArgs e)
        {
            _objectInspectorPopupMenu.ClosePopup();
        }

        private void SettingsButton_ItemClick(object sender, ItemClickEventArgs e)
        {
            if (ControlsTree.FocusedNode == null)
            {
                MessageBox.Show("NoItemSelected", "NeedToSelectItem", MessageBoxButtons.OK,
                    MessageBoxIcon.Information);
                return;
            }

            var showingNode = (PropertiesNode) ControlsTree.FocusedNode.Tag;
            var controlFocused = showingNode.GetControl();
            SelectElement(controlFocused);
            ShowPropertyGrid(controlFocused, showingNode);
        }

        public void ShowPropertyGrid(object control, PropertiesNode focusedNode)
        {
            var propertiesControl = control as IPropertiesProvider;
            if (propertiesControl != null)
            {
                var form = new PropertiesForm();

                var validationRules = propertiesControl.GetValidationRules();
                form.SetValidationRules(validationRules);
                var propertyEditors = propertiesControl.GetPropertyEditors();
                form.SetPropertyEditors(propertyEditors);
                var simpleProperties = propertiesControl.GetSimpleProperties();


                form.SetSimpleProperties(simpleProperties);
                var collectionProperties = propertiesControl.GetCollections();
                form.SetCollectionProperties(collectionProperties);

                if (form.ShowDialog() == DialogResult.OK)
                {
                    propertiesControl.ApplySimpleProperties();
                    propertiesControl.ApplyCollections();

                    RenameNode(focusedNode, simpleProperties["Name"].Value.ToString());
                }
                else
                {
                    form.RevertChanges();
                }
            }
        }

        private void RenameNode(PropertiesNode propertiesNode, string newName)
        {
            var node = FindElement(propertiesNode.GetControl(), ControlsTree.Nodes);
            propertiesNode.ControlName = newName;
            node.SetValue("ControlName", newName);
        }

        private void ButtonCopy_ItemClick(object sender, ItemClickEventArgs e)
        {
            if (ControlsTree.FocusedNode == null)
            {
                MessageBox.Show("NoItemSelected", "NeedToSelectItem", MessageBoxButtons.OK,
                    MessageBoxIcon.Information);
                return;
            }

            var focusedPropertiesNode = ((PropertiesNode) ControlsTree.FocusedNode.Tag);
            MoveToClipboard(focusedPropertiesNode, false);
        }

        private void ButtonCut_ItemClick(object sender, ItemClickEventArgs e)
        {
            if (ControlsTree.FocusedNode == null)
            {
                MessageBox.Show("NoItemSelected", "NeedToSelectItem", MessageBoxButtons.OK,
                    MessageBoxIcon.Information);
                return;
            }

            var focusedPropertiesNode = ((PropertiesNode) ControlsTree.FocusedNode.Tag);

            if (MessageBox.Show(string.Format("ConfirmDeleteDocument", focusedPropertiesNode.ControlName),
                "NeedConfirm", MessageBoxButtons.OKCancel, MessageBoxIcon.Question,
                MessageBoxDefaultButton.Button2) ==
                DialogResult.Cancel)
            {
                return;
            }

            MoveToClipboard(focusedPropertiesNode, true);
        }

        private void MoveToClipboard(PropertiesNode focusedPropertiesNode, bool delete)
        {
            var layoutProvider = (focusedPropertiesNode.ControlWrapper as ILayoutProvider);
            if (layoutProvider == null || (focusedPropertiesNode.OnCopy != null && !focusedPropertiesNode.OnCopy()))
            {
                MessageBox.Show(
                    string.Format("Can't make clipboard operation on element \"{0}\"", focusedPropertiesNode.ControlName),
                    "Can't make operation", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }


            var focusedPropertiesNodeParent = ((PropertiesNode) ControlsTree.FocusedNode.Tag);

            if (delete)
            {
                DeleteNode(focusedPropertiesNodeParent);
            }

            _clipboardNode = layoutProvider;

            ButtonPaste.Enabled = true;
        }

        private void ButtonPaste_ItemClick(object sender, ItemClickEventArgs e)
        {
            if (ControlsTree.FocusedNode == null)
            {
                MessageBox.Show("NoItemSelected", "NeedToSelectItem", MessageBoxButtons.OK,
                    MessageBoxIcon.Information);
                return;
            }

            if (_clipboardNode == null)
            {
                MessageBox.Show("ClipboardIsEmpty", "Warnings", MessageBoxButtons.OK,
                    MessageBoxIcon.Exclamation,
                    MessageBoxDefaultButton.Button1);
                return;
            }

            var nodeToInsert = ((PropertiesNode) ControlsTree.FocusedNode.Tag);

            if (nodeToInsert.ControlWrapper != null)
            {
                dynamic item = new DynamicWrapper();
                item[_clipboardNode.GetPropertyName()] = _clipboardNode.GetLayout();

                nodeToInsert.ControlWrapper.InsertLayout(item);
            }
            else
            {
                MessageBox.Show(string.Format("Can't paste control on element \"{0}\"", nodeToInsert.ControlName),
                    "Paste operation failed", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
        }

        private void ButtonDelete_ItemClick(object sender, ItemClickEventArgs e)
        {
            if (ControlsTree.FocusedNode == null || ControlsTree.FocusedNode.ParentNode == null)
            {
                MessageBox.Show("NoItemSelected", "NeedToSelectItem", MessageBoxButtons.OK,
                    MessageBoxIcon.Information);
                return;
            }

            var focusedPropertiesNode = ((PropertiesNode) ControlsTree.FocusedNode.Tag);


            if (MessageBox.Show(string.Format("ConfirmDeleteDocument", focusedPropertiesNode.ControlName),
                "NeedConfirm", MessageBoxButtons.OKCancel, MessageBoxIcon.Question,
                MessageBoxDefaultButton.Button2) ==
                DialogResult.Cancel)
            {
                return;
            }

            DeleteNode(focusedPropertiesNode);
        }

        private TreeListNode AppendNode(PropertiesNode propertiesNode, TreeListNode parent)
        {
            var node = ControlsTree.AppendNode(new object[] {propertiesNode.ControlName}, parent);
            _nodes.Add(propertiesNode, node);
            return node;
        }

        public void AddNode(PropertiesNode childNode)
        {
            TreeListNode nodeToAdd = null;
            FocusedPropertiesNode = FocusedPropertiesNode ?? PropertiesRootNode;

            if (FocusedPropertiesNode == PropertiesRootNode)
            {
                nodeToAdd = ControlsTree.Nodes.FirstNode;
            }
            else
            {
                nodeToAdd = _nodes[FocusedPropertiesNode];
            }

            var node = AppendNode(childNode, nodeToAdd);
            childNode.ObjectInspector = this;

            var parentNode = (PropertiesNode) nodeToAdd.Tag;
            parentNode.AddChild(childNode);
            node.Tag = childNode;

            FocusControl(childNode.GetControl());
            FocusedPropertiesNode = childNode;
            if (childNode.ControlWrapper != null)
            {
                childNode.ControlWrapper.SetSimpleProperty("Name", childNode.ControlName);
            }


            ControlsTree.FocusedNode = node;
        }

        public void DeleteNode(PropertiesNode childNode, bool ignoreInspector = false)
        {
            if (ControlsTree.FocusedNode.ParentNode.Tag != null)
            {
                var node = FindElement(childNode.GetControl(), ControlsTree.Nodes);

                var focusedPropertiesNodeParent = ((PropertiesNode) node.ParentNode.Tag);

                if (!focusedPropertiesNodeParent.RemoveChild(childNode, ignoreInspector))
                {
                    return;
                }


                ControlsTree.DeleteNode(node);
            }
        }

        public void RegisterResizer(PropertiesNode propertiesNode)
        {
            var clientHeightProvider = propertiesNode.GetControl() as IClientHeightProvider;

            if (clientHeightProvider != null)
            {
                _resizers.Add(new ResizerFixed(propertiesNode.ControlWrapper));
            }
        }

        public int GetSize(Control control)
        {
            var propertiesControl = control as PropertiesControl ?? FindParentPropertiesControl(control);

            if (propertiesControl != null)
            {
                var resizerCalculator = new ResizerCalculator(propertiesControl);
                return resizerCalculator.GetSize(_resizers);
            }
            return 0;
        }

        private PropertiesControl FindParentPropertiesControl(Control control)
        {
            var parent = control.Parent;
            while (parent != null)
            {
                var propertiesControl = parent as PropertiesControl;
                if (propertiesControl != null)
                {
                    return propertiesControl;
                }
                parent = parent.Parent;
            }
            return null;
        }

        public void BeginUpdate()
        {
            ControlsTree.BeginUpdate();
        }

        public void EndUpdate()
        {
            ControlsTree.EndUpdate();
        }
    }
}
using System;
using System.Collections.Generic;
using System.Windows.Forms;
using InfinniPlatform.DesignControls.Layout;
using InfinniPlatform.DesignControls.PropertyDesigner;

namespace InfinniPlatform.DesignControls.ObjectInspector
{
    public enum EnabledItems
    {
        Data,
        Action,
        Layout
    }

    /// <summary>
    ///     Узел дерева инспектора объектов, соответствующий каждому контролу дизайнера
    /// </summary>
    public sealed class PropertiesNode
    {
        private string _controlName;
        private PropertiesControl _controlWrapper;
        private ObjectInspectorTree _objectInspector;

        public IEnumerable<EnabledItems> EnabledLayoutTypes = new List<EnabledItems>
        {
            EnabledItems.Action,
            EnabledItems.Data,
            EnabledItems.Layout
        };

        private readonly IList<PropertiesNode> _children = new List<PropertiesNode>();
        private readonly Control _control;

        public PropertiesNode(Control control)
        {
            _control = control;
        }

        /// <summary>
        ///     Обертка в виде PropertiesControl над визуальным контролом.
        /// </summary>
        public PropertiesControl ControlWrapper
        {
            get { return _controlWrapper; }
            set
            {
                _controlWrapper = value;
                _controlWrapper.SetControlCaption(ControlName);
            }
        }

        public string ControlName
        {
            get { return _controlName; }
            set
            {
                _controlName = value;
                if (_controlWrapper != null)
                {
                    _controlWrapper.SetControlCaption(_controlName);
                }
            }
        }

        public ObjectInspectorTree ObjectInspector
        {
            get { return _objectInspector; }
            set
            {
                _objectInspector = value;

                var inspectedControl = _control as IInspectedItem;
                if (inspectedControl != null)
                {
                    inspectedControl.ObjectInspector = value;
                }

                //по-хорошему, необходимо создать некий базовый класс,
                //который по цепочке устанавливает ObjectInspector у элемента, 
                //возвращаемого в результате IControlHost.GetHost()
                //Однако, не хочется создавать такой базовый класс,
                //поэтому пробрасывать ObjectInspector во внутреннюю панель, предоставляющую
                //Host, будем здесь


                var host = _control as IControlHost;

                if (host != null)
                {
                    var inspectedItem = host.GetHost() as IInspectedItem;
                    if (inspectedItem != null)
                    {
                        inspectedItem.ObjectInspector = value;
                    }
                }
            }
        }

        public Func<Control, bool> OnRemoveChild { get; set; }
        public Func<bool> OnCopy { get; set; }

        public IList<PropertiesNode> Children
        {
            get { return _children; }
        }

        public Control GetControl()
        {
            return _control;
        }

        public void RemoveChildren()
        {
            foreach (var child in Children)
            {
                RemoveChild(child, true);
            }

            Children.Clear();
        }

        public void AddChild(PropertiesNode propertiesNode)
        {
            var host = _control as IControlHost;

            if (host != null)
            {
                propertiesNode.ControlWrapper = host.GetHost().AddControl(propertiesNode.GetControl());

                _objectInspector.RegisterResizer(propertiesNode);

                host.GetHost().AlignControls();
            }


            Children.Add(propertiesNode);
        }

        public bool RemoveChild(PropertiesNode child, bool ignoreInspector)
        {
            if (OnRemoveChild != null)
            {
                var resultRemove = OnRemoveChild(child.GetControl());
                if (!resultRemove)
                {
                    MessageBox.Show("CantRemoveItemFromParent", "FailToRemoveParent",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return false;
                }
            }

            if (child.ControlWrapper != null)
            {
                child.ControlWrapper.Parent = null;

                if (ControlWrapper != null)
                {
                    ControlWrapper.AlignControls();
                }
                else
                {
                    var alignmentProvider = _control as IAlignment;
                    if (alignmentProvider != null)
                    {
                        alignmentProvider.AlignControls();
                    }
                }
                return true;
            }
            if (!ignoreInspector)
            {
                MessageBox.Show("Element can't be removed from inspector tree itself", "Can't delete element",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Exclamation);
                return false;
            }
            return true;
        }

        public void Remove()
        {
            RemoveChildren();
        }
    }
}
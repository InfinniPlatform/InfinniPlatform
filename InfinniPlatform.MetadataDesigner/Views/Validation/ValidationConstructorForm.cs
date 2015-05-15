using System.Drawing;
using InfinniPlatform.MetadataDesigner.Views.Validation.Controls;
using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace InfinniPlatform.MetadataDesigner.Views.Validation
{
    public partial class ValidationConstructorForm : Form
	{
	    private readonly ValidationConstructor _validationBuilder;
        
	    public ValidationConstructorForm(ValidationConstructor validationBuilder)
		{
		    _validationBuilder = validationBuilder;
		    InitializeComponent();
		}

	    private void ButtonOK_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.OK;
        }

        private void CancelButton_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
        }

        private void AddChildButton_Click(object sender, EventArgs e)
        {
            var currentControl = (IPredicateBuilderControl)panelBuilderPlaceholder.Controls[0];

            var currentPredicate = currentControl.GetPredicateDescription();

            _validationBuilder.AddValidationDescription(currentPredicate);
            
            // Кнопка ОК и предпросмотр становится активной после того, как
            // выражение содержит минимально необходимый набор предикатов.
            // Мы не можем закончить выражение композитным или корневым предикатом
            if (currentPredicate.Type == PredicateDescriptionType.Root ||
                currentPredicate.Type == PredicateDescriptionType.CollectionComposite ||
                currentPredicate.Type == PredicateDescriptionType.ObjectComposite)
            {
                ButtonOK.Enabled = false;
            }
            else
            {
                ButtonOK.Enabled = true;
                UpdatePreview();
            }

            ValidationTreeView.Nodes.Clear();

            var currentNode = new TreeNode
            {
                Text = _validationBuilder.RootPredicate.ToString(),
                Tag = _validationBuilder.RootPredicate
            };

            ValidationTreeView.Nodes.Add(currentNode);
            
            AddChildNodes(currentNode, _validationBuilder.RootPredicate.Children);

            ValidationTreeView.ExpandAll();

            panelBuilderPlaceholder.Controls.Clear();
        }

        private void AddChildNodes(TreeNode currentNode, IEnumerable<PredicateDescriptionNode> children)
        {
            foreach (var predicateDescriptionNode in children)
            {
                var newNode = new TreeNode
                {
                    Text = predicateDescriptionNode.ToString(),
                    Tag = predicateDescriptionNode
                };

                currentNode.Nodes.Add(newNode);

                if (predicateDescriptionNode.Children.Count > 0)
                {
                    AddChildNodes(newNode, predicateDescriptionNode.Children);
                }
            }
        }

        private void UpdatePreview()
        {
            string validationMessage = null;

            try
            {
                validationMessage = _validationBuilder.BuildValidationStatement().ToString();
            }
            catch( Exception e)
            {
                PreviewMemoEdit.Text = e.Message;
            }

            if (validationMessage != null)
            {
                PreviewMemoEdit.Text = validationMessage;
            }
        }

        private void AddNewNodeContextMenuClick(object sender, EventArgs e)
        {
            if (ValidationTreeView.SelectedNode != null)
            {
                var activePredicate = (PredicateDescriptionNode) ValidationTreeView.SelectedNode.Tag;

                PredicateDescriptionType controlType;

                switch (activePredicate.Type)
                {
                    case PredicateDescriptionType.Root:
                        controlType = 
                            activePredicate.MethodName == "ForObject" ? 
                            PredicateDescriptionType.ObjectComposite : 
                            PredicateDescriptionType.CollectionComposite;
                        break;
                    case PredicateDescriptionType.CollectionComposite:
                        controlType = PredicateDescriptionType.Collection;
                        break;
                    case PredicateDescriptionType.ObjectComposite:
                        controlType = PredicateDescriptionType.Object;
                        break;
                    case PredicateDescriptionType.CollectionBasePredicate:
                        if (activePredicate.MethodName == "All" || activePredicate.MethodName == "Any")
                        {
                            controlType = PredicateDescriptionType.ObjectComposite;
                        }
                        else
                        {
                            controlType = PredicateDescriptionType.CollectionBasePredicate;
                        }
                        break;
                    case PredicateDescriptionType.ObjectBasePredicate:
                        if (activePredicate.MethodName == "Property")
                        {
                            controlType = PredicateDescriptionType.ObjectComposite;
                        }
                        else if (activePredicate.MethodName == "Collection")
                        {
                            controlType = PredicateDescriptionType.CollectionComposite;
                        }
                        else
                        {
                            controlType = PredicateDescriptionType.ObjectBasePredicate;
                        }
                        break;

                    default:
                        throw new ArgumentOutOfRangeException();
                }

                panelBuilderPlaceholder.Controls.Clear();

                switch (controlType)
                {
                    case PredicateDescriptionType.Root:
                        panelBuilderPlaceholder.Controls.Add(new PredicateBuilderControl());
                        break;
                    case PredicateDescriptionType.ObjectComposite:
                        panelBuilderPlaceholder.Controls.Add(new ObjectCompositePredicateBuilderControl());
                        break;
                    case PredicateDescriptionType.CollectionComposite:
                        panelBuilderPlaceholder.Controls.Add(new CollectionCompositePredicateBuilderControl());
                        break;
                    case PredicateDescriptionType.ObjectBasePredicate:
                    case PredicateDescriptionType.Object:
                        panelBuilderPlaceholder.Controls.Add(new ObjectPredicateBuilderControl());
                        break;
                    case PredicateDescriptionType.CollectionBasePredicate:
                    case PredicateDescriptionType.Collection:
                        panelBuilderPlaceholder.Controls.Add(new CollectionPredicateBuilderControl());
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
                
                panelBuilderPlaceholder.Controls.Add(AddChildButton);

                _validationBuilder.ActivePredicate = activePredicate;
            }
            else if (ValidationTreeView.Nodes.Count == 0)
            {
                // Нет элементов, даём возможность добавить корневой элемент
                panelBuilderPlaceholder.Controls.Clear();
                panelBuilderPlaceholder.Controls.Add(new PredicateBuilderControl());
                
                panelBuilderPlaceholder.Controls.Add(AddChildButton);
            }

            if (panelBuilderPlaceholder.Controls.Count > 0)
            {
                panelBuilderPlaceholder.Controls[0].Location = new Point(2, 10);
                panelBuilderPlaceholder.Controls[1].Location = new Point(2, 430);
            }
        }

        private void ValidationTreeView_MouseUp(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                ValidationTreeView.SelectedNode = ValidationTreeView.GetNodeAt(e.X, e.Y);
                
                if (ValidationTreeView.SelectedNode != null)
                {
                    var activePredicate = (PredicateDescriptionNode)ValidationTreeView.SelectedNode.Tag;

                    if (activePredicate.Type != PredicateDescriptionType.Object &&
                        activePredicate.Type != PredicateDescriptionType.Collection)
                    {
                        menu.Show(ValidationTreeView, e.Location);
                    }
                }
                else if (ValidationTreeView.Nodes.Count == 0)
                {
                    menu.Show(ValidationTreeView, e.Location);
                }
            }
        }
	}
}

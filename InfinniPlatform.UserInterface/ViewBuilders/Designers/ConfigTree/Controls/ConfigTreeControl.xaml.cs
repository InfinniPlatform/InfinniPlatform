using System;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

using DevExpress.Data.Filtering;
using DevExpress.Data.Filtering.Helpers;
using DevExpress.Xpf.Grid;
using DevExpress.Xpf.Grid.TreeList;

using InfinniPlatform.UserInterface.ViewBuilders.Commands;

namespace InfinniPlatform.UserInterface.ViewBuilders.Designers.ConfigTree.Controls
{
	/// <summary>
	/// Элемент управления для отображения и редактирования дерева конфигураций.
	/// </summary>
	public sealed partial class ConfigTreeControl : UserControl
	{
		// EditPanel

		public static readonly DependencyProperty EditPanelProperty = DependencyProperty.Register("EditPanel",
																								  typeof(IConfigElementEditPanel), typeof(ConfigTreeControl));

		// SelectedElement

		public static readonly DependencyProperty SelectedElementProperty =
			DependencyProperty.Register("SelectedElement", typeof(ConfigElementNode), typeof(ConfigTreeControl));

		public ConfigTreeControl()
		{
			InitializeComponent();
		}

		/// <summary>
		/// Панель для размещения редакторов элементов конфигурации.
		/// </summary>
		public IConfigElementEditPanel EditPanel
		{
			get { return (IConfigElementEditPanel)GetValue(EditPanelProperty); }
			set { SetValue(EditPanelProperty, value); }
		}

		/// <summary>
		/// Элемент печатного представления, который выделен в дереве.
		/// </summary>
		public ConfigElementNode SelectedElement
		{
			get { return (ConfigElementNode)GetValue(SelectedElementProperty); }
			set { SetValue(SelectedElementProperty, value); }
		}

		private void OnLoaded(object sender, RoutedEventArgs e)
		{
			if (!DesignerProperties.GetIsInDesignMode(this))
			{
				var editPanel = EditPanel;

				Task.Factory
					.StartNew(() => ConfigTreeBuilder.Build(editPanel))
					.ContinueWith(t =>
								  {
									  if (!t.IsFaulted)
									  {
										  TreeList.ItemsSource = t.Result;

										  if (TreeList.View.Nodes.Count > 0)
										  {
											  TreeList.View.Nodes[0].IsExpanded = true;
										  }
									  }
								  },
								  TaskScheduler.FromCurrentSynchronizationContext());
			}
		}

		private void OnTreeListSelectedItemChanged(object sender, SelectedItemChangedEventArgs e)
		{
			SelectedElement = e.NewItem as ConfigElementNode;
		}

		private void OnTreeListMouseDown(object sender, MouseButtonEventArgs e)
		{
			var rowHandle = TreeList.View.GetRowHandleByMouseEventArgs(e);
			TreeList.View.FocusedRowHandle = rowHandle;
		}

		// ContextMenu

		private void OnCanRefreshCommandHandler(object sender, CanExecuteRoutedEventArgs e)
		{
			e.CanExecute = CanExecuteCommand(SelectedElement, i => i.RefreshCommand, true);
		}

		private void OnRefreshCommandHandler(object sender, ExecutedRoutedEventArgs e)
		{
			ExecuteCommand(SelectedElement, i => i.RefreshCommand, true);
		}

		private void OnCanCopyCommandHandler(object sender, CanExecuteRoutedEventArgs e)
		{
			e.CanExecute = CanExecuteCommand(SelectedElement, i => i.CopyCommand);
		}

		private void OnCopyCommandHandler(object sender, ExecutedRoutedEventArgs e)
		{
			ExecuteCommand(SelectedElement, i => i.CopyCommand);
		}

		private void OnCanPasteCommandHandler(object sender, CanExecuteRoutedEventArgs e)
		{
			e.CanExecute = CanExecuteCommand(SelectedElement, i => i.PasteCommand);
		}

		private void OnPasteCommandHandler(object sender, ExecutedRoutedEventArgs e)
		{
			ExecuteCommand(SelectedElement, i => i.PasteCommand);
		}

		private void OnCanDeleteCommandHandler(object sender, CanExecuteRoutedEventArgs e)
		{
			e.CanExecute = CanExecuteCommand(SelectedElement, i => i.DeleteCommand);
		}

		private void OnDeleteCommandHandler(object sender, ExecutedRoutedEventArgs e)
		{
			ExecuteCommand(SelectedElement, i => i.DeleteCommand);
		}

		private void OnTreeListKeyDown(object sender, KeyEventArgs e)
		{
			var treeView = TreeList.View;
			var focusedNode = treeView.GetNodeByRowHandle(treeView.FocusedRowHandle);

			if (focusedNode != null)
			{
				if (e.Key == Key.Right)
				{
					e.Handled = true;

					if (focusedNode.IsExpanded)
					{
						treeView.MoveNextRow();
					}
					else
					{
						focusedNode.IsExpanded = true;
					}
				}
				else if (e.Key == Key.Left)
				{
					e.Handled = true;

					if (focusedNode.IsExpanded)
					{
						focusedNode.IsExpanded = false;
					}
					else
					{
						treeView.MovePrevRow();
					}
				}
				else if (e.Key == Key.Enter)
				{
					e.Handled = true;

					ExecuteEditCommand();
				}
			}
		}

		private void OnNodeExpanded(object sender, TreeListNodeEventArgs e)
		{
			var node = e.Row as ConfigElementNode;

			if (CanExecuteCommand(node, i => i.RefreshCommand))
			{
				ExecuteCommand(node, i => i.RefreshCommand);
			}
		}

		private void OnNodeDoubleClick(object sender, RowDoubleClickEventArgs e)
		{
			ExecuteEditCommand();
		}

		private void ExecuteEditCommand()
		{
			if (CanExecuteCommand(SelectedElement,
								  i => (i.EditCommands != null)
										   ? i.EditCommands.FirstOrDefault()
										   : null))
			{
				ExecuteCommand(SelectedElement, i => (i.EditCommands != null)
														 ? i.EditCommands.FirstOrDefault()
														 : null);
			}
		}

		private static bool CanExecuteCommand<T>(ConfigElementNode node, Func<ConfigElementNode, ICommand<T>> selector,
												 T parameter = default(T))
		{
			if (node != null)
			{
				var command = selector(node);

				if (command != null)
				{
					return command.CanExecute(parameter);
				}
			}

			return false;
		}

		private static void ExecuteCommand<T>(ConfigElementNode node, Func<ConfigElementNode, ICommand<T>> selector,
											  T parameter = default(T))
		{
			if (node != null)
			{
				var command = selector(node);

				if (command != null)
				{
					//try
					//{
					command.Execute(parameter);
					//}
					//catch (Exception error)
					//{
					//    CommonHelper.ShowErrorMessage(error.Message);
					//}
				}
			}
		}

		private void OnCustomNodeFilter(object sender, TreeListNodeFilterEventArgs e)
		{
			var filter = ((TreeListView)sender).DataControl.FilterCriteria;
			if (filter == null)
			{
				return;
			}
			if (IsNodeVisible(e.Node, filter) || IsChildNodeVisible(e.Node, filter))
			{
				MakeNodeVisible(e.Node);
				e.Visible = true;
			}
			else
			{
				e.Visible = false;
			}
			e.Handled = true;
		}

		public bool IsChildNodeVisible(TreeListNode node, CriteriaOperator filter)
		{
			foreach (var n in node.Nodes)
			{
				if (IsNodeVisible(n, filter))
				{
					return true;
				}
				if (IsChildNodeVisible(n, filter))
				{
					return true;
				}
			}
			return false;
		}

		public bool IsNodeVisible(TreeListNode node, CriteriaOperator filter)
		{
			var ee = new ExpressionEvaluator(TypeDescriptor.GetProperties(node.Content.GetType()), filter, false);
			return ee.Fit(node.Content);
		}

		public void MakeNodeVisible(TreeListNode node)
		{
			if (node.ParentNode == null)
			{
				return;
			}
			node.ParentNode.IsExpanded = true;
			MakeNodeVisible(node.ParentNode);
		}
	}
}
using System;
using System.Linq;
using System.Windows.Forms;

using FastReport;

namespace InfinniPlatform.ReportDesigner.Views.Bands
{
	/// <summary>
	/// Представление для отображения и редактирования блоков отчета.
	/// </summary>
	sealed partial class ReportBandConfigView : Form
	{
		public ReportBandConfigView()
		{
			InitializeComponent();
		}


		private ReportPage _reportPage;

		/// <summary>
		/// Страница отчета.
		/// </summary>
		public ReportPage ReportPage
		{
			get
			{
				return _reportPage;
			}
			set
			{
				_reportPage = value;

				RebuildBandsTree();
			}
		}


		/// <summary>
		/// Событие изменения объекта отчета.
		/// </summary>
		public event EventHandler ReportChanged;

		private void InvokeReportChanged()
		{
			if (ReportChanged != null)
			{
				ReportChanged(this, EventArgs.Empty);
			}

			RebuildBandsTree();
		}


		// MENU HANDLERS

		private void OnAddReportTitle(object sender, EventArgs e)
		{
			ReportPage.ReportTitle = CreateReportBand<ReportTitleBand>();
			InvokeReportChanged();
		}

		private void OnAddReportSummary(object sender, EventArgs e)
		{
			ReportPage.ReportSummary = CreateReportBand<ReportSummaryBand>();
			InvokeReportChanged();
		}

		private void OnAddPageHeader(object sender, EventArgs e)
		{
			ReportPage.PageHeader = CreateReportBand<PageHeaderBand>();
			InvokeReportChanged();
		}

		private void OnAddPageFooter(object sender, EventArgs e)
		{
			ReportPage.PageFooter = CreateReportBand<PageFooterBand>();
			InvokeReportChanged();
		}

		private void OnAddGroupHeader(object sender, EventArgs e)
		{
			var selectedBand = GetBandNode(BandsTree.SelectedNode);

			var groupHeaderBand = CreateReportBand<GroupHeaderBand>();
			var groupFooterBand = CreateReportBand<GroupFooterBand>();
			groupHeaderBand.GroupFooter = groupFooterBand;

			var parent = selectedBand.Parent;
			selectedBand.Parent = groupHeaderBand;
			groupHeaderBand.Parent = parent;

			InvokeReportChanged();
		}

		private void OnAddGroupFooter(object sender, EventArgs e)
		{
			var groupHeaderBand = (GroupHeaderBand)GetBandNode(BandsTree.SelectedNode);
			var groupFooterBand = CreateReportBand<GroupFooterBand>();
			groupHeaderBand.GroupFooter = groupFooterBand;

			InvokeReportChanged();
		}

		private void OnAddData(object sender, EventArgs e)
		{
			var selectedBand = GetBandNode(BandsTree.SelectedNode);

			var dataBand = CreateReportBand<DataBand>();
			dataBand.Parent = (selectedBand is DataBand) ? (Base)selectedBand : ReportPage;

			InvokeReportChanged();
		}

		private void OnDelete(object sender, EventArgs e)
		{
			var selectedBand = GetBandNode(BandsTree.SelectedNode);
			selectedBand.Delete();
			InvokeReportChanged();
		}

		private TBand CreateReportBand<TBand>() where TBand : BandBase, new()
		{
			var band = new TBand();
			band.SetReport(ReportPage.Report);
			band.CreateUniqueName();
			band.Height = band.GetPreferredSize().Height;

			return band;
		}


		// BUTTON HANDLERS

		private void OnAdd(object sender, EventArgs e)
		{
			Separator0.Visible = false;
			DeleteMenuBtn.Visible = false;
			AddButtonMenu.Show(AddBtn, 0, AddBtn.Height);
		}

		private void OnMoveUp(object sender, EventArgs e)
		{
			var selectedBand = GetBandNode(BandsTree.SelectedNode);

			if ((selectedBand.Parent == ReportPage) || (selectedBand is DataBand))
			{
				selectedBand.ZOrder--;

				InvokeReportChanged();
			}
			else if ((selectedBand is GroupHeaderBand) && (selectedBand.Parent is GroupHeaderBand))
			{
				// parent
				// - group1
				// -- group2 (move up)
				// --- band

				var group1 = selectedBand.Parent;
				var group2 = (GroupHeaderBand)selectedBand;
				var band = (group2.Data != null) ? (BandBase)group2.Data : group2.NestedGroup;

				var parent = group1.Parent;
				var parentOrder = group1.ZOrder;

				group1.Parent = null;
				group2.Parent = parent;
				group2.ZOrder = parentOrder;
				band.Parent = group1;
				group1.Parent = group2;

				InvokeReportChanged();
			}
		}

		private void OnMoveDown(object sender, EventArgs e)
		{
			var selectedBand = GetBandNode(BandsTree.SelectedNode);

			if ((selectedBand.Parent == ReportPage) || (selectedBand is DataBand))
			{
				selectedBand.ZOrder += 2;

				InvokeReportChanged();
			}
			else
			{
				var group1 = selectedBand as GroupHeaderBand;

				if (group1 != null && group1.NestedGroup != null)
				{
					// parent
					// - group1 (move down)
					// -- group2
					// --- band

					var group2 = group1.NestedGroup;

					var parent = selectedBand.Parent;
					var parentOrder = group1.ZOrder;

					group1.Parent = null;
					group2.Parent = parent;
					group2.ZOrder = parentOrder;

					if (group2.Data != null)
					{
						group1.Data = group2.Data;
					}
					else
					{
						group1.NestedGroup = group2.NestedGroup;
					}

					group1.Parent = group2;

					InvokeReportChanged();
				}
			}
		}

		private void OnClose(object sender, EventArgs e)
		{
			Close();
		}


		// TREE HANDLERS

		private void OnNodeClick(object sender, TreeNodeMouseClickEventArgs e)
		{
			BandsTree.SelectedNode = e.Node;

			if (e.Button == MouseButtons.Right)
			{
				Separator0.Visible = true;
				DeleteMenuBtn.Visible = true;
			}
		}

		private void OnSelectedNodeChanged(object sender, TreeViewEventArgs e)
		{
			SetAllowedActions();
		}

		private void OnBeforeCollapse(object sender, TreeViewCancelEventArgs e)
		{
			e.Cancel = true;
		}

		private void SetAllowedActions()
		{
			var selectedBand = GetBandNode(BandsTree.SelectedNode);
			var hasReportPage = (ReportPage != null);

			// Можно добавлять заголовок и итоги отчета, если их еще нет
			AddReportTitleBtn.Enabled = (hasReportPage && ReportPage.ReportTitle == null);
			AddReportSummaryBtn.Enabled = (hasReportPage && ReportPage.ReportSummary == null);

			// Можно добавлять заголовок и итоги стараницы, если их еще нет
			AddPageHeaderBtn.Enabled = (hasReportPage && ReportPage.PageHeader == null);
			AddPageFooterBtn.Enabled = (hasReportPage && ReportPage.PageFooter == null);

			// Можно добавлять заголовок группы, если выделена группа или блок данных
			AddGroupHeaderBtn.Enabled = (hasReportPage && (selectedBand is GroupHeaderBand || selectedBand is DataBand));

			// Можно добавлять итоги группы, если выделена группа, у которой нет итогов
			AddGroupFooterBtn.Enabled = (hasReportPage && selectedBand is GroupHeaderBand && ((GroupHeaderBand)selectedBand).GroupFooter == null);

			// Можно добавлять блок данных, если это дочерний блок или у отчета нет блока данных
			AddDataBandBtn.Enabled = (hasReportPage && (selectedBand is DataBand || ReportPage.AllObjects.OfType<DataBand>().Any() == false));

			// Можно удалять любой блок, если это не блок данных, либо у блока данных нет групп
			DeleteBtn.Enabled = (hasReportPage && selectedBand != null && (selectedBand is DataBand == false || selectedBand.Parent as GroupHeaderBand == null));
			DeleteMenuBtn.Enabled = DeleteBtn.Enabled;

			// Можно перемещать только группы и блоки данных
			MoveUpBtn.Enabled = (hasReportPage && (selectedBand is GroupHeaderBand || selectedBand is DataBand));
			MoveDownBtn.Enabled = (hasReportPage && (selectedBand is GroupHeaderBand || selectedBand is DataBand));
		}


		// HELPERS

		private void RebuildBandsTree()
		{
			var selectedNode = BandsTree.SelectedNode;

			BandsTree.Nodes.Clear();

			if (ReportPage != null)
			{
				AddBandNode(BandsTree.Nodes, ReportPage.ReportTitle, "Report Title: {0}", "ReportTitle");
				AddBandNode(BandsTree.Nodes, ReportPage.PageHeader, "Page Header: {0}", "PageHeader");
				AddDataBandNode(BandsTree.Nodes, ReportPage.Bands);
				AddBandNode(BandsTree.Nodes, ReportPage.ReportSummary, "Report Summary: {0}", "ReportSummary");
				AddBandNode(BandsTree.Nodes, ReportPage.PageFooter, "Page Footer: {0}", "PageFooter");
			}

			BandsTree.ExpandAll();

			SelectBandNode(selectedNode);

			SetAllowedActions();
		}

		private void AddDataBandNode(TreeNodeCollection nodes, BandCollection bands)
		{
			foreach (var band in bands)
			{
				if (band is DataBand)
				{
					BuildDataBandTree(nodes, (DataBand)band);
				}
				else if (band is GroupHeaderBand)
				{
					BuildGroupBandTree(nodes, (GroupHeaderBand)band);
				}
			}
		}

		private void BuildGroupBandTree(TreeNodeCollection parent, GroupHeaderBand groupBand)
		{
			if (groupBand != null)
			{
				var groupBandNode = AddBandNode(parent, groupBand, "Group Header: {0}", "GroupHeader");

				if (groupBand.Data != null)
				{
					BuildDataBandTree(groupBandNode.Nodes, groupBand.Data);
				}
				else if (groupBand.NestedGroup != null)
				{
					BuildGroupBandTree(groupBandNode.Nodes, groupBand.NestedGroup);
				}

				if (groupBand.GroupFooter != null)
				{
					AddBandNode(parent, groupBand.GroupFooter, "Group Footer: {0}", "GroupFooter");
				}
			}
		}

		private void BuildDataBandTree(TreeNodeCollection parent, DataBand dataBand)
		{
			if (dataBand != null)
			{
				var dataBandNode = AddBandNode(parent, dataBand, "Data: {0}", "Data");

				AddDataBandNode(dataBandNode.Nodes, dataBand.Bands);
			}
		}

		private static TreeNode AddBandNode(TreeNodeCollection parent, BandBase band, string caption, string image)
		{
			if (band != null)
			{
				var bandNode = parent.Add(band.Name, string.Format(caption, band.Name), image, image);
				bandNode.Tag = band;

				return bandNode;
			}

			return null;
		}

		private void SelectBandNode(TreeNode node)
		{
			if (node != null)
			{
				var band = GetBandNode(node);

				if (band != null)
				{
					BandsTree.SelectedNode = FindBandNode(BandsTree.Nodes, band);
				}
			}
		}

		private static BandBase GetBandNode(TreeNode node)
		{
			return (node != null) ? node.Tag as BandBase : null;
		}

		private static TreeNode FindBandNode(TreeNodeCollection parent, BandBase band)
		{
			TreeNode result = null;

			foreach (TreeNode childNode in parent)
			{
				var childBand = GetBandNode(childNode);

				result = (childBand == null || !childBand.Equals(band))
							 ? FindBandNode(childNode.Nodes, band)
							 : childNode;

				if (result != null)
				{
					break;
				}
			}

			return result;
		}
	}
}
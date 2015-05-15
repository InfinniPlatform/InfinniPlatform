using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using InfinniPlatform.Api.Schema;
using InfinniPlatform.Api.SearchOptions;
using InfinniPlatform.QueryDesigner.Contracts;

namespace InfinniPlatform.QueryDesigner.Views
{
	public partial class QueryConstructorWhereCondition : UserControl
	{
		public QueryConstructorWhereCondition()
		{
			InitializeComponent();
		}

		public string Caption
		{
			get { return FieldPart.Caption; }
			set { FieldPart.Caption = value; }
		}

		public CriteriaType CriteriaType
		{
			get { return ConditionPart.Value; }
			set { ConditionPart.Value = value; }
		}

		public object Value
		{
			get { return ValuePart.EditValue; }
			set { ValuePart.EditValue = value; }
		}

		public string Property
		{
			get { return FieldPart.Path; }
		}

		public void SetPathItems(IEnumerable<dynamic> pathItems)
		{
			FieldPart.SetPathItems(pathItems);
		}

		public IDataProvider DataProvider
		{
			get { return FieldPart.DataProvider; }
			set { FieldPart.DataProvider = value; }
		}

		public Action<Control> OnQueryDelete
		{
			get;
			set;
		}

		public PathResolveType PathResolveType
		{
			get { return FieldPart.PathResolveType; }
			set { FieldPart.PathResolveType = value; }
		}

		private void ButtonRemoveClick(object sender, EventArgs e)
		{
			if (OnQueryDelete != null)
			{
				OnQueryDelete(this);
			}
		}
	}
}

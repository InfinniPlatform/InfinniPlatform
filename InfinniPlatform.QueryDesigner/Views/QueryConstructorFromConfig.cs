using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using InfinniPlatform.Api.Dynamic;
using InfinniPlatform.QueryDesigner.Contracts;

namespace InfinniPlatform.QueryDesigner.Views
{
	public partial class QueryConstructorFromConfig : UserControl, IQueryBlockProvider
	{
		public QueryConstructorFromConfig()
		{
			InitializeComponent();
		}

		public ConstructOrder GetConstructOrder()
		{
			return ConstructOrder.ConstructFrom;			
		}

		public void ProcessQuery(dynamic query)
		{
			if (IndexConfigPart.Configuration != null && IndexConfigPart.Document != null)
			{
				query.From = new DynamicWrapper();
				query.From.Index = IndexConfigPart.Configuration;
				query.From.Type = IndexConfigPart.Document;
				if (!string.IsNullOrEmpty(IndexConfigPart.Alias))
				{
					query.From.Alias = IndexConfigPart.Alias;
				}
			}
		}

		public bool DefinitionCompleted()
		{
			return !string.IsNullOrEmpty(Configuration) && !string.IsNullOrEmpty(Document);
		}

		public string GetErrorMessage()
		{
			return "Some required field in block 'FROM' unsettled.";
		}

		public Action<string> OnDocumentValueChanged
		{
			get { return IndexConfigPart.OnDocumentValueChanged; }
			set { IndexConfigPart.OnDocumentValueChanged = value; }
		}

		public Action<string> OnConfigurationValueChanged
		{
			get { return IndexConfigPart.OnConfigurationValueChanged; }
			set { IndexConfigPart.OnConfigurationValueChanged = value; }
		}

		public string Configuration
		{
			get { return IndexConfigPart.Configuration; }
		}

		public string Document
		{
			get { return IndexConfigPart.Document; }
		}

	    public Action<string> OnAliasValueChanged
	    {
	        get { return IndexConfigPart.OnAliasValueChanged; }
	        set { IndexConfigPart.OnAliasValueChanged = value; }
	    }

	    public bool ShowAlias
	    {
	        get { return IndexConfigPart.ShowAlias; }
	        set { IndexConfigPart.ShowAlias = value; }
	    }
	}
}

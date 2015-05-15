using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

using DevExpress.XtraEditors;
using DevExpress.XtraEditors.Controls;

using InfinniPlatform.Api.Hosting;
using InfinniPlatform.Api.TestEnvironment;
using InfinniPlatform.QueryDesigner.Contracts;

namespace InfinniPlatform.QueryDesigner.Views
{
	/// <summary>
	///   Контрол, предоставляющий список конфигураций и документов внутри них для выбора
	/// </summary>
	public partial class QueryConstructorIndexConfig : UserControl, IRequestExecutor, IInitializedOnLoad
	{
	    private bool _showAlias;

	    public QueryConstructorIndexConfig()
		{
			InitializeComponent();
		}

		public void InitRouting(HostingConfig hostingConfig)
		{
			TestApi.InitClientRouting(hostingConfig);
		}

		public void OnLoad()
		{
			ComboBoxConfiguration.Properties.Items.Clear();
			var configurations = DataProvider.GetConfigurationList();
			ComboBoxConfiguration.Properties.Items.AddRange(
				configurations.Select(c => new ImageComboBoxItem(string.Format("{0} ({1})", c.Caption, c.Name), c)).ToList());
		}

		private void ComboBoxConfigurationEditValueChanged(object sender, EventArgs e)
		{
			ComboBoxDocument.EditValue = null;
			ComboBoxDocument.Properties.Items.Clear();

			if (ComboBoxConfiguration.EditValue != null)
			{
				IEnumerable<dynamic> documents = DataProvider.GetDocuments(((dynamic)ComboBoxConfiguration.EditValue).Name);
				ComboBoxDocument.Properties.Items.AddRange(documents.Select(d => new ImageComboBoxItem(string.Format("{0} ({1})", d.Name, d.Caption), d)).ToList());
			}

			if (OnConfigurationValueChanged != null)
			{
				OnConfigurationValueChanged(Configuration);
			}
		}

		private void ComboBoxDocumentEditValueChanged(object sender, EventArgs e)
		{
			if (OnDocumentValueChanged != null)
			{
				OnDocumentValueChanged(Document);
			}
		}

        private void TextEditAliasEditValueChanged(object sender, EventArgs e)
        {
            if (OnAliasValueChanged != null)
            {
                OnAliasValueChanged(Alias);
            }
        }



		private void DeleteValueClick(object sender, ButtonPressedEventArgs e)
		{
			if (e.Button.Kind == ButtonPredefines.Delete)
			{
				((BaseEdit)sender).EditValue = null;
			}
		}

		public string Configuration
		{
			get { return ComboBoxConfiguration.EditValue != null ? ((dynamic)ComboBoxConfiguration.EditValue).Name : string.Empty; }
		}

		public string Document
		{
			get { return ComboBoxDocument.EditValue != null ? ((dynamic)ComboBoxDocument.EditValue).Name : string.Empty; }
		}

		public string Alias
		{
			get { return TextEditAlias.Text; }
		}

		public Action<string> OnDocumentValueChanged { get; set; }

		public Action<string> OnConfigurationValueChanged { get; set; }

        public Action<string> OnAliasValueChanged { get; set; }

		public IDataProvider DataProvider { get; set; }

	    public bool ShowAlias
	    {
	        get { return _showAlias; }
            set
            {
                _showAlias = value;
                LabelAlias.Visible = _showAlias;
                TextEditAlias.Visible = _showAlias;
            }
	    }

	}
}

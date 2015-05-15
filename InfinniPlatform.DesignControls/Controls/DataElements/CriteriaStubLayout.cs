using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using InfinniPlatform.DesignControls.Layout;

namespace InfinniPlatform.DesignControls.Controls.DataElements
{
	public partial class CriteriaStubLayout : UserControl, IClientHeightProvider
	{
		public CriteriaStubLayout()
		{
			InitializeComponent();
		}

		public string Caption
		{
			get { return CaptionLabel.Text; }
			set { CaptionLabel.Text = value; }
		}

		public int GetClientHeight()
		{
			return 32;
		}

		public bool IsFixedHeight()
		{
			return true;
		}
	}
}

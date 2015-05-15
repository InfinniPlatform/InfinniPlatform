using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace InfinniPlatform.DesignControls.ScriptEditor
{
	public partial class ScriptEditForm : Form
	{
		public ScriptEditForm()
		{
			InitializeComponent();
		}

		public string Script
		{
			get { return ScriptEditor.Script; }
			set { ScriptEditor.Script = value; }
		}
	}
}

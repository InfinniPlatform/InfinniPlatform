using System.Windows.Forms;

namespace InfinniPlatform.DesignControls.PropertyDesigner
{
    public partial class ValueEdit : Form
    {
        public ValueEdit()
        {
            InitializeComponent();
        }

        public string Value
        {
            get { return ScriptEditor.Script; }
            set { ScriptEditor.Script = value; }
        }

	    public bool ReadOnly
	    {
		    get { return ScriptEditor.ReadOnly; }
		    set { ScriptEditor.ReadOnly = value; }
	    }
    }
}

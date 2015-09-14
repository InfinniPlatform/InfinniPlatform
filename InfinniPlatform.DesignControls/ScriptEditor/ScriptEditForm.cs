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
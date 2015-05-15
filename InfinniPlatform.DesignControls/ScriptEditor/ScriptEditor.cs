using System.Drawing;
using System.Windows.Forms;
using DigitalRune.Windows.TextEditor.Formatting;
using DigitalRune.Windows.TextEditor.Highlighting;
using DigitalRune.Windows.TextEditor.Properties;

namespace InfinniPlatform.DesignControls.ScriptEditor
{
	public partial class ScriptEditor : UserControl
	{
		private bool _viewMode;

		public ScriptEditor()
		{
			InitializeComponent();

			ScriptTextEditor.Document.HighlightingStrategy = HighlightingManager.Manager.FindHighlighter("JavaScript");

			ScriptTextEditor.Document.FormattingStrategy = new CSharpFormattingStrategy();

			ScriptTextEditor.Document.FoldingManager.FoldingStrategy = new CodeFoldingStrategy();

			Font consolasFont = new Font("Consolas", 9.75f);
			if (consolasFont.Name == "Consolas")        // Set font if it is available on this machine.
				ScriptTextEditor.Font = consolasFont;
		}

		public string Script
		{
			get { return ScriptTextEditor.Document.TextContent; }
			set { ScriptTextEditor.Document.TextContent = value; }
		}

		public bool ViewMode
		{
			get { return _viewMode; }
			set { 
				_viewMode = value;
				ScriptTextEditor.ShowLineNumbers = !value;
				ScriptTextEditor.LineViewerStyle = value ? LineViewerStyle.None : LineViewerStyle.FullRow;
				ScriptTextEditor.ShowScrollBars = !value;
				ScriptTextEditor.IsReadOnly = true;
				ScriptTextEditor.ShowVRuler = false;
			}
		}

		public bool ReadOnly
		{
			get { return ScriptTextEditor.IsReadOnly; }
			set { ScriptTextEditor.IsReadOnly = value; }
		}
	}
}

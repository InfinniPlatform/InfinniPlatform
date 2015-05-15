using System;
using System.ComponentModel;
using System.Globalization;
using System.Windows.Forms;

namespace InfinniPlatform.ReportDesigner.Views.Controls
{
	/// <summary>
	/// Элемент для редактирования чисел.
	/// </summary>
	sealed partial class NumericEditor : UserControl
	{
		public NumericEditor()
		{
			InitializeComponent();

			TextBoxControl.KeyDown += (s, e) => OnKeyDown(e);

			_decimalSeparator = CultureInfo.CurrentCulture.NumberFormat.NumberDecimalSeparator[0];
		}


		private readonly char _decimalSeparator;


		[DefaultValue(false)]
		public bool IsRealNumber
		{
			get;
			set;
		}

		[DefaultValue(0)]
		public double Value
		{
			get
			{
				double result;
				double.TryParse(TextBoxControl.Text, out result);

				return result;
			}
			set
			{
				TextBoxControl.Text = (value != 0) ? value.ToString(CultureInfo.InvariantCulture) : string.Empty;
			}
		}

		[Browsable(false)]
		public override string Text
		{
			get
			{
				return TextBoxControl.Text;
			}
			set
			{
			}
		}


		private void OnKeyPress(object sender, KeyPressEventArgs e)
		{
			if (e.KeyChar == '.' || e.KeyChar == ',')
			{
				e.KeyChar = _decimalSeparator;
			}

			if (char.IsNumber(e.KeyChar) == false && e.KeyChar != '-' && e.KeyChar != '\b' && e.KeyChar != _decimalSeparator)
			{
				e.Handled = true;
			}

			if (e.KeyChar == _decimalSeparator && IsRealNumber)
			{
				if (TextBoxControl.SelectionStart == 0)
				{
					TextBoxControl.Text = string.Format("0{0}{1}", _decimalSeparator, TextBoxControl.Text);
					TextBoxControl.SelectionStart = 2;
				}

				if (TextBoxControl.TextLength < 1)
				{
					e.Handled = true;
				}
				else
				{
					if (TextBoxControl.Text[0] == '-' && TextBoxControl.SelectionStart == 1)
					{
						e.Handled = true;
					}

					for (var i = 0; i < TextBoxControl.TextLength; ++i)
					{
						if (TextBoxControl.Text[i] == _decimalSeparator)
						{
							e.Handled = true;
						}
					}
				}
			}
			else if (e.KeyChar == _decimalSeparator)
			{
				e.Handled = true;
			}

			if (TextBoxControl.SelectionStart != 0)
			{
				if (e.KeyChar == '-')
				{
					e.Handled = true;
				}
			}
		}

		private void OnClipboardPaste(object sender, ClipboardEventArgs e)
		{
			var clipboardText = (e.Text ?? string.Empty).Replace('.', _decimalSeparator).Replace(',', _decimalSeparator);

			double clipboardValue;

			if (double.TryParse(clipboardText, out clipboardValue) == false)
			{
				clipboardText = null;
			}

			e.Text = clipboardText;
		}


		sealed class ClipboardTextBox : TextBox
		{
			public event EventHandler<ClipboardEventArgs> ClipboardPaste;

			protected override void WndProc(ref Message m)
			{
				if (m.Msg == 0x302 && Clipboard.ContainsText() && ClipboardPaste != null)
				{
					var clipboardEventArgs = new ClipboardEventArgs(Clipboard.GetText());

					ClipboardPaste(this, clipboardEventArgs);

					if (string.IsNullOrEmpty(clipboardEventArgs.Text))
					{
						return;
					}

					Clipboard.SetText(clipboardEventArgs.Text);
				}

				base.WndProc(ref m);
			}
		}


		sealed class ClipboardEventArgs : EventArgs
		{
			public ClipboardEventArgs(string text)
			{
				Text = text;
			}

			public string Text { get; set; }
		}
	}
}
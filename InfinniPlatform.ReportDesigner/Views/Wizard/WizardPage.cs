using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace InfinniPlatform.ReportDesigner.Views.Wizard
{
	sealed class WizardPage
	{
		public Control Target;
		public Func<bool> Condition;
		public Func<bool> OnNext;
		public Func<bool> OnBack;
		public Func<bool> OnFinish;
		public Func<bool> OnCancel;
		public Action OnReset;
		public ICollection<WizardPage> Pages;
	}
}
using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace InfinniPlatform.ReportDesigner.Views.Wizard
{
    internal sealed class WizardPage
    {
        public Func<bool> Condition;
        public Func<bool> OnBack;
        public Func<bool> OnCancel;
        public Func<bool> OnFinish;
        public Func<bool> OnNext;
        public Action OnReset;
        public ICollection<WizardPage> Pages;
        public Control Target;
    }
}
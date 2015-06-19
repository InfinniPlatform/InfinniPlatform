using System;
using System.Windows.Forms;

namespace InfinniPlatform.ReportDesigner.Views.Wizard
{
    sealed partial class WizardView : Form
    {
        private WizardRunner _wizardRunner;

        public WizardView()
        {
            InitializeComponent();
        }

        public void SetupSteps(Control target, Action<WizardPageBuilder> page)
        {
            _wizardRunner = new WizardRunner(target, page);

            SetCurrentPage();
        }

        public void Reset()
        {
            if (_wizardRunner != null)
            {
                _wizardRunner.ResetAll();

                SetCurrentPage();
            }
        }

        private void OnNext(object sender, EventArgs e)
        {
            _wizardRunner.Next();

            SetCurrentPage();
        }

        private void OnBack(object sender, EventArgs e)
        {
            _wizardRunner.Back();

            SetCurrentPage();
        }

        private void OnFinish(object sender, EventArgs e)
        {
            if (_wizardRunner.Finish())
            {
                DialogResult = DialogResult.OK;
                Close();
            }
        }

        private void OnCancel(object sender, EventArgs e)
        {
            _wizardRunner.Cancel();

            DialogResult = DialogResult.Cancel;
            Close();
        }

        private void OnReset(object sender, EventArgs e)
        {
            _wizardRunner.Reset();
        }

        private void SetCurrentPage()
        {
            var currentPageTarget = _wizardRunner.CurrentPage.Target;

            if (MainPanel.Controls.Count == 0 || MainPanel.Controls[0] != currentPageTarget)
            {
                currentPageTarget.Dock = DockStyle.Fill;

                MainPanel.Controls.Clear();
                MainPanel.Controls.Add(currentPageTarget);

                NextBtn.Enabled = _wizardRunner.CanNext;
                BackBtn.Enabled = _wizardRunner.CanBack;
                FinishBtn.Enabled = _wizardRunner.CanFinish;
            }
        }
    }
}
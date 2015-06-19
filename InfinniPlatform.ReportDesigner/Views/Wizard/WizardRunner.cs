using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace InfinniPlatform.ReportDesigner.Views.Wizard
{
    internal sealed class WizardRunner
    {
        private readonly Stack<WizardPage> _historySteps;
        private readonly WizardPage _rootPage;

        public WizardRunner(Control target, Action<WizardPageBuilder> page)
        {
            var pageBuilder = new WizardPageBuilder(target);

            if (page != null)
            {
                page(pageBuilder);
            }

            _rootPage = pageBuilder.Build();
            _historySteps = new Stack<WizardPage>();

            CurrentPage = _rootPage;
        }

        public WizardPage CurrentPage { get; private set; }

        public bool CanNext
        {
            get { return CurrentPage.Pages.Count > 0; }
        }

        public bool CanBack
        {
            get { return _historySteps.Count > 0; }
        }

        public bool CanFinish
        {
            get { return CurrentPage.Pages.Count == 0; }
        }

        public void Next()
        {
            if (CanNext && CurrentPage.OnNext())
            {
                foreach (var nextPage in CurrentPage.Pages)
                {
                    if (nextPage.Condition())
                    {
                        _historySteps.Push(CurrentPage);

                        CurrentPage = nextPage;

                        break;
                    }
                }
            }
        }

        public void Back()
        {
            if (CanBack && CurrentPage.OnBack())
            {
                var prevPage = _historySteps.Pop();

                CurrentPage = prevPage;
            }
        }

        public bool Finish()
        {
            return CanFinish && CurrentPage.OnFinish();
        }

        public bool Cancel()
        {
            var success = CurrentPage.OnCancel();

            foreach (var page in _historySteps)
            {
                success &= page.OnCancel();
            }

            return success;
        }

        public void Reset()
        {
            CurrentPage.OnReset();
        }

        public void ResetAll()
        {
            Reset();

            foreach (var page in _historySteps)
            {
                page.OnReset();
            }

            _historySteps.Clear();

            CurrentPage = _rootPage;
        }
    }
}
using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace InfinniPlatform.ReportDesigner.Views.Wizard
{
	sealed class WizardPageBuilder
	{
		private static readonly Action NullAction = () => { };
		private static readonly Func<bool> TruePredicate = () => true;


		public WizardPageBuilder(Control target)
		{
			_page = new WizardPage
						{
							Target = target,
							Condition = TruePredicate,
							OnNext = TruePredicate,
							OnBack = TruePredicate,
							OnFinish = TruePredicate,
							OnCancel = TruePredicate,
							OnReset = NullAction,
							Pages = new List<WizardPage>()
						};
		}


		private readonly WizardPage _page;


		public WizardPageBuilder Condition(Func<bool> value)
		{
			_page.Condition = value ?? TruePredicate;

			return this;
		}

		public WizardPageBuilder OnNext(Func<bool> value)
		{
			_page.OnNext = value ?? TruePredicate;

			return this;
		}

		public WizardPageBuilder OnBack(Func<bool> value)
		{
			_page.OnBack = value ?? TruePredicate;

			return this;
		}

		public WizardPageBuilder OnFinish(Func<bool> value)
		{
			_page.OnFinish = value ?? TruePredicate;

			return this;
		}

		public WizardPageBuilder OnCancel(Func<bool> value)
		{
			_page.OnCancel = value ?? TruePredicate;

			return this;
		}

		public WizardPageBuilder OnReset(Action value)
		{
			_page.OnReset = value ?? NullAction;

			return this;
		}

		public WizardPageBuilder AddPage(Control target, Action<WizardPageBuilder> page = null)
		{
			var childPageBuilder = new WizardPageBuilder(target);

			if (page != null)
			{
				page(childPageBuilder);
			}

			_page.Pages.Add(childPageBuilder._page);

			return this;
		}


		public WizardPage Build()
		{
			return _page;
		}
	}
}
using System;
using System.Dynamic;

using InfinniPlatform.Api.Actions;
using InfinniPlatform.Api.ContextTypes.ContextImpl;
using InfinniPlatform.Api.Validation;
using InfinniPlatform.Metadata;
using InfinniPlatform.Metadata.StateMachine.Statuses;

using NUnit.Framework;


namespace InfinniPlatform.Core.Tests.Events
{
	[TestFixture]
	[Category(TestCategories.UnitTest)]
	public class StatusBehavior
	{
	    [Test]
	    public void ShouldMoveWithoutAction()
	    {
	        //given
	        dynamic expandoPerson = new ExpandoObject();
	        expandoPerson.Id = "123";
	        expandoPerson.Type = "SomeType";
	        expandoPerson.Family = "Иванов";


	        //when
			//var stateChanger = new StateWorkflowRegister();
			//stateChanger.DefineWorkflow("testFlow", wf =>
			//	wf.ForState("Temporary",
			//		wc => wc.Move(new StateTransitionApproverObjectStatus("Saved"),
			//			new StateTransitionInvalidObjectStatus())
			//			.Move(new StateTransitionApproverObjectStatus("Published"),
			//				new StateTransitionInvalidObjectStatus())));


			//stateChanger.MoveWorkflow("testFlow", expandoPerson);
			////then
			//Assert.AreEqual(expandoPerson.Status, "Published");
	    }

	    [Test]
	    public void ShouldNotMoveIfActionFailed()
	    {
	        //given
	        dynamic expandoPerson = new ExpandoObject();
	        expandoPerson.Id = "123";
	        expandoPerson.Type = "SomeType";
	        expandoPerson.Family = "Иванов";

	        //when
			//var stateChanger = new StateWorkflowRegister();
			//stateChanger.DefineWorkflow("testFlow", wf =>
			//	wf.ForState("Temporary",
			//		wc => wc
			//			.Move(new StateTransitionApproverObjectStatus("Saved"),
			//				new StateTransitionInvalidObjectStatus(), config => config
			//					.WithAction(() => new TestActionOperator())
			//					.WithValidationError(() => new TestValidationOperator(false)))));


			//stateChanger.MoveWorkflow("testFlow", expandoPerson);
			////then
			//Assert.AreEqual(expandoPerson.Status.ToString(), "Invalid");
			//Assert.AreEqual(expandoPerson.Family, "Иванов");
	    }


	    [Test]
	    public void ShouldNotThrowIfActionFailed()
	    {
	        //given
	        dynamic expandoPerson = new ExpandoObject();
	        expandoPerson.Id = "123";
	        expandoPerson.Type = "SomeType";
	        expandoPerson.Family = "Иванов";

	        //when
			//var stateChanger = new StateWorkflowRegister();
			//stateChanger.DefineWorkflow("testFlow", wf => wf.ForState("Temporary",
			//	wc => wc
			//		.Move(new StateTransitionApproverObjectStatus("Saved"),
			//			new StateTransitionInvalidObjectStatus(), config =>
			//				config
			//					.WithAction(() => new TestActionOperatorException())
			//					.WithValidationError(() => new TestValidationOperator(true)))));

			//var eventContext = new ApplyContext();
			//eventContext.Item = expandoPerson;
			//stateChanger.MoveWorkflow("testFlow", eventContext);
			////then
			//Assert.AreEqual(eventContext.Status.ToString(), "Invalid");
			//Assert.AreEqual(expandoPerson.Family, "Иванов");
	    }

	    [Test]
	    public void ShouldMoveToStateNonDefaultAction()
	    {
	        //given
	        dynamic expandoPerson = new ExpandoObject();
	        expandoPerson.Id = "123";
	        expandoPerson.Type = "SomeType";
	        expandoPerson.Family = "Иванов";

	        //when
			//var stateChanger = new StateWorkflowRegister();
			//stateChanger.DefineWorkflow("testFlow", wf =>
			//	wf.ForState("Temporary",
			//		wc => wc.Move(new StateTransitionApproverObjectStatus("Saved"),
			//			new StateTransitionInvalidObjectStatus(), config =>
			//				config.WithAction(() => new TestSavedStatusUpdater()))
			//			.Move(new StateTransitionApproverObjectStatus("Published"),
			//				new StateTransitionInvalidObjectStatus(), config =>
			//					config.WithAction(
			//						() => new TestPublishedStatusUpdater()))));

			//var eventContext = new ApplyContext();
			//eventContext.Item = expandoPerson;
			//stateChanger.MoveWorkflow("testFlow", eventContext);
			////then
			//Assert.AreEqual(eventContext.Status, "Published");
			//Assert.AreEqual(expandoPerson.TestFieldSaved, "testFieldSaved");
			//Assert.AreEqual(expandoPerson.TestFieldPublished, "testFieldPublished");
	    }


	}

	class TestActionOperator : IActionOperator
	{
		private readonly int _mode;

		public TestActionOperator(int mode = 0)
		{
			_mode = mode;
		}

		public void Action(dynamic target)
		{
			if (_mode == 0)
			{
				target.Item.Family = "Петров";
			}
			else
			{
				target.Item.TestValue = "1";
			}
		}
	}

	class TestValidationOperator : IValidationOperator
	{
		private readonly bool _makeValidated;

		public TestValidationOperator(bool makeValidated)
		{
			_makeValidated = makeValidated;
		}

		public bool Validate(object validationObject, ValidationResult validationResult, string parentProperty)
		{
			return _makeValidated;
		}
	}

	class TestActionOperatorException : IActionOperator
	{
		public void Action(dynamic target)
		{
			throw new Exception();
		}
	}

	class TestValidationOperatorException : IValidationOperator
	{
		public bool Validate(object validationObject, ValidationResult validationResult, string parentProperty)
		{
			throw new Exception();
		}
	}
}
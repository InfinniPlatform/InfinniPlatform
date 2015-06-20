using System;
using InfinniPlatform.Metadata.StateMachine.ActionUnits.ActionOperatorBuilders;
using InfinniPlatform.Metadata.StateMachine.Statuses;
using InfinniPlatform.Sdk.Application.Dynamic;
using NUnit.Framework;

namespace InfinniPlatform.Metadata.Tests
{
    [TestFixture]
    [Category(TestCategories.UnitTest)]
    public sealed class StateWorkflowBehavior
    {
        [TestCase(false)]
        [TestCase(true)]
        public void ShouldInvokeTransitionsForWorkflow(bool runWithFlow)
        {
            dynamic target = new DynamicWrapper();


            Action<dynamic> actionFlow = (context) => { target.ActionFlowExecuted = "Executed"; };

            Action<dynamic> actionWithoutFlow = context => { target.ActionWithoutFlowExecuted = "Executed"; };

            var workflowConfig = new StateWorkflowStartingPointConfig();
            workflowConfig
                .ForState("Temporary",
                          st =>
                          st.Move(
                              transition =>
                              transition.WithAction(() => new ActionOperator("TemporaryAction", actionFlow))))
                .FlowWithoutState(
                    st => st.Move(tr => tr.WithAction(() => new ActionOperator("ActionWithoutFlow", actionWithoutFlow))));

            if (runWithFlow)
            {
                workflowConfig.MoveWorkflow(target, "Temporary");
                Assert.AreEqual(target.ActionFlowExecuted, "Executed");
                Assert.AreEqual(target.ActionWithoutFlowExecuted, null);
            }

            else
            {
                workflowConfig.MoveWorkflow(target, null);
                Assert.AreEqual(target.ActionFlowExecuted, null);
                Assert.AreEqual(target.ActionWithoutFlowExecuted, "Executed");
            }
        }
    }
}
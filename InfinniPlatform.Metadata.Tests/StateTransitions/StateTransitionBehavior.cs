using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using InfinniPlatform.Api.ContextTypes.ContextImpl;
using InfinniPlatform.Api.Dynamic;
using InfinniPlatform.Metadata.StateMachine.ActionUnits.ActionOperatorBuilders;
using InfinniPlatform.Metadata.StateMachine.Statuses;
using InfinniPlatform.Metadata.StateTransitions.StateTransitionApprovers;
using InfinniPlatform.Metadata.StateTransitions.StateTransitionInvalid;
using NUnit.Framework;

namespace InfinniPlatform.Metadata.Tests.StateTransitions
{
    [TestFixture]
	[Category(TestCategories.UnitTest)]
    public sealed class StateTransitionBehavior
    {
        [Test]
        public void ShouldApproveState()
        {
            var target = new ApplyContext();

            var stateTransition = new StateTransition(
                new StateTransitionApproverObjectStatus("Saved"),
                new StateTransitionInvalidObjectStatus(),
                null, null, null,null,null,null,null,null
                );
            stateTransition.ApplyTransition(target);

            Assert.AreEqual(target.IsValid,true);
            Assert.AreEqual(target.ValidationMessage.ValidationErrors.IsValid,true);
            Assert.AreEqual(target.ValidationMessage.ValidationErrors.Message, string.Empty);
            Assert.AreEqual(target.ValidationMessage.ValidationWarnings.IsValid, true);
            Assert.AreEqual(target.ValidationMessage.ValidationWarnings.Message, string.Empty);

            Assert.AreEqual(target.Status,"Saved");
        }

        [Test]
        public void ShouldNotApproveState()
        {
            var target = new ApplyContext();

            var stateTransition = new StateTransition(
                new StateTransitionApproverObjectStatus("Saved"),
                new StateTransitionInvalidObjectStatus(),
                new []
                    {
                        new ActionOperator((item) =>
                            {
                                throw new Exception("fail message");
                            }), 
                    }, null, null, null, null, null, null, null
                );
            stateTransition.ApplyTransition(target);

            Assert.AreEqual(target.IsValid, false);
            Assert.AreEqual(target.ValidationMessage.ValidationErrors.IsValid, false);
            Assert.True(target.ValidationMessage.ValidationErrors.Message.Contains("fail message"));
            Assert.IsNull(target.ValidationMessage.ValidationWarnings);
            Assert.AreEqual(target.Status, "Invalid");

        }

        [Test]
        public void ShouldSetInvalidStatusOnSuccessActionFail()
        {
            var target = new ApplyContext();

            var stateTransition = new StateTransition(
                new StateTransitionApproverObjectStatus("Saved"),
                new StateTransitionInvalidObjectStatus(),
                    null, null, null, null, null, new ActionOperator((item) =>
                            {
                                throw new Exception("fail message");
                            }), null, null
                );
            stateTransition.ApplyTransition(target);

            Assert.AreEqual(target.IsValid, false);
            Assert.AreEqual(target.ValidationMessage.ValidationErrors.IsValid, false);
            Assert.True(target.ValidationMessage.ValidationErrors.Message.Contains("fail message"));
            Assert.IsNull(target.ValidationMessage.ValidationWarnings);
            Assert.AreEqual(target.Status, "Invalid");
        }

        [Test]
        public void ShouldSetInvalidStatusOnFailActionFail()
        {
            var target = new ApplyContext();

            var stateTransition = new StateTransition(
                new StateTransitionApproverObjectStatus("Saved"),
                new StateTransitionInvalidObjectStatus(),
                    null, null, null, null, null, null, new ActionOperator((item) =>
                    {
                        throw new Exception("fail message");
                    }), null
                );
            stateTransition.ApplyTransition(target);

            Assert.AreEqual(target.IsValid, false);
            Assert.AreEqual(target.ValidationMessage.ValidationErrors.IsValid, false);
            Assert.True(target.ValidationMessage.ValidationErrors.Message.Contains("fail message"));
            Assert.IsNull(target.ValidationMessage.ValidationWarnings);
            Assert.AreEqual(target.Status, "Invalid");
        }

    }
}

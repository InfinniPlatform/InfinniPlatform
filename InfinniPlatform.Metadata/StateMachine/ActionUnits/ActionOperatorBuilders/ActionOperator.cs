using System;
using InfinniPlatform.Api.Actions;

namespace InfinniPlatform.Metadata.StateMachine.ActionUnits.ActionOperatorBuilders
{
    public sealed class ActionOperator : IActionOperator
    {
        private readonly Action<dynamic> _actionToExecute;

        public ActionOperator(string description, Action<dynamic> action)
        {
            _actionToExecute = action;
            Description = description;
        }

        public string Description { get; private set; }

        public void Action(dynamic target)
        {
            if (_actionToExecute != null)
            {
                _actionToExecute.Invoke(target);
            }
        }
    }
}
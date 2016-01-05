using System;
using System.Collections.Generic;
using System.Linq;

using InfinniPlatform.Sdk.Environment.Worklow;

namespace InfinniPlatform.SystemConfig.StateMachine.Statuses
{
    public sealed class StateWorkflow : IStateWorkflow
    {
        private readonly List<StateTransition> _stateTransitions;
        private readonly object _target;

        public StateWorkflow(IEnumerable<StateTransition> stateTransitions, object target)
        {
            _target = target;
            _stateTransitions = stateTransitions.ToList();
        }

        public bool Move()
        {
            if (_stateTransitions == null)
            {
                throw new ArgumentException("Transition list should be specified");
            }

            foreach (var stateTransition in _stateTransitions)
            {
                var applied = stateTransition.ApplyTransition(_target);
                if (!applied)
                {
                    return false;
                }
            }
            return true;
        }
    }
}
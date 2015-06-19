using System;
using System.Collections.Generic;
using InfinniPlatform.Api.Worklow;

namespace InfinniPlatform.Metadata.StateMachine.Statuses
{
    /// <summary>
    ///     Конфигурация потока
    /// </summary>
    public sealed class StateWorkflowConfig : IStateWorkflowConfig
    {
        private readonly IStateTransitionCondition _stateTransitionCondition;
        private readonly IList<StateTransitionConfig> _stateTransitionConfigs = new List<StateTransitionConfig>();

        public StateWorkflowConfig(IStateTransitionCondition stateTransitionCondition)
        {
            _stateTransitionCondition = stateTransitionCondition;
        }

        /// <summary>
        ///     Перевести объект с использованием указанных параметров
        /// </summary>
        /// <param name="stateTransitionConfig"></param>
        /// <returns></returns>
        public IStateWorkflowConfig Move(Action<IStateTransitionConfig> stateTransitionConfig = null)
        {
            var transitionConfig = new StateTransitionConfig();
            _stateTransitionConfigs.Add(transitionConfig);

            if (stateTransitionConfig != null)
            {
                stateTransitionConfig.Invoke(transitionConfig);
            }

            return this;
        }

        /// <summary>
        ///     Создать поток для указанного объекта
        /// </summary>
        /// <param name="target"></param>
        /// <returns></returns>
        internal StateWorkflow BuildWorkflow(dynamic target)
        {
            var listTransitions = new List<StateTransition>();
            foreach (var stateTransitionConfig in _stateTransitionConfigs)
            {
                var transition = stateTransitionConfig.BuildStateTransition();
                listTransitions.Add(transition);
            }
            if (_stateTransitionCondition == null)
            {
                throw new ArgumentException("workflow starting point not found");
            }

            return new StateWorkflow(listTransitions, target);
        }

        internal bool CanMoveFrom(object state)
        {
            return _stateTransitionCondition.CanApplyFor(state);
        }
    }
}
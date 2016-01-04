using System;
using System.Collections.Generic;
using System.Linq;

using InfinniPlatform.Sdk.Environment.Worklow;
using InfinniPlatform.SystemConfig.Metadata.StateMachine.Statuses.StateTransitionConditions;

namespace InfinniPlatform.SystemConfig.Metadata.StateMachine.Statuses
{
    /// <summary>
    ///     Конфигурация стартовой точки описания переходов между состояниями
    /// </summary>
    public class StateWorkflowStartingPointConfig : IStateWorkflowStartingPointConfig
    {
        private readonly IList<StateWorkflowConfig> _workflowConfigs = new List<StateWorkflowConfig>();

        public IStateWorkflowStartingPointConfig ForState(object status,
            Action<IStateWorkflowConfig> workflowConfigInitializer = null)
        {
            if (status == null)
            {
                throw new ArgumentException("workflow state cannot be null");
            }

            var workflowConfig = new StateWorkflowConfig(new StateTransitionConditionObjectStatus(status));

            if (workflowConfigInitializer != null)
            {
                workflowConfigInitializer(workflowConfig);
            }

            _workflowConfigs.Add(workflowConfig);
            return this;
        }

        /// <summary>
        ///     Зарегистрировать поток без осуществления перехода в какое-либо состояние
        /// </summary>
        /// <param name="workflowConfigInitializer">Инициализатор потока</param>
        /// <returns></returns>
        public IStateWorkflowStartingPointConfig FlowWithoutState(
            Action<IStateWorkflowConfig> workflowConfigInitializer = null)
        {
            var workflowConfig = new StateWorkflowConfig(new StateTransitionConditionNone());
            if (workflowConfigInitializer != null)
            {
                workflowConfigInitializer(workflowConfig);
            }
            _workflowConfigs.Add(workflowConfig);
            return this;
        }

        /// <summary>
        ///     Выполнить перевод объекта из указанного состояния
        /// </summary>
        /// <param name="target">Объект, над которым осуществляется переход</param>
        /// <param name="state">Состояние объекта, из которого выполняется переход</param>
        /// <returns>результат перехода</returns>
        public dynamic MoveWorkflow(dynamic target, object state)
        {
            var configs = _workflowConfigs.Where(wf => wf.CanMoveFrom(state)).ToList();
            if (configs.Count == 0)
            {
                configs.Add(_workflowConfigs.FirstOrDefault());
            }

            configs.ToList().ForEach(f => f.BuildWorkflow(target).Move());
            return target;
        }
    }
}
using System;
using System.Collections.Generic;

namespace InfinniPlatform.SystemConfig.StateMachine
{
    /// <summary>
    ///     Реестр переходов между состояниями объектов
    /// </summary>
    public sealed class StateWorkflowRegister : IStateWorkflowRegister
    {
        private readonly Dictionary<string, StateWorkflowStartingPointConfig> _flows =
            new Dictionary<string, StateWorkflowStartingPointConfig>();

        /// <summary>
        ///     Зарегистрировать описание перехода из состояния в состояние
        /// </summary>
        /// <param name="flowName">Наименование перехода</param>
        /// <param name="workflowInitializer">Инициализатор потока перехода</param>
        public void DefineWorkflow(string flowName, Action<IStateWorkflowStartingPointConfig> workflowInitializer)
        {
            var workflowStartingPoint = new StateWorkflowStartingPointConfig();
            if (workflowInitializer != null)
            {
                workflowInitializer(workflowStartingPoint);
            }

            if (_flows.ContainsKey(flowName.ToLowerInvariant()))
            {
                _flows.Remove(flowName.ToLowerInvariant());
            }
            _flows.Add(flowName.ToLowerInvariant(), workflowStartingPoint);
        }

        /// <summary>
        ///     Перевести объект из указанного состояния
        /// </summary>
        /// <param name="flowname">Наименование потока выполнения</param>
        /// <param name="target">Объект для перехода в состояние</param>
        /// <param name="state">Состояние, из которого выполняется переход</param>
        /// <returns>Результирующий объект перехода</returns>
        public dynamic MoveWorkflow(string flowname, dynamic target, dynamic state = null)
        {
            //если не зарегистрированы метаданные точки расширения
            if (string.IsNullOrEmpty(flowname))
            {
                return target;
            }

            StateWorkflowStartingPointConfig stateWorkflowStartingPointConfig;

            if (_flows.TryGetValue(flowname.ToLowerInvariant(), out stateWorkflowStartingPointConfig))
            {
                return stateWorkflowStartingPointConfig.MoveWorkflow(target, state);
            }
            return target;
        }
    }
}
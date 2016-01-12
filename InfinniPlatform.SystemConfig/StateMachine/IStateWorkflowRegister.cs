using System;

namespace InfinniPlatform.SystemConfig.StateMachine
{
    public interface IStateWorkflowRegister
    {
        void DefineWorkflow(string flowName, Action<IStateWorkflowStartingPointConfig> workflowInitializer);
        dynamic MoveWorkflow(string flowname, dynamic target, dynamic state);
    }
}
using System;

namespace InfinniPlatform.Sdk.Environment.Worklow
{
    public interface IStateWorkflowRegister
    {
        void DefineWorkflow(string flowName, Action<IStateWorkflowStartingPointConfig> workflowInitializer);
        dynamic MoveWorkflow(string flowname, dynamic target, dynamic state);
    }
}
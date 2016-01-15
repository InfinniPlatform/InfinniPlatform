using InfinniPlatform.Core.Tests.RestBehavior.Acceptance;
using InfinniPlatform.Sdk.Contracts;
using InfinniPlatform.Sdk.Registers;

namespace InfinniPlatform.Core.Tests.RestBehavior.TestActions
{
    public sealed class ActionUnitDeleteRegisterEntriesAction
    {
        private readonly IRegisterApi _registryComponent;

        public ActionUnitDeleteRegisterEntriesAction(IRegisterApi registryComponent)
        {
            _registryComponent = registryComponent;
        }

        public void Action(IApplyContext target)
        {
            // После удаления документа, удаляем соответствующие записи в регистрах
            _registryComponent.DeleteEntry(target.Configuration, RegistersBehavior.AvailableBedsRegister, target.Item);
            _registryComponent.DeleteEntry(target.Configuration, RegistersBehavior.InfoRegister, target.Item);
        }
    }
}
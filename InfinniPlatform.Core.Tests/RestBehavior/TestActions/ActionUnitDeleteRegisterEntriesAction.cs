using InfinniPlatform.Core.Tests.RestBehavior.Acceptance;
using InfinniPlatform.Sdk.ContextComponents;
using InfinniPlatform.Sdk.Contracts;

namespace InfinniPlatform.Core.Tests.RestBehavior.TestActions
{
    public sealed class ActionUnitDeleteRegisterEntriesAction
    {
        private readonly IRegistryComponent _registryComponent;

        public ActionUnitDeleteRegisterEntriesAction(IRegistryComponent registryComponent)
        {
            _registryComponent = registryComponent;
        }

        public void Action(IApplyContext target)
        {
            // После удаления документа, удаляем соответствующие записи в регистрах
            _registryComponent.DeleteRegisterEntry(target.Configuration, RegistersBehavior.AvailableBedsRegister, target.Item);
            _registryComponent.DeleteRegisterEntry(target.Configuration, RegistersBehavior.InfoRegister, target.Item);
        }
    }
}
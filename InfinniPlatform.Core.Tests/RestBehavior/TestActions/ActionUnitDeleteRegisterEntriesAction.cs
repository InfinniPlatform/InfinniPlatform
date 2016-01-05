using InfinniPlatform.Core.Tests.RestBehavior.Acceptance;
using InfinniPlatform.Sdk.ContextComponents;
using InfinniPlatform.Sdk.Contracts;

namespace InfinniPlatform.Core.Tests.RestBehavior.TestActions
{
    public sealed class ActionUnitDeleteRegisterEntriesAction
    {
        public void Action(IApplyContext target)
        {
            // После удаления документа, удаляем соответствующие записи в регистрах
            var registryComponent = target.Context.GetComponent<IRegistryComponent>();
            registryComponent.DeleteRegisterEntry(target.Configuration, RegistersBehavior.AvailableBedsRegister, target.Item);
            registryComponent.DeleteRegisterEntry(target.Configuration, RegistersBehavior.InfoRegister, target.Item);
        }
    }
}
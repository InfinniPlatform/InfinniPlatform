using InfinniPlatform.Api.Tests.RestBehavior.Acceptance;
using InfinniPlatform.Sdk.ContextComponents;
using InfinniPlatform.Sdk.Contracts;

namespace InfinniPlatform.Api.Tests.RestBehavior.TestActions
{
    public sealed class ActionUnitDeleteRegisterEntriesAction
    {
        public void Action(IApplyContext target)
        {
            // После удаления документа, удаляем соответствующие записи в регистрах
            var registryComponent = target.Context.GetComponent<IRegistryComponent>();
            registryComponent.DeleteRegisterEntry(target.Item.Configuration, RegistersBehavior.AvailableBedsRegister, target.Item.Id);
            registryComponent.DeleteRegisterEntry(target.Item.Configuration, RegistersBehavior.InfoRegister, target.Item.Id);
        }
    }
}
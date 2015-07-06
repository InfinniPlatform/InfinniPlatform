using InfinniPlatform.Sdk.ContextComponents;
using InfinniPlatform.Sdk.Contracts;

namespace InfinniPlatform.Api.Tests.RestBehavior.TestActions
{
    public sealed class DeleteRegisterEntriesAction
    {
        public void Action(IApplyContext target)
        {
            // После удаления документа, удаляем соответсвующие записи в регистрах
            target.Context.GetComponent<IRegistryComponent>()
                  .DeleteRegisterEntry(target.Item.Configuration, "availablebeds", target.Item.Id);
            target.Context.GetComponent<IRegistryComponent>()
                  .DeleteRegisterEntry(target.Item.Configuration, "inforegister", target.Item.Id);
        }
    }
}
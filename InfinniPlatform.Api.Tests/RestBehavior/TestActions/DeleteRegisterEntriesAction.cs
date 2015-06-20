using InfinniPlatform.Api.ContextComponents;
using InfinniPlatform.Sdk.Application.Contracts;

namespace InfinniPlatform.Api.Tests.RestBehavior.TestActions
{
    public sealed class DeleteRegisterEntriesAction
    {
        public void Action(IApplyContext target)
        {
            // После удаления документа, удаляем соответсвующие записи в регистрах
            target.Context.GetComponent<IRegistryComponent>(target.Version)
                  .DeleteRegisterEntry(target.Item.Configuration, "availablebeds", target.Item.Id);
            target.Context.GetComponent<IRegistryComponent>(target.Version)
                  .DeleteRegisterEntry(target.Item.Configuration, "inforegister", target.Item.Id);
        }
    }
}
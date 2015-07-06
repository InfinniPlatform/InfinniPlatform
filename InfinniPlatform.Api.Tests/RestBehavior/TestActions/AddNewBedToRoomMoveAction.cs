using InfinniPlatform.Api.Registers;
using InfinniPlatform.Sdk.ContextComponents;
using InfinniPlatform.Sdk.Contracts;

namespace InfinniPlatform.Api.Tests.RestBehavior.TestActions
{
    public sealed class AddNewBedToRoomMoveAction
    {
        public void Action(IApplyContext target)
        {
            if (target.Item.Info == null)
            {
                // Койка освободилась - income

                var entry =
                    target.Context.GetComponent<IRegistryComponent>()
                          .CreateAccumulationRegisterEntry(target.Item.Configuration, "availablebeds",
                                                           target.Item.Metadata,
                                                           target.Item, target.Item.Date);

                entry.EntryType = RegisterEntryType.Income;
                entry.Room = target.Item.Room;
                entry.Bed = target.Item.Bed;
                entry.Value = 1; // Изменение количества на единицу

                target.Context.GetComponent<IRegistryComponent>()
                      .PostRegisterEntries(target.Item.Configuration, "availablebeds", new[] {entry});
            }
            else
            {
                // Добавляем данные в регистр сведений

                var infoEntry =
                    target.Context.GetComponent<IRegistryComponent>()
                          .CreateInfoRegisterEntry(target.Item.Configuration, "inforegister",
                                                   target.Item.Metadata, target.Item, target.Item.Date);

                infoEntry.EntryType = RegisterEntryType.Income;
                infoEntry.Room = target.Item.Room;
                infoEntry.Bed = target.Item.Bed;
                infoEntry.Value = 1; // Изменение количества на единицу

                target.Context.GetComponent<IRegistryComponent>()
                      .PostRegisterEntries(target.Item.Configuration, "inforegister", new[] {infoEntry});
            }
        }
    }
}
using InfinniPlatform.Api.ContextComponents;
using InfinniPlatform.Api.Registers;
using InfinniPlatform.Sdk.Contracts;

namespace InfinniPlatform.Api.Tests.RestBehavior.TestActions
{
    public sealed class HospitalRegisterMoveAction
    {
        public void Action(IApplyContext target)
        {
            dynamic incomeEntry = null;
            dynamic consumptionEntry = null;

            if (target.Item.OldRoom != null &&
                target.Item.OldBed != null)
            {
                incomeEntry =
                    target.Context.GetComponent<IRegistryComponent>(target.Version)
                          .CreateAccumulationRegisterEntry(target.Item.Configuration, "availablebeds",
                                                           target.Item.Metadata, target.Item, target.Item.Date);
                incomeEntry.Value = 1; // Изменение количества на единицу

                // Койка освободилась - income
                incomeEntry.EntryType = RegisterEntryType.Income;
                incomeEntry.Room = target.Item.OldRoom;
                incomeEntry.Bed = target.Item.OldBed;
            }

            if (target.Item.NewRoom != null &&
                target.Item.NewBed != null)
            {
                consumptionEntry =
                    target.Context.GetComponent<IRegistryComponent>(target.Version)
                          .CreateAccumulationRegisterEntry(target.Item.Configuration, "availablebeds",
                                                           target.Item.Metadata, target.Item, target.Item.Date);
                consumptionEntry.Value = 1; // Изменение количества на единицу

                // Койку заняли - consumption
                consumptionEntry.EntryType = RegisterEntryType.Consumption;
                consumptionEntry.Room = target.Item.NewRoom;
                consumptionEntry.Bed = target.Item.NewBed;
            }

            if (incomeEntry != null)
            {
                target.Context.GetComponent<IRegistryComponent>(target.Version)
                      .PostRegisterEntries(target.Item.Configuration, "availablebeds", new[] {incomeEntry});
            }

            if (consumptionEntry != null)
            {
                target.Context.GetComponent<IRegistryComponent>(target.Version)
                      .PostRegisterEntries(target.Item.Configuration, "availablebeds", new[] {consumptionEntry});
            }
        }
    }
}
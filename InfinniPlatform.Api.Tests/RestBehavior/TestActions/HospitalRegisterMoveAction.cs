using InfinniPlatform.Api.Tests.RestBehavior.Acceptance;
using InfinniPlatform.Sdk.ContextComponents;
using InfinniPlatform.Sdk.Contracts;
using InfinniPlatform.Sdk.Environment.Register;

namespace InfinniPlatform.Api.Tests.RestBehavior.TestActions
{
    public sealed class HospitalRegisterMoveAction
    {
        public void Action(IApplyContext target)
        {
            dynamic incomeEntry = null;
            dynamic consumptionEntry = null;

            var registryComponent = target.Context.GetComponent<IRegistryComponent>();

            if (target.Item.OldRoom != null && target.Item.OldBed != null)
            {
                incomeEntry = registryComponent.CreateAccumulationRegisterEntry(target.Item.Configuration, RegistersBehavior.AvailableBedsRegister, target.Item.Metadata, target.Item, target.Item.Date);
                incomeEntry.Value = 1; // Изменение количества на единицу

                // Койка освободилась - income
                incomeEntry.EntryType = RegisterEntryType.Income;
                incomeEntry.Room = target.Item.OldRoom;
                incomeEntry.Bed = target.Item.OldBed;
            }

            if (target.Item.NewRoom != null && target.Item.NewBed != null)
            {
                consumptionEntry = registryComponent.CreateAccumulationRegisterEntry(target.Item.Configuration, RegistersBehavior.AvailableBedsRegister, target.Item.Metadata, target.Item, target.Item.Date);
                consumptionEntry.Value = 1; // Изменение количества на единицу

                // Койку заняли - consumption
                consumptionEntry.EntryType = RegisterEntryType.Consumption;
                consumptionEntry.Room = target.Item.NewRoom;
                consumptionEntry.Bed = target.Item.NewBed;
            }

            if (incomeEntry != null)
            {
                registryComponent.PostRegisterEntries(target.Item.Configuration, RegistersBehavior.AvailableBedsRegister, new[] { incomeEntry });
            }

            if (consumptionEntry != null)
            {
                registryComponent.PostRegisterEntries(target.Item.Configuration, RegistersBehavior.AvailableBedsRegister, new[] { consumptionEntry });
            }
        }
    }
}
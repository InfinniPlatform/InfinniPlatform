using InfinniPlatform.Core.Registers;
using InfinniPlatform.Core.Tests.RestBehavior.Acceptance;
using InfinniPlatform.Sdk.Contracts;
using InfinniPlatform.Sdk.Registers;

namespace InfinniPlatform.Core.Tests.RestBehavior.TestActions
{
    public sealed class ActionUnitHospitalRegisterMoveAction
    {
        public ActionUnitHospitalRegisterMoveAction(IRegistryComponent registryComponent)
        {
            _registryComponent = registryComponent;
        }

        private readonly IRegistryComponent _registryComponent;

        public void Action(IApplyContext target)
        {
            dynamic incomeEntry = null;
            dynamic consumptionEntry = null;

            if (target.Item.OldRoom != null && target.Item.OldBed != null)
            {
                incomeEntry = _registryComponent.CreateAccumulationRegisterEntry(target.Configuration, RegistersBehavior.AvailableBedsRegister, target.Item.Metadata, target.Item, target.Item.Date);
                incomeEntry.Value = 1; // Изменение количества на единицу

                // Койка освободилась - income
                incomeEntry.EntryType = RegisterEntryType.Income;
                incomeEntry.Room = target.Item.OldRoom;
                incomeEntry.Bed = target.Item.OldBed;
            }

            if (target.Item.NewRoom != null && target.Item.NewBed != null)
            {
                consumptionEntry = _registryComponent.CreateAccumulationRegisterEntry(target.Configuration, RegistersBehavior.AvailableBedsRegister, target.Item.Metadata, target.Item, target.Item.Date);
                consumptionEntry.Value = 1; // Изменение количества на единицу

                // Койку заняли - consumption
                consumptionEntry.EntryType = RegisterEntryType.Consumption;
                consumptionEntry.Room = target.Item.NewRoom;
                consumptionEntry.Bed = target.Item.NewBed;
            }

            if (incomeEntry != null)
            {
                _registryComponent.PostRegisterEntries(target.Configuration, RegistersBehavior.AvailableBedsRegister, new[] { incomeEntry });
            }

            if (consumptionEntry != null)
            {
                _registryComponent.PostRegisterEntries(target.Configuration, RegistersBehavior.AvailableBedsRegister, new[] { consumptionEntry });
            }
        }
    }
}
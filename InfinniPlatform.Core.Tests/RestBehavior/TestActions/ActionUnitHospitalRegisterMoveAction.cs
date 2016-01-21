using System;

using InfinniPlatform.Core.Tests.RestBehavior.Registers;
using InfinniPlatform.Sdk.Contracts;
using InfinniPlatform.Sdk.Registers;

namespace InfinniPlatform.Core.Tests.RestBehavior.TestActions
{
    public sealed class ActionUnitHospitalRegisterMoveAction
    {
        public ActionUnitHospitalRegisterMoveAction(IRegisterApi registerApi)
        {
            _registerApi = registerApi;
        }

        private readonly IRegisterApi _registerApi;

        public void Action(IActionContext target)
        {
            dynamic incomeEntry = null;
            dynamic consumptionEntry = null;

            if (target.Item.OldRoom != null && target.Item.OldBed != null)
            {
                incomeEntry = _registerApi.CreateEntry(target.Configuration, RegisterApiAcceptanceTest.AvailableBedsRegister, target.Item.Metadata, (DateTime?)target.Item.Date, (object)target.Item, false);
                incomeEntry.Value = 1; // Изменение количества на единицу

                // Койка освободилась - income
                incomeEntry.EntryType = RegisterEntryType.Income;
                incomeEntry.Room = target.Item.OldRoom;
                incomeEntry.Bed = target.Item.OldBed;
            }

            if (target.Item.NewRoom != null && target.Item.NewBed != null)
            {
                consumptionEntry = _registerApi.CreateEntry(target.Configuration, RegisterApiAcceptanceTest.AvailableBedsRegister, target.Item.Metadata, (DateTime?)target.Item.Date, (object)target.Item, false);
                consumptionEntry.Value = 1; // Изменение количества на единицу

                // Койку заняли - consumption
                consumptionEntry.EntryType = RegisterEntryType.Consumption;
                consumptionEntry.Room = target.Item.NewRoom;
                consumptionEntry.Bed = target.Item.NewBed;
            }

            if (incomeEntry != null)
            {
                _registerApi.PostEntries(target.Configuration, RegisterApiAcceptanceTest.AvailableBedsRegister, new[] { incomeEntry });
            }

            if (consumptionEntry != null)
            {
                _registerApi.PostEntries(target.Configuration, RegisterApiAcceptanceTest.AvailableBedsRegister, new[] { consumptionEntry });
            }
        }
    }
}
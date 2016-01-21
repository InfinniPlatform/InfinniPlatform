using System;

using InfinniPlatform.Core.Tests.RestBehavior.Registers;
using InfinniPlatform.Sdk.Contracts;
using InfinniPlatform.Sdk.Registers;

namespace InfinniPlatform.Core.Tests.RestBehavior.TestActions
{
    public sealed class ActionUnitAddNewBedToRoomMoveAction
    {
        public ActionUnitAddNewBedToRoomMoveAction(IRegisterApi registerApi)
        {
            _registerApi = registerApi;
        }

        private readonly IRegisterApi _registerApi;

        public void Action(IActionContext target)
        {
            bool isInfoRegister = (target.Item.Info != null);
            string registerName = isInfoRegister ? RegisterApiAcceptanceTest.InfoRegister : RegisterApiAcceptanceTest.AvailableBedsRegister;

            CreateRegisterEntry(target, registerName,
                r => r.CreateEntry(
                    target.Configuration,
                    RegisterApiAcceptanceTest.AvailableBedsRegister,
                    target.DocumentType,
                    (DateTime?)target.Item.Date,
                    (object)target.Item,
                    isInfoRegister));
        }

        private void CreateRegisterEntry(IActionContext target, string registerName, Func<IRegisterApi, dynamic> createEntry)
        {
            var registerEntry = createEntry(_registerApi);
            registerEntry.EntryType = RegisterEntryType.Income;
            registerEntry.Info = target.Item.Info;
            registerEntry.Room = target.Item.Room;
            registerEntry.Bed = target.Item.Bed;
            registerEntry.Value = 1; // Изменение количества на единицу

            _registerApi.PostEntries(target.Configuration, registerName, new[] { registerEntry });
        }
    }
}
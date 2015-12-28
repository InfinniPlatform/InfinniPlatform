using System;

using InfinniPlatform.Api.Tests.RestBehavior.Acceptance;
using InfinniPlatform.Sdk.ContextComponents;
using InfinniPlatform.Sdk.Contracts;
using InfinniPlatform.Sdk.Environment.Register;

namespace InfinniPlatform.Api.Tests.RestBehavior.TestActions
{
    public sealed class ActionUnitAddNewBedToRoomMoveAction
    {
        public ActionUnitAddNewBedToRoomMoveAction(IRegistryComponent registryComponent)
        {
            _registryComponent = registryComponent;
        }

        private readonly IRegistryComponent _registryComponent;

        public void Action(IApplyContext target)
        {
            if (target.Item.Info == null)
            {
                CreateRegisterEntry(target, RegistersBehavior.AvailableBedsRegister,
                    r => r.CreateAccumulationRegisterEntry(
                        target.Configuration,
                        RegistersBehavior.AvailableBedsRegister,
                        target.Metadata,
                        target.Item,
                        target.Item.Date));
            }
            else
            {
                CreateRegisterEntry(target, RegistersBehavior.InfoRegister,
                    r => r.CreateInfoRegisterEntry(
                        target.Configuration,
                        RegistersBehavior.InfoRegister,
                        target.Metadata,
                        target.Item,
                        target.Item.Date));
            }
        }

        private void CreateRegisterEntry(IApplyContext target, string registerName, Func<IRegistryComponent, dynamic> createEntry)
        {
            var registerEntry = createEntry(_registryComponent);
            registerEntry.EntryType = RegisterEntryType.Income;
            registerEntry.Info = target.Item.Info;
            registerEntry.Room = target.Item.Room;
            registerEntry.Bed = target.Item.Bed;
            registerEntry.Value = 1; // Изменение количества на единицу

            _registryComponent.PostRegisterEntries(target.Configuration, registerName, new[] { registerEntry });
        }
    }
}
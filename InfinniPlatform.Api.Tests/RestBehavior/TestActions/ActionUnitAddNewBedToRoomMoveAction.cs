using System;

using InfinniPlatform.Api.Tests.RestBehavior.Acceptance;
using InfinniPlatform.Sdk.ContextComponents;
using InfinniPlatform.Sdk.Contracts;
using InfinniPlatform.Sdk.Environment.Register;

namespace InfinniPlatform.Api.Tests.RestBehavior.TestActions
{
    public sealed class ActionUnitAddNewBedToRoomMoveAction
    {
        public void Action(IApplyContext target)
        {
            if (target.Item.Info == null)
            {
                CreateRegisterEntry(target, RegistersBehavior.AvailableBedsRegister,
                    r => r.CreateAccumulationRegisterEntry(
                        target.Item.Configuration,
                        RegistersBehavior.AvailableBedsRegister,
                        target.Item.Metadata,
                        target.Item,
                        target.Item.Date));
            }
            else
            {
                CreateRegisterEntry(target, RegistersBehavior.InfoRegister,
                    r => r.CreateInfoRegisterEntry(
                        target.Item.Configuration,
                        RegistersBehavior.InfoRegister,
                        target.Item.Metadata,
                        target.Item,
                        target.Item.Date));
            }
        }

        private static void CreateRegisterEntry(IApplyContext target, string registerName, Func<IRegistryComponent, dynamic> createEntry)
        {
            var registryComponent = target.Context.GetComponent<IRegistryComponent>();

            var registerEntry = createEntry(registryComponent);
            registerEntry.EntryType = RegisterEntryType.Income;
            registerEntry.Info = target.Item.Info;
            registerEntry.Room = target.Item.Room;
            registerEntry.Bed = target.Item.Bed;
            registerEntry.Value = 1; // Изменение количества на единицу

            registryComponent.PostRegisterEntries(target.Item.Configuration, registerName, new[] { registerEntry });
        }
    }
}
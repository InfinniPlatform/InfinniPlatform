﻿using InfinniPlatform.Core.Tests.RestBehavior.Acceptance;
using InfinniPlatform.Core.Tests.RestBehavior.Registers;
using InfinniPlatform.Sdk.Contracts;
using InfinniPlatform.Sdk.Registers;

namespace InfinniPlatform.Core.Tests.RestBehavior.TestActions
{
    public sealed class ActionUnitDeleteRegisterEntriesAction
    {
        private readonly IRegisterApi _registryComponent;

        public ActionUnitDeleteRegisterEntriesAction(IRegisterApi registryComponent)
        {
            _registryComponent = registryComponent;
        }

        public void Action(IActionContext target)
        {
            // После удаления документа, удаляем соответствующие записи в регистрах
            _registryComponent.DeleteEntry(RegisterApiAcceptanceTest.AvailableBedsRegister, target.Item);
            _registryComponent.DeleteEntry(RegisterApiAcceptanceTest.InfoRegister, target.Item);
        }
    }
}
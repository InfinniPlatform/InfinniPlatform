﻿using InfinniPlatform.Sdk.Application.Contracts;

namespace InfinniPlatform.Api.Tests.RestBehavior.TestActions
{
    public sealed class TestComplexValidator
    {
        public void Validate(IApplyContext target)
        {
            target.IsValid = false;
            target.ValidationMessage = "testmessage";
        }
    }
}
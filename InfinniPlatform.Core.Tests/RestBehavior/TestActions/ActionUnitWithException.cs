using System;

using InfinniPlatform.Sdk.Contracts;

namespace InfinniPlatform.Core.Tests.RestBehavior.TestActions
{
    public sealed class ActionUnitWithException
    {
        public void Action(IApplyContext target)
        {
            ThrowOne();
        }

        private static void ThrowOne()
        {
            ThrowTwo();
        }

        private static void ThrowTwo()
        {
            ThrowThree();
        }

        private static void ThrowThree()
        {
            throw new Exception("Important exception details");
        }
    }
}
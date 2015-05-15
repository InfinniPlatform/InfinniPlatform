using InfinniPlatform.Api.ContextTypes;
using System;

namespace InfinniPlatform.Api.Tests.RestBehavior.TestActions
{
    public sealed class ActionWithException
    {
        public void Action(IApplyContext target)
        {
            ThrowOne();
        }

        /// <summary>
        /// Throws Not Supported Exception.
        /// </summary>
        /// <exception cref="System.NotSupportedException"></exception>
        private static void ThrowOne()
        {
            ThrowTwo();

            throw new NotSupportedException();
        }

        /// <summary>
        /// Throws Not Implemented Exception.
        /// </summary>
        /// <exception cref="System.NotImplementedException"></exception>
        private static void ThrowTwo()
        {
            ThrowThree();

            throw new NotImplementedException();
        }

        /// <summary>
        /// Throws Argument Null Exception.
        /// </summary>
        /// <exception cref="System.ArgumentNullException"></exception>
        private static void ThrowThree()
        {
            throw new Exception("Important exception details");
        }
    }
}

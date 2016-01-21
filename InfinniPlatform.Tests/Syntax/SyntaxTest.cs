using System;

using Microsoft.CSharp.RuntimeBinder;

using NUnit.Framework;

namespace InfinniPlatform.Syntax
{
    [TestFixture]
    [Category(TestCategories.UnitTest)]
    public class SyntaxTest
    {
        [Test]
        public void MaybeOperatorWhenDynamicContext()
        {
            // Given
            var isRunningOnMono = IsRunningOnMono();

            // When

            if (isRunningOnMono)
            {
                // Then (Linux) - It does not work
                Assert.Throws<RuntimeBinderException>(() => DoSomething(new SomeClass()));
            }
            else
            {
                // Then (Windows) - It works
                Assert.IsNull(DoSomething(new SomeClass()));
            }
        }

        public string DoSomething(dynamic value)
        {
            return value.Property1?.Property2; // <-- Microsoft.CSharp.RuntimeBinder.RuntimeBinderException: Cannot perform member binding on `null' value
        }

        public static bool IsRunningOnMono()
        {
            return Type.GetType("Mono.Runtime") != null;
        }

        public class SomeClass
        {
            public SomeClass2 Property1 { get; set; }
        }

        public class SomeClass2
        {
            public string Property2 { get; set; }
        }
    }
}
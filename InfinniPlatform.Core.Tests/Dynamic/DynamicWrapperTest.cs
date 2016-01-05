using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Dynamic;

using InfinniPlatform.Sdk.Dynamic;

using NUnit.Framework;

namespace InfinniPlatform.Core.Tests.Dynamic
{
    [TestFixture]
    [Category(TestCategories.UnitTest)]
    public sealed class DynamicWrapperTest
    {
        // Clone()

        private static dynamic CreateTestEntity(object content)
        {
            dynamic result = new DynamicWrapper();
            result.Content = content;
            return result;
        }

        private class SomePrototype : DynamicWrapper
        {
            public string PrototypeField;

            public string PrototypeProperty { get; set; }

            public string PrototypeMethod(string arg)
            {
                return arg;
            }

            public event Action PrototypeEvent;
        }

        [Test]
        [Ignore("Manual")]
        [Category(TestCategories.PerformanceTest)]
        public void PerformanceGetAndSetDynamicProperty()
        {
            // Given

            const int iterations = 10000000;

            dynamic dynamicWrapper = new DynamicWrapper();
            var dynamicWrapperGetTimer = new Stopwatch();
            var dynamicWrapperSetTimer = new Stopwatch();

            dynamic expandoObject = new ExpandoObject();
            var expandoObjectGetTimer = new Stopwatch();
            var expandoObjectSetTimer = new Stopwatch();

            // When

            for (int i = 0; i < iterations; ++i)
            {
                int value = i;

                dynamicWrapperSetTimer.Start();
                dynamicWrapper.DynamicProperty = value;
                dynamicWrapperSetTimer.Stop();

                dynamicWrapperGetTimer.Start();
                value = dynamicWrapper.DynamicProperty;
                dynamicWrapperGetTimer.Stop();

                i = value;
            }

            for (int i = 0; i < iterations; ++i)
            {
                int value = i;

                expandoObjectSetTimer.Start();
                expandoObject.DynamicProperty = value;
                expandoObjectSetTimer.Stop();

                expandoObjectGetTimer.Start();
                value = expandoObject.DynamicProperty;
                expandoObjectGetTimer.Stop();

                i = value;
            }

            // Then

            double dynamicWrapperGetTime = dynamicWrapperGetTimer.Elapsed.TotalMilliseconds;
            double dynamicWrapperSetTime = dynamicWrapperSetTimer.Elapsed.TotalMilliseconds;
            double dynamicWrapperTotalTime = dynamicWrapperGetTime + dynamicWrapperSetTime;
            double expandoObjectGetTime = expandoObjectGetTimer.Elapsed.TotalMilliseconds;
            double expandoObjectSetTime = expandoObjectSetTimer.Elapsed.TotalMilliseconds;
            double expandoObjectTotalTime = expandoObjectGetTime + expandoObjectSetTime;

            Console.WriteLine("DynamicWrapper:");
            Console.WriteLine("   Get   = {0:N2} ms", dynamicWrapperGetTime);
            Console.WriteLine("   Set   = {0:N2} ms", dynamicWrapperSetTime);
            Console.WriteLine("   Total = {0:N2} ms", dynamicWrapperTotalTime);
            Console.WriteLine("ExpandoObject:");
            Console.WriteLine("   Get   = {0:N2} ms", expandoObjectGetTime);
            Console.WriteLine("   Set   = {0:N2} ms", expandoObjectSetTime);
            Console.WriteLine("   Total = {0:N2} ms", expandoObjectTotalTime);
            Console.WriteLine("DynamicWrapper vs ExpandoObject:");
            Console.WriteLine("   Get   = x{0:N2}", expandoObjectGetTime/dynamicWrapperGetTime);
            Console.WriteLine("   Set   = x{0:N2}", expandoObjectSetTime/dynamicWrapperSetTime);
            Console.WriteLine("   Total = x{0:N2}", expandoObjectTotalTime/dynamicWrapperTotalTime);

            Assert.LessOrEqual(dynamicWrapperTotalTime, expandoObjectTotalTime);
        }

        [Test]
        [Ignore("Manual")]
        [Category(TestCategories.PerformanceTest)]
        public void PerformanceInvokeDynamicProperty()
        {
            // Given

            const int iterations = 10000000;

            Func<object, object> dynamicMethod = x => x;

            dynamic dynamicWrapper = new DynamicWrapper();
            dynamicWrapper.DynamicMethod = dynamicMethod;
            var dynamicWrapperInvokeTimer = new Stopwatch();

            dynamic expandoObject = new ExpandoObject();
            expandoObject.DynamicMethod = dynamicMethod;
            var expandoObjectInvokeTimer = new Stopwatch();

            // When

            for (int i = 0; i < iterations; ++i)
            {
                dynamicWrapperInvokeTimer.Start();
                dynamicWrapper.DynamicMethod(i);
                dynamicWrapperInvokeTimer.Stop();
            }

            for (int i = 0; i < iterations; ++i)
            {
                expandoObjectInvokeTimer.Start();
                expandoObject.DynamicMethod(i);
                expandoObjectInvokeTimer.Stop();
            }

            // Then

            double dynamicWrapperInvokeTime = dynamicWrapperInvokeTimer.Elapsed.TotalMilliseconds;
            double expandoObjectInvokeTime = expandoObjectInvokeTimer.Elapsed.TotalMilliseconds;

            Console.WriteLine("DynamicWrapper:");
            Console.WriteLine("   Invoke = {0:N2} ms", dynamicWrapperInvokeTime);
            Console.WriteLine("ExpandoObject:");
            Console.WriteLine("   Invoke = {0:N2} ms", expandoObjectInvokeTime);
            Console.WriteLine("DynamicWrapper vs ExpandoObject:");
            Console.WriteLine("   Invoke = x{0:N2}", expandoObjectInvokeTime/dynamicWrapperInvokeTime);

            Assert.LessOrEqual(dynamicWrapperInvokeTime, expandoObjectInvokeTime);
        }

        [Test]
        public void SholdCloneWhenContentIsSimple()
        {
            // Given
            dynamic target = CreateTestEntity("123");

            // When
            dynamic clone = target.Clone();

            // Then
            Assert.IsFalse(target.Equals(clone));
            Assert.IsTrue(target.Content.Equals(clone.Content));
        }

        [Test]
        public void ShouldAddDynamicMethodWhenSubscribe()
        {
            // Given
            Guid expected = Guid.NewGuid();
            dynamic element = new SomePrototype();
            Func<object> subscription = () => expected;

            // When
            element.DynamicMethod += subscription;
            dynamic actual = element.DynamicMethod();

            // Then
            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void ShouldCloneWhenContentIsComplex()
        {
            // Given
            dynamic content = CreateTestEntity("123");
            dynamic target = CreateTestEntity(content);

            // When
            dynamic clone = target.Clone();

            // Then
            Assert.IsFalse(target.Equals(clone));
            Assert.IsFalse(target.Content.Equals(clone.Content));
            Assert.IsTrue(target.Content.Content.Equals(clone.Content.Content));
        }

        [Test]
        public void ShouldCloneWhenContentIsComplexCollection()
        {
            // Given
            var content = new List<object> {CreateTestEntity("1"), CreateTestEntity("2")};
            dynamic target = CreateTestEntity(content);

            // When
            dynamic clone = target.Clone();

            // Then
            Assert.IsFalse(target.Equals(clone));
            Assert.IsFalse(target.Content.Equals(clone.Content));
            Assert.IsTrue(target.Content.Count.Equals(clone.Content.Count));
            Assert.IsFalse(target.Content[0].Equals(clone.Content[0]));
            Assert.IsFalse(target.Content[1].Equals(clone.Content[1]));
            Assert.IsTrue(target.Content[0].Content.Equals(clone.Content[0].Content));
            Assert.IsTrue(target.Content[1].Content.Equals(clone.Content[1].Content));
        }

        [Test]
        public void ShouldCloneWhenContentIsSimpleCollection()
        {
            // Given
            var content = new List<string> {"1", "2", "3"};
            dynamic target = CreateTestEntity(content);

            // When
            dynamic clone = target.Clone();

            // Then
            Assert.IsFalse(target.Equals(clone));
            Assert.IsFalse(target.Content.Equals(clone.Content));
            Assert.IsTrue(target.Content.Count.Equals(clone.Content.Count));
            Assert.IsTrue(target.Content[0].Equals(clone.Content[0]));
            Assert.IsTrue(target.Content[1].Equals(clone.Content[1]));
            Assert.IsTrue(target.Content[2].Equals(clone.Content[2]));
        }

        [Test]
        public void ShouldCloneWhenCyclicReferences()
        {
            // Given
            dynamic target1 = CreateTestEntity(1);
            dynamic target2 = CreateTestEntity(2);
            dynamic target3 = CreateTestEntity(3);
            target1.Reference = target2;
            target2.Reference = target3;
            target3.Reference = target1;

            // When
            dynamic clone = target1.Clone();

            // Then
            Assert.IsFalse(clone.Equals(target1));
            Assert.IsTrue(clone.Content.Equals(target1.Content));
            Assert.IsFalse(clone.Reference.Equals(target2));
            Assert.IsTrue(clone.Reference.Content.Equals(target2.Content));
            Assert.IsFalse(clone.Reference.Reference.Equals(target3));
            Assert.IsTrue(clone.Reference.Reference.Content.Equals(target3.Content));
            Assert.IsFalse(clone.Reference.Reference.Reference.Equals(target1));
            Assert.IsTrue(clone.Reference.Reference.Reference.Content.Equals(target1.Content));
            Assert.IsTrue(clone.Reference.Reference.Reference.Equals(clone));
        }


        // Prototype

        [Test]
        public void ShouldGetAndSetDynamicProperty()
        {
            // Given
            var expected = new object();
            dynamic element = new SomePrototype();

            // When
            element.DynamicProperty = expected;
            dynamic actual = element.DynamicProperty;

            // Then
            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void ShouldGetPrototypeEvent()
        {
            // Given
            string result = string.Empty;
            var element = new SomePrototype();
            element.PrototypeEvent += () => result += "Handler1";
            element.PrototypeEvent += () => result += "Handler2";

            // When
            Action actual = ((dynamic) element).PrototypeEvent;

            // Then

            Assert.IsNotNull(actual);

            actual();
            Assert.AreEqual("Handler1Handler2", result);
        }

        [Test]
        public void ShouldGetPrototypeField()
        {
            // Given
            string expected = Guid.NewGuid().ToString();
            dynamic element = new SomePrototype {PrototypeField = expected};

            // When
            dynamic actual = element.PrototypeField;

            // Then
            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void ShouldGetPrototypeMethod()
        {
            // Given
            var element = new SomePrototype();
            Func<string, string> expected = element.PrototypeMethod;

            // When
            Func<string, string> actual = ((dynamic) element).PrototypeMethod;

            // Then
            Assert.IsNotNull(actual);
            Assert.AreEqual(expected, actual);
            Assert.AreEqual(expected("Arg1"), actual("Arg1"));
        }

        [Test]
        public void ShouldGetPrototypeProperty()
        {
            // Given
            string expected = Guid.NewGuid().ToString();
            dynamic element = new SomePrototype {PrototypeProperty = expected};

            // When
            dynamic actual = element.PrototypeProperty;

            // Then
            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void ShouldInvokeDynamicMethod()
        {
            // Given
            Guid expected = Guid.NewGuid();
            dynamic element = new SomePrototype();
            element.DynamicMethod = new Func<object>(() => expected);

            // When
            dynamic actual = element.DynamicMethod();

            // Then
            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void ShouldRemoveDynamicMethodWhenUnsubscribe()
        {
            // Given
            Guid expected = Guid.NewGuid();
            dynamic element = new SomePrototype();
            Func<object> subscription = () => expected;
            element.DynamicMethod = subscription;

            // When
            element.DynamicMethod -= subscription;
            dynamic actual = element.DynamicMethod;

            // Then
            Assert.IsNull(actual);
        }

        [Test]
        public void ShouldReplacePrototypeEvents()
        {
            // Given
            string result = string.Empty;
            var element = new SomePrototype();
            element.PrototypeEvent += () => result += "Handler1";
            element.PrototypeEvent += () => result += "Handler2";
            Func<int> delegateWithWrongSignature = () => 12345;

            // When
            ((dynamic) element).PrototypeEvent = delegateWithWrongSignature;
            Func<int> actual = ((dynamic) element).PrototypeEvent;

            // Then
            Assert.IsNotNull(actual);
            Assert.AreEqual(12345, actual());
        }

        [Test]
        public void ShouldReplacePrototypeFields()
        {
            // Given
            DateTime wrongTypeValue = DateTime.Now;
            dynamic element = new SomePrototype();

            // When
            element.PrototypeField = wrongTypeValue;
            dynamic actual = element.PrototypeField;

            // Then
            Assert.AreEqual(wrongTypeValue, actual);
        }

        [Test]
        public void ShouldReplacePrototypeMethods()
        {
            // Given
            Func<int> delegateWithWrongSignature = () => 12345;
            dynamic element = new SomePrototype();

            // When
            element.PrototypeMethod = delegateWithWrongSignature;
            Func<int> actual = element.PrototypeMethod;

            // Then
            Assert.IsNotNull(actual);
            Assert.AreEqual(12345, actual());
        }

        [Test]
        public void ShouldReplacePrototypeProperties()
        {
            // Given
            DateTime wrongTypeValue = DateTime.Now;
            dynamic element = new SomePrototype();

            // When
            element.PrototypeProperty = wrongTypeValue;
            dynamic actual = element.PrototypeProperty;

            // Then
            Assert.AreEqual(wrongTypeValue, actual);
        }

        [Test]
        public void ShouldSetPrototypeEvent()
        {
            // Given
            string result = string.Empty;
            var element = new SomePrototype();
            element.PrototypeEvent += () => result += "Handler1";
            element.PrototypeEvent += () => result += "Handler2";

            // When
            ((dynamic) element).PrototypeEvent = new Action(() => result += "NewHandler");
            Action actual = ((dynamic) element).PrototypeEvent;

            // Then

            Assert.IsNotNull(actual);

            actual();
            Assert.AreEqual("NewHandler", result);
        }

        [Test]
        public void ShouldSetPrototypeField()
        {
            // Given
            string expected = Guid.NewGuid().ToString();
            dynamic element = new SomePrototype();

            // When
            element.PrototypeField = expected;
            dynamic actual = element.PrototypeField;

            // Then
            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void ShouldSetPrototypeMethod()
        {
            // Given
            var element = new SomePrototype();

            // When
            ((dynamic) element).PrototypeMethod = new Func<string, string>(a => "NewMethod");
            Func<string, string> actual = ((dynamic) element).PrototypeMethod;

            // Then
            Assert.IsNotNull(actual);
            Assert.AreEqual("NewMethod", actual("Arg1"));
        }

        [Test]
        public void ShouldSetPrototypeProperty()
        {
            // Given
            string expected = Guid.NewGuid().ToString();
            dynamic element = new SomePrototype();

            // When
            element.PrototypeProperty = expected;
            dynamic actual = element.PrototypeProperty;

            // Then
            Assert.AreEqual(expected, actual);
        }

        // ReSharper restore All
    }
}
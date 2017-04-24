using System;

using InfinniPlatform.Tests;

using NUnit.Framework;

namespace InfinniPlatform.Dynamic
{
    [TestFixture]
    [Category(TestCategories.UnitTest)]
    public sealed class DynamicWrapperTest
    {
        [Test]
        public void ShouldAddDynamicMethodWhenSubscribe()
        {
            // Given
            Guid expected = Guid.NewGuid();
            dynamic element = new DynamicWrapper();
            Func<object> subscription = () => expected;

            // When
            element.DynamicMethod += subscription;
            dynamic actual = element.DynamicMethod();

            // Then
            Assert.AreEqual(expected, actual);
        }


        // Prototype

        [Test]
        public void ShouldGetAndSetDynamicProperty()
        {
            // Given
            var expected = new object();
            dynamic element = new DynamicWrapper();

            // When
            element.DynamicProperty = expected;
            dynamic actual = element.DynamicProperty;

            // Then
            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void ShouldInvokeDynamicMethod()
        {
            // Given
            Guid expected = Guid.NewGuid();
            dynamic element = new DynamicWrapper();
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
            dynamic element = new DynamicWrapper();
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
            Func<int> function1 = () => 12345;
            Func<string> function2 = () => "Hello!";
            dynamic element = new DynamicWrapper();

            // When

            element.SomeMethod = function1;
            int result1 = element.SomeMethod();

            element.SomeMethod = function2;
            string result2 = element.SomeMethod();

            // Then
            Assert.AreEqual(function1(), result1);
            Assert.AreEqual(function2(), result2);
        }

        [Test]
        public void ShouldReplacePrototypeFields()
        {
            // Given
            DateTime wrongTypeValue = DateTime.Now;
            dynamic element = new DynamicWrapper();

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
            dynamic element = new DynamicWrapper();

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
            dynamic element = new DynamicWrapper();

            // When
            element.PrototypeProperty = wrongTypeValue;
            dynamic actual = element.PrototypeProperty;

            // Then
            Assert.AreEqual(wrongTypeValue, actual);
        }

        [Test]
        public void ShouldSetPrototypeField()
        {
            // Given
            string expected = Guid.NewGuid().ToString();
            dynamic element = new DynamicWrapper();

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
            var element = new DynamicWrapper();

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
            dynamic element = new DynamicWrapper();

            // When
            element.PrototypeProperty = expected;
            dynamic actual = element.PrototypeProperty;

            // Then
            Assert.AreEqual(expected, actual);
        }

        // ReSharper restore All
    }
}
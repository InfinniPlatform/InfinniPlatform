using InfinniPlatform.Serialization;
using InfinniPlatform.Tests;

using NUnit.Framework;

namespace InfinniPlatform.Dynamic
{
    [TestFixture]
    [Category(TestCategories.UnitTest)]
    public class DynamicObjectExtensionsTest
    {
        // ReSharper disable UnusedAutoPropertyAccessor.Local

        class A
        {
            [SerializerPropertyName("p1")]
            public int? Property1 { get; set; }

            [SerializerPropertyName("p2")]
            public B Property2 { get; set; }

            [SerializerPropertyName("p3")]
            public B[] Property3 { get; set; }
        }


        class B
        {
            [SerializerPropertyName("sp1")]
            public int? SubProperty1 { get; set; }
        }

        // ReSharper restore UnusedAutoPropertyAccessor.Local


        [Test]
        [TestCase("NonExistentProperty", null)]
        [TestCase("Property1", 123)]
        [TestCase("p1", 123)]
        public void ShouldGetPropertyValueWhenTypedInstance(string propertyName, int? expectedPropertyValue)
        {
            // Given
            var target = new A { Property1 = 123 };

            // When
            var actualPropertyValue = target.TryGetPropertyValue(propertyName);

            // Then
            Assert.AreEqual(expectedPropertyValue, actualPropertyValue);
        }

        [Test]
        [TestCase("Property1", 123)]
        public void ShouldGetPropertyValueWhenDynamicInstance(string propertyName, int? expectedPropertyValue)
        {
            // Given
            var target = new DynamicDocument { { "Property1", 123} };

            // When
            var actualPropertyValue = target.TryGetPropertyValue(propertyName);

            // Then
            Assert.AreEqual(expectedPropertyValue, actualPropertyValue);
        }


        [Test]
        [TestCase("NonExistentProperty", 123, null)]
        [TestCase("Property1", 123, 123)]
        [TestCase("p1", 456, 456)]
        public void ShouldSetPropertyValueWhenTypedInstance(string propertyName, int? propertyValue, int? expectedPropertyValue)
        {
            // Given
            var target = new A();

            // When
            target.TrySetPropertyValue(propertyName, propertyValue);
            var actualPropertyValue = target.TryGetPropertyValue(propertyName);

            // Then
            Assert.AreEqual(expectedPropertyValue, actualPropertyValue);
        }

        [Test]
        [TestCase("Property1", 123, 123)]
        [TestCase("p1", 456, 456)]
        public void ShouldSetPropertyValueWhenDynamicInstance(string propertyName, int? propertyValue, int? expectedPropertyValue)
        {
            // Given
            var target = new DynamicDocument();

            // When
            target.TrySetPropertyValue(propertyName, propertyValue);
            var actualPropertyValue = target.TryGetPropertyValue(propertyName);

            // Then
            Assert.AreEqual(expectedPropertyValue, actualPropertyValue);
        }


        [Test]
        [TestCase("NonExistent.Property.Path", null)]
        [TestCase("Property1", 123)]
        [TestCase("p1", 123)]
        [TestCase("Property2.SubProperty1", 456)]
        [TestCase("Property2.sp1", 456)]
        [TestCase("p2.SubProperty1", 456)]
        [TestCase("p2.sp1", 456)]
        [TestCase("Property3.0.SubProperty1", 789)]
        [TestCase("Property3.0.sp1", 789)]
        [TestCase("p3.0.SubProperty1", 789)]
        [TestCase("p3.0.sp1", 789)]
        public void ShouldGetPropertyValueByPathWhenTypedInstance(string propertyPath, int? expectedPropertyValue)
        {
            // Given
            var target = new A
                         {
                             Property1 = 123,
                             Property2 = new B { SubProperty1 = 456 },
                             Property3 = new[] { new B { SubProperty1 = 789 } }
                         };

            // When
            var actualPropertyValue = target.TryGetPropertyValueByPath(propertyPath);

            // Then
            Assert.AreEqual(expectedPropertyValue, actualPropertyValue);
        }

        [Test]
        [TestCase("NonExistent.Property.Path", null)]
        [TestCase("Property1", 123)]
        [TestCase("Property2.SubProperty1", 456)]
        [TestCase("Property3.0.SubProperty1", 789)]
        public void ShouldGetPropertyValueByPathWhenDynamicInstance(string propertyPath, int? expectedPropertyValue)
        {
            // Given
            var target = new DynamicDocument
                         {
                             { "Property1", expectedPropertyValue },
                             { "Property2", new DynamicDocument { { "SubProperty1", 456 } } },
                             { "Property3", new[] { new DynamicDocument { { "SubProperty1", 789 } } } }
                         };

            // When
            var actualPropertyValue = target.TryGetPropertyValueByPath(propertyPath);

            // Then
            Assert.AreEqual(expectedPropertyValue, actualPropertyValue);
        }


        [Test]
        [TestCase("NonExistent.Property.Path", 123, null)]
        [TestCase("Property1", 123, 123)]
        [TestCase("p1", 123, 123)]
        [TestCase("Property2.SubProperty1", 456, 456)]
        [TestCase("Property2.sp1", 456, 456)]
        [TestCase("p2.SubProperty1", 456, 456)]
        [TestCase("p2.sp1", 456, 456)]
        [TestCase("Property3.0.SubProperty1", 789, 789)]
        [TestCase("Property3.0.sp1", 789, 789)]
        [TestCase("p3.0.SubProperty1", 789, 789)]
        [TestCase("p3.0.sp1", 789, 789)]
        public void ShouldSetPropertyValueByPathWhenTypedInstance(string propertyPath, int? propertyValue, int? expectedPropertyValue)
        {
            // Given
            var target = new A
                         {
                             Property1 = 0,
                             Property2 = new B { SubProperty1 = 0 },
                             Property3 = new[] { new B { SubProperty1 = 0 } }
                         };

            // When
            target.TrySetPropertyValueByPath(propertyPath, propertyValue);
            var actualPropertyValue = target.TryGetPropertyValueByPath(propertyPath);

            // Then
            Assert.AreEqual(expectedPropertyValue, actualPropertyValue);
        }

        [Test]
        [TestCase("NonExistent.Property.Path", 123, null)]
        [TestCase("Property1", 123, 123)]
        [TestCase("Property2.SubProperty1", 456, 456)]
        [TestCase("Property3.0.SubProperty1", 789, 789)]
        public void ShouldSetPropertyValueByPathWhenDynamicInstance(string propertyPath, int? propertyValue, int? expectedPropertyValue)
        {
            // Given
            var target = new DynamicDocument
                         {
                             { "Property1", 0 },
                             { "Property2", new DynamicDocument { { "SubProperty1", 0 } } },
                             { "Property3", new[] { new DynamicDocument { { "SubProperty1", 0 } } } }
                         };

            // When
            target.TrySetPropertyValueByPath(propertyPath, propertyValue);
            var actualPropertyValue = target.TryGetPropertyValueByPath(propertyPath);

            // Then
            Assert.AreEqual(expectedPropertyValue, actualPropertyValue);
        }
    }
}
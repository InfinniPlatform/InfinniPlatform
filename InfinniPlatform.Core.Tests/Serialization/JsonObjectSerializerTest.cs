using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;

using InfinniPlatform.Core.Abstractions.Dynamic;
using InfinniPlatform.Core.Abstractions.Serialization;
using InfinniPlatform.Core.Serialization;
using InfinniPlatform.Core.Tests.TestEntities;

using NUnit.Framework;

namespace InfinniPlatform.Core.Tests.Serialization
{
    [TestFixture]
    [Category(TestCategories.UnitTest)]
    public sealed class JsonObjectSerializerTest
    {
        private static T SerializeAndDeserialize<T>(T value, Action<KnownTypesContainer> addKnownTypes = null)
        {
            IEnumerable<IKnownTypesSource> knowTypes = (addKnownTypes != null) ? new[] { new KnownTypesSourceStub(addKnownTypes) } : null;

            var serializer = new JsonObjectSerializer(true, knownTypes: knowTypes);

            byte[] data = serializer.Serialize(value);

#if DEBUG
            using (var memory = new MemoryStream(data))
            {
                using (var reader = new StreamReader(memory))
                {
                    Console.WriteLine(reader.ReadToEnd());
                }
            }
#endif

            return (T)serializer.Deserialize(data, typeof(T));
        }

        [Test]
        public void ShouldSerializeAbstractClassReference()
        {
            // Given
            Action<KnownTypesContainer> knownTypes = c => c.Add<Milk>("milk").Add<Bread>("bread").Add<Employee>("employee");
            var milk = new Milk { Caption = "Milk1", Protein = 2.9f };
            var bread = new Bread { Caption = "Bread1", Richness = 365 };
            var item1 = new OrderItem { Product = milk, Count = 1, Price = 45.5f };
            var item2 = new OrderItem { Product = bread, Count = 1, Price = 20.3f };
            var client = new Employee { FirstName = "FirstName1", JobTitle = "JobTitle1" };
            var target = new Order { Client = client, Items = new[] { item1, item2 } };

            // When
            Order result = SerializeAndDeserialize(target, knownTypes);

            // Then
            Assert.IsNotNull(result);
            Assert.IsInstanceOf<Employee>(result.Client);
            Assert.AreEqual(client.FirstName, result.Client.FirstName);
            Assert.AreEqual(client.JobTitle, ((Employee)result.Client).JobTitle);
            Assert.IsNotNull(result.Items);
            Assert.AreEqual(2, result.Items.Count());
            Assert.IsInstanceOf<Milk>(result.Items.ElementAt(0).Product);
            Assert.AreEqual(milk.Caption, ((Milk)result.Items.ElementAt(0).Product).Caption);
            Assert.AreEqual(milk.Protein, ((Milk)result.Items.ElementAt(0).Product).Protein);
            Assert.AreEqual(item1.Count, result.Items.ElementAt(0).Count);
            Assert.AreEqual(item1.Price, result.Items.ElementAt(0).Price);
            Assert.IsInstanceOf<Bread>(result.Items.ElementAt(1).Product);
            Assert.AreEqual(bread.Caption, ((Bread)result.Items.ElementAt(1).Product).Caption);
            Assert.AreEqual(bread.Richness, ((Bread)result.Items.ElementAt(1).Product).Richness);
            Assert.AreEqual(item2.Count, result.Items.ElementAt(1).Count);
            Assert.AreEqual(item2.Price, result.Items.ElementAt(1).Price);
        }

        [Test]
        public void ShouldSerializeCustomCollection()
        {
            // Given
            Action<KnownTypesContainer> knownTypes = c => c.Add<Milk>("milk").Add<Bread>("bread").Add<Employee>("employee");
            var milk = new Milk { Caption = "Milk1", Protein = 2.9f };
            var bread = new Bread { Caption = "Bread1", Richness = 365 };
            var item1 = new OrderItem { Product = milk, Count = 1, Price = 45.5f };
            var item2 = new OrderItem { Product = bread, Count = 1, Price = 20.3f };
            var client = new Employee { FirstName = "FirstName1", JobTitle = "JobTitle1" };
            var order1 = new Order { Client = client, Items = new[] { item1 } };
            var order2 = new Order { Client = client, Items = new[] { item2 } };
            var target = new OrderHistory();
            target.AddOrder(order1);
            target.AddOrder(order2);

            // When
            OrderHistory result = SerializeAndDeserialize(target, knownTypes);

            // Then
            Assert.IsNotNull(result);
            Assert.AreEqual(2, result.Count());
            Assert.IsInstanceOf<Employee>(result.ElementAt(0).Client);
            Assert.AreEqual(client.FirstName, result.ElementAt(0).Client.FirstName);
            Assert.AreEqual(client.JobTitle, ((Employee)result.ElementAt(0).Client).JobTitle);
            Assert.IsNotNull(result.ElementAt(0).Items);
            Assert.AreEqual(1, result.ElementAt(0).Items.Count());
            Assert.IsInstanceOf<Milk>(result.ElementAt(0).Items.ElementAt(0).Product);
            Assert.AreEqual(milk.Caption, ((Milk)result.ElementAt(0).Items.ElementAt(0).Product).Caption);
            Assert.AreEqual(milk.Protein, ((Milk)result.ElementAt(0).Items.ElementAt(0).Product).Protein);
            Assert.AreEqual(item1.Count, result.ElementAt(0).Items.ElementAt(0).Count);
            Assert.AreEqual(item1.Price, result.ElementAt(0).Items.ElementAt(0).Price);
            Assert.IsInstanceOf<Bread>(result.ElementAt(1).Items.ElementAt(0).Product);
            Assert.AreEqual(bread.Caption, ((Bread)result.ElementAt(1).Items.ElementAt(0).Product).Caption);
            Assert.AreEqual(bread.Richness, ((Bread)result.ElementAt(1).Items.ElementAt(0).Product).Richness);
            Assert.AreEqual(item2.Count, result.ElementAt(1).Items.ElementAt(0).Count);
            Assert.AreEqual(item2.Price, result.ElementAt(1).Items.ElementAt(0).Price);
        }

        [Test]
        public void ShouldSerializeCustomCollectionReference()
        {
            // Given
            Action<KnownTypesContainer> knownTypes = c => c.Add<Milk>("milk").Add<Bread>("bread").Add<Employee>("employee");
            var milk = new Milk { Caption = "Milk1", Protein = 2.9f };
            var bread = new Bread { Caption = "Bread1", Richness = 365 };
            var item1 = new OrderItem { Product = milk, Count = 1, Price = 45.5f };
            var item2 = new OrderItem { Product = bread, Count = 1, Price = 20.3f };
            var client = new Employee { FirstName = "FirstName1", JobTitle = "JobTitle1" };
            var order1 = new Order { Client = client, Items = new[] { item1 } };
            var order2 = new Order { Client = client, Items = new[] { item2 } };
            var history = new OrderHistory();
            history.AddOrder(order1);
            history.AddOrder(order2);
            var target = new Account { OrderHistory = history };

            // When
            Account result = SerializeAndDeserialize(target, knownTypes);

            // Then
            Assert.IsNotNull(result);
            Assert.IsNotNull(result.OrderHistory);
            Assert.AreEqual(2, result.OrderHistory.Count());
            Assert.IsInstanceOf<Employee>(result.OrderHistory.ElementAt(0).Client);
            Assert.AreEqual(client.FirstName, result.OrderHistory.ElementAt(0).Client.FirstName);
            Assert.AreEqual(client.JobTitle, ((Employee)result.OrderHistory.ElementAt(0).Client).JobTitle);
            Assert.IsNotNull(result.OrderHistory.ElementAt(0).Items);
            Assert.AreEqual(1, result.OrderHistory.ElementAt(0).Items.Count());
            Assert.IsInstanceOf<Milk>(result.OrderHistory.ElementAt(0).Items.ElementAt(0).Product);
            Assert.AreEqual(milk.Caption, ((Milk)result.OrderHistory.ElementAt(0).Items.ElementAt(0).Product).Caption);
            Assert.AreEqual(milk.Protein, ((Milk)result.OrderHistory.ElementAt(0).Items.ElementAt(0).Product).Protein);
            Assert.AreEqual(item1.Count, result.OrderHistory.ElementAt(0).Items.ElementAt(0).Count);
            Assert.AreEqual(item1.Price, result.OrderHistory.ElementAt(0).Items.ElementAt(0).Price);
            Assert.IsInstanceOf<Bread>(result.OrderHistory.ElementAt(1).Items.ElementAt(0).Product);
            Assert.AreEqual(bread.Caption, ((Bread)result.OrderHistory.ElementAt(1).Items.ElementAt(0).Product).Caption);
            Assert.AreEqual(bread.Richness, ((Bread)result.OrderHistory.ElementAt(1).Items.ElementAt(0).Product).Richness);
            Assert.AreEqual(item2.Count, result.OrderHistory.ElementAt(1).Items.ElementAt(0).Count);
            Assert.AreEqual(item2.Price, result.OrderHistory.ElementAt(1).Items.ElementAt(0).Price);
        }

        [Test]
        public void ShouldSerializeInterfaceArrayReference()
        {
            // Given
            Action<KnownTypesContainer> knownTypes = c => c.Add<Milk>("milk").Add<Bread>("bread");
            var milk = new Milk { Caption = "Milk1", Protein = 2.9f };
            var bread = new Bread { Caption = "Bread1", Richness = 365 };
            var target = new ProductCategory { Products = new IProduct[] { milk, bread } };

            // When
            ProductCategory result = SerializeAndDeserialize(target, knownTypes);

            // Then
            Assert.IsNotNull(result);
            Assert.IsNotNull(result.Products);
            Assert.AreEqual(2, result.Products.Count());
            Assert.IsInstanceOf<Milk>(result.Products.ElementAt(0));
            Assert.AreEqual(milk.Caption, ((Milk)result.Products.ElementAt(0)).Caption);
            Assert.AreEqual(milk.Protein, ((Milk)result.Products.ElementAt(0)).Protein);
            Assert.IsInstanceOf<Bread>(result.Products.ElementAt(1));
            Assert.AreEqual(bread.Caption, ((Bread)result.Products.ElementAt(1)).Caption);
            Assert.AreEqual(bread.Richness, ((Bread)result.Products.ElementAt(1)).Richness);
        }

        [Test]
        public void ShouldSerializeInterfaceReference()
        {
            // Given
            Action<KnownTypesContainer> knownTypes = c => c.Add<Milk>("milk");
            var milk = new Milk { Caption = "Milk1", Protein = 2.9f };
            var target = new OrderItem { Product = milk, Count = 1, Price = 45.5f };

            // When
            OrderItem result = SerializeAndDeserialize(target, knownTypes);

            // Then
            Assert.IsNotNull(result);
            Assert.IsInstanceOf<Milk>(result.Product);
            Assert.AreEqual(milk.Caption, ((Milk)result.Product).Caption);
            Assert.AreEqual(milk.Protein, ((Milk)result.Product).Protein);
            Assert.AreEqual(target.Count, result.Count);
            Assert.AreEqual(target.Price, result.Price);
        }

        [Test]
        public void ShouldSerializeKnownType()
        {
            // Given
            Action<KnownTypesContainer> knownTypes = c => c.Add<Bread>("bread");
            var target = new Bread { Caption = "Bread1", Richness = 365 };

            // When
            Bread result = SerializeAndDeserialize(target, knownTypes);

            // Then
            Assert.IsNotNull(result);
            Assert.AreEqual(target.Caption, result.Caption);
            Assert.AreEqual(target.Richness, result.Richness);
        }

        [Test]
        public void ShouldSerializeKnownTypeArray()
        {
            // Given
            Action<KnownTypesContainer> knownTypes = c => c.Add<Bread>("bread");
            var target = new[] { new Bread { Caption = "Bread1", Richness = 365 } };

            // When
            Bread[] result = SerializeAndDeserialize(target, knownTypes);

            // Then
            Assert.IsNotNull(result);
            Assert.AreEqual(1, result.Length);
            Assert.AreEqual(target[0].Caption, result[0].Caption);
            Assert.AreEqual(target[0].Richness, result[0].Richness);
        }

        [Test]
        public void ShouldSerializeUnknownType()
        {
            // Given
            var target = new Bread { Caption = "Bread1", Richness = 365 };

            // When
            Bread result = SerializeAndDeserialize(target);

            // Then
            Assert.IsNotNull(result);
            Assert.AreEqual(target.Caption, result.Caption);
            Assert.AreEqual(target.Richness, result.Richness);
        }

        [Test]
        public void ShouldSerializeUnknownTypeArray()
        {
            // Given
            var target = new[] { new Bread { Caption = "Bread1", Richness = 365 } };

            // When
            Bread[] result = SerializeAndDeserialize(target);

            // Then
            Assert.IsNotNull(result);
            Assert.AreEqual(1, result.Length);
            Assert.AreEqual(target[0].Caption, result[0].Caption);
            Assert.AreEqual(target[0].Richness, result[0].Richness);
        }

        [Test]
        public void ShouldSerializeWhenAbstractionRefersToAnotherAbstraction()
        {
            // Given
            Action<KnownTypesContainer> knownTypes = c => c.Add<ProductCategory>("category").Add<Milk>("milk").Add<Bread>("bread");
            var milk = new Milk { Caption = "Milk1", Protein = 2.9f };
            var bread = new Bread { Caption = "Bread1", Richness = 365 };
            var target = new ProductCategory { Products = new IProduct[] { milk, bread } };

            // When
            ProductCategory result = SerializeAndDeserialize(target, knownTypes);

            // Then
            Assert.IsNotNull(result);
            Assert.IsNotNull(result.Products);
            Assert.AreEqual(2, result.Products.Count());
            Assert.IsInstanceOf<Milk>(result.Products.ElementAt(0));
            Assert.AreEqual(milk.Caption, ((Milk)result.Products.ElementAt(0)).Caption);
            Assert.AreEqual(milk.Protein, ((Milk)result.Products.ElementAt(0)).Protein);
            Assert.IsInstanceOf<Bread>(result.Products.ElementAt(1));
            Assert.AreEqual(bread.Caption, ((Bread)result.Products.ElementAt(1)).Caption);
            Assert.AreEqual(bread.Richness, ((Bread)result.Products.ElementAt(1)).Richness);
        }

        [Test]
        public void ShouldIgnoreNullablePropertiesWhenSerializingKnowTypes()
        {
            // Given
            Action<KnownTypesContainer> knownTypes = c => c.Add<Milk>("milk").Add<Bread>("bread");
            var milk = new Milk { Caption = null, Protein = 2.9f };
            var bread = new Bread { Caption = null, Richness = 365 };
            var target = new ProductCategory { Products = new IProduct[] { milk, bread } };
            var serializer = new JsonObjectSerializer(knownTypes: new[] { new KnownTypesSourceStub(knownTypes) }, withFormatting: false);

            // When
            var entityToJson = serializer.ConvertToString(target);

            // Then
            Assert.AreEqual(@"{""Products"":[{""milk"":{""Protein"":2.9}},{""bread"":{""Richness"":365}}]}", entityToJson);
        }

        [Test]
        public void SerializationVisibilityTest()
        {
            // Given

            var entity = new TestEntity("publicField",
                                        "publicReadonlyField",
                                        "privateField",
                                        "privateReadonlyField",
                                        "privateVisibleField",
                                        "privateVisibleReadonlyField",
                                        "publicProperty",
                                        "publicPropertyWithPrivateSetter",
                                        "publicPropertyWithoutSetter",
                                        "privateProperty",
                                        "publicPropertyWithePrivateVisiblSetter",
                                        "privateVisibleProperty");

            // When

            var deserializedEntity = SerializeAndDeserialize(entity);

            // Then

            Assert.IsNotNull(deserializedEntity);
            Assert.AreEqual(entity.GetPublicField(), deserializedEntity.GetPublicField());
            Assert.AreEqual(null, deserializedEntity.GetPublicReadonlyField());
            Assert.AreEqual(null, deserializedEntity.GetPrivateField());
            Assert.AreEqual(null, deserializedEntity.GetPrivateReadonlyField());
            Assert.AreEqual(entity.GetPrivateVisibleField(), deserializedEntity.GetPrivateVisibleField());
            Assert.AreEqual(null, deserializedEntity.GetPrivateVisibleReadonlyField());
            Assert.AreEqual(entity.GetPublicProperty(), deserializedEntity.GetPublicProperty());
            Assert.AreEqual(null, deserializedEntity.GetPublicPropertyWithPrivateSetter());
            Assert.AreEqual(null, deserializedEntity.GetPublicPropertyWithoutSetter());
            Assert.AreEqual(null, deserializedEntity.GetPrivateProperty());
            Assert.AreEqual(entity.GetPublicPropertyWithePrivateVisiblSetter(), deserializedEntity.GetPublicPropertyWithePrivateVisiblSetter());
            Assert.AreEqual(entity.GetPrivateVisibleProperty(), deserializedEntity.GetPrivateVisibleProperty());
        }

        [Test]
        public void ShouldSerializeWithCustomPropertyName()
        {
            // Given
            var entity = new EnityWithCustomPropertyName { FirstName = "John", LastName = "Smith" };
            var serializer = new JsonObjectSerializer();

            // When
            var entityToJson = serializer.ConvertToString(entity);
            var jsonToEntity = serializer.Deserialize<EnityWithCustomPropertyName>(entityToJson);

            // Then

            StringAssert.IsMatch(@"\{\s*\""forename\""\s*\:\s*\""John\""\s*\,\s*\""surname\""\s*\:\s*\""Smith\""\s*\}", entityToJson);

            Assert.IsNotNull(jsonToEntity);
            Assert.AreEqual("John", entity.FirstName);
            Assert.AreEqual("Smith", entity.LastName);
        }

        [Test]
        public void ShouldDeserializeObjectPropertiesAsDynamicWrapper()
        {
            // Given

            const string sourceJson
                = @"{
                        'PropertyInt': 111,
                        'PropertyScalar': 222,
                        'PropertyObject': {
                            'Property1': 333,
                            'Property2': 'Hello!'
                        },
                        'PropertyArray': [
                            1,
                            2,
                            3
                        ]
                    }";

            var serializer = new JsonObjectSerializer();

            // When
            var result = serializer.Deserialize<SomeClass>(sourceJson);

            // Then

            Assert.IsNotNull(result);

            // Strong type
            Assert.AreEqual(111, result.PropertyInt);

            // Scalar type
            Assert.AreEqual(222, result.PropertyScalar);

            // Object type
            Assert.IsInstanceOf<DynamicWrapper>(result.PropertyObject);
            var propertyObject = (DynamicWrapper)result.PropertyObject;
            Assert.AreEqual(333, propertyObject["Property1"]);
            Assert.AreEqual("Hello!", propertyObject["Property2"]);

            // Array type
            Assert.IsInstanceOf<IEnumerable>(result.PropertyArray);
            var propertyArray = ((IEnumerable)result.PropertyArray).Cast<object>().ToArray();
            Assert.AreEqual(3, propertyArray.Length);
            Assert.AreEqual(1, propertyArray[0]);
            Assert.AreEqual(2, propertyArray[1]);
            Assert.AreEqual(3, propertyArray[2]);
        }


        // ReSharper disable UnusedAutoPropertyAccessor.Local

        private class SomeClass
        {
            public int PropertyInt { get; set; }

            public object PropertyScalar { get; set; }

            public object PropertyObject { get; set; }

            public object PropertyArray { get; set; }
        }

        // ReSharper restore UnusedAutoPropertyAccessor.Local


        private class KnownTypesSourceStub : IKnownTypesSource
        {
            private readonly Action<KnownTypesContainer> _addKnownTypes;

            public KnownTypesSourceStub(Action<KnownTypesContainer> addKnownTypes)
            {
                _addKnownTypes = addKnownTypes;
            }

            public void AddKnownTypes(KnownTypesContainer knownTypesContainer)
            {
                _addKnownTypes(knownTypesContainer);
            }
        }
    }
}
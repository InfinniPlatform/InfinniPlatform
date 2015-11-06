using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using InfinniPlatform.Api.Serialization;
using NUnit.Framework;

namespace InfinniPlatform.Api.Tests.Serialization
{
    [TestFixture]
    [Category(TestCategories.UnitTest)]
    public sealed class JsonObjectSerializerTest
    {
        private static T SerializeAndDeserialize<T>(T value, KnownTypesContainer knownTypes = null)
        {
            var serializer = new JsonObjectSerializer(false, knownTypes);

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

            return (T) serializer.Deserialize(data, typeof (T));
        }

        [Test]
        public void ShouldSerializeAbstractClassArray()
        {
            // Given
            KnownTypesContainer knownTypes = new KnownTypesContainer().Add<Employee>("employee").Add<Bum>("bum");
            var employee = new Employee {FirstName = "Вася", JobTitle = "Аналитик"};
            var bum = new Bum {FirstName = "Шнур", Address = "Ленинград"};
            var target = new Person[] {employee, bum};

            // When
            Person[] result = SerializeAndDeserialize(target, knownTypes);

            // Then
            Assert.IsNotNull(result);
            Assert.AreEqual(2, result.Length);
            Assert.IsInstanceOf<Employee>(result[0]);
            Assert.AreEqual(employee.FirstName, result[0].FirstName);
            Assert.AreEqual(employee.JobTitle, ((Employee) result[0]).JobTitle);
            Assert.IsInstanceOf<Bum>(result[1]);
            Assert.AreEqual(bum.FirstName, result[1].FirstName);
            Assert.AreEqual(bum.Address, ((Bum) result[1]).Address);
        }

        [Test]
        public void ShouldSerializeAbstractClassReference()
        {
            // Given
            KnownTypesContainer knownTypes =
                new KnownTypesContainer().Add<Milk>("milk").Add<Bread>("bread").Add<Employee>("employee");
            var milk = new Milk {Caption = "Первый вкус", Protein = 2.9f};
            var bread = new Bread {Caption = "Бородинский", Richness = 365};
            var item1 = new OrderItem {Product = milk, Count = 1, Price = 45.5f};
            var item2 = new OrderItem {Product = bread, Count = 1, Price = 20.3f};
            var client = new Employee {FirstName = "Вася", JobTitle = "Аналитик"};
            var target = new Order {Client = client, Items = new[] {item1, item2}};

            // When
            Order result = SerializeAndDeserialize(target, knownTypes);

            // Then
            Assert.IsNotNull(result);
            Assert.IsInstanceOf<Employee>(result.Client);
            Assert.AreEqual(client.FirstName, result.Client.FirstName);
            Assert.AreEqual(client.JobTitle, ((Employee) result.Client).JobTitle);
            Assert.IsNotNull(result.Items);
            Assert.AreEqual(2, result.Items.Count());
            Assert.IsInstanceOf<Milk>(result.Items.ElementAt(0).Product);
            Assert.AreEqual(milk.Caption, ((Milk) result.Items.ElementAt(0).Product).Caption);
            Assert.AreEqual(milk.Protein, ((Milk) result.Items.ElementAt(0).Product).Protein);
            Assert.AreEqual(item1.Count, result.Items.ElementAt(0).Count);
            Assert.AreEqual(item1.Price, result.Items.ElementAt(0).Price);
            Assert.IsInstanceOf<Bread>(result.Items.ElementAt(1).Product);
            Assert.AreEqual(bread.Caption, ((Bread) result.Items.ElementAt(1).Product).Caption);
            Assert.AreEqual(bread.Richness, ((Bread) result.Items.ElementAt(1).Product).Richness);
            Assert.AreEqual(item2.Count, result.Items.ElementAt(1).Count);
            Assert.AreEqual(item2.Price, result.Items.ElementAt(1).Price);
        }

        [Test]
        public void ShouldSerializeCustomCollection()
        {
            // Given
            KnownTypesContainer knownTypes =
                new KnownTypesContainer().Add<Milk>("milk").Add<Bread>("bread").Add<Employee>("employee");
            var milk = new Milk {Caption = "Первый вкус", Protein = 2.9f};
            var bread = new Bread {Caption = "Бородинский", Richness = 365};
            var item1 = new OrderItem {Product = milk, Count = 1, Price = 45.5f};
            var item2 = new OrderItem {Product = bread, Count = 1, Price = 20.3f};
            var client = new Employee {FirstName = "Вася", JobTitle = "Аналитик"};
            var order1 = new Order {Client = client, Items = new[] {item1}};
            var order2 = new Order {Client = client, Items = new[] {item2}};
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
            Assert.AreEqual(client.JobTitle, ((Employee) result.ElementAt(0).Client).JobTitle);
            Assert.IsNotNull(result.ElementAt(0).Items);
            Assert.AreEqual(1, result.ElementAt(0).Items.Count());
            Assert.IsInstanceOf<Milk>(result.ElementAt(0).Items.ElementAt(0).Product);
            Assert.AreEqual(milk.Caption, ((Milk) result.ElementAt(0).Items.ElementAt(0).Product).Caption);
            Assert.AreEqual(milk.Protein, ((Milk) result.ElementAt(0).Items.ElementAt(0).Product).Protein);
            Assert.AreEqual(item1.Count, result.ElementAt(0).Items.ElementAt(0).Count);
            Assert.AreEqual(item1.Price, result.ElementAt(0).Items.ElementAt(0).Price);
            Assert.IsInstanceOf<Bread>(result.ElementAt(1).Items.ElementAt(0).Product);
            Assert.AreEqual(bread.Caption, ((Bread) result.ElementAt(1).Items.ElementAt(0).Product).Caption);
            Assert.AreEqual(bread.Richness, ((Bread) result.ElementAt(1).Items.ElementAt(0).Product).Richness);
            Assert.AreEqual(item2.Count, result.ElementAt(1).Items.ElementAt(0).Count);
            Assert.AreEqual(item2.Price, result.ElementAt(1).Items.ElementAt(0).Price);
        }

        [Test]
        public void ShouldSerializeCustomCollectionReference()
        {
            // Given
            KnownTypesContainer knownTypes =
                new KnownTypesContainer().Add<Milk>("milk").Add<Bread>("bread").Add<Employee>("employee");
            var milk = new Milk {Caption = "Первый вкус", Protein = 2.9f};
            var bread = new Bread {Caption = "Бородинский", Richness = 365};
            var item1 = new OrderItem {Product = milk, Count = 1, Price = 45.5f};
            var item2 = new OrderItem {Product = bread, Count = 1, Price = 20.3f};
            var client = new Employee {FirstName = "Вася", JobTitle = "Аналитик"};
            var order1 = new Order {Client = client, Items = new[] {item1}};
            var order2 = new Order {Client = client, Items = new[] {item2}};
            var history = new OrderHistory();
            history.AddOrder(order1);
            history.AddOrder(order2);
            var target = new Account {OrderHistory = history};

            // When
            Account result = SerializeAndDeserialize(target, knownTypes);

            // Then
            Assert.IsNotNull(result);
            Assert.IsNotNull(result.OrderHistory);
            Assert.AreEqual(2, result.OrderHistory.Count());
            Assert.IsInstanceOf<Employee>(result.OrderHistory.ElementAt(0).Client);
            Assert.AreEqual(client.FirstName, result.OrderHistory.ElementAt(0).Client.FirstName);
            Assert.AreEqual(client.JobTitle, ((Employee) result.OrderHistory.ElementAt(0).Client).JobTitle);
            Assert.IsNotNull(result.OrderHistory.ElementAt(0).Items);
            Assert.AreEqual(1, result.OrderHistory.ElementAt(0).Items.Count());
            Assert.IsInstanceOf<Milk>(result.OrderHistory.ElementAt(0).Items.ElementAt(0).Product);
            Assert.AreEqual(milk.Caption, ((Milk) result.OrderHistory.ElementAt(0).Items.ElementAt(0).Product).Caption);
            Assert.AreEqual(milk.Protein, ((Milk) result.OrderHistory.ElementAt(0).Items.ElementAt(0).Product).Protein);
            Assert.AreEqual(item1.Count, result.OrderHistory.ElementAt(0).Items.ElementAt(0).Count);
            Assert.AreEqual(item1.Price, result.OrderHistory.ElementAt(0).Items.ElementAt(0).Price);
            Assert.IsInstanceOf<Bread>(result.OrderHistory.ElementAt(1).Items.ElementAt(0).Product);
            Assert.AreEqual(bread.Caption, ((Bread) result.OrderHistory.ElementAt(1).Items.ElementAt(0).Product).Caption);
            Assert.AreEqual(bread.Richness,
                            ((Bread) result.OrderHistory.ElementAt(1).Items.ElementAt(0).Product).Richness);
            Assert.AreEqual(item2.Count, result.OrderHistory.ElementAt(1).Items.ElementAt(0).Count);
            Assert.AreEqual(item2.Price, result.OrderHistory.ElementAt(1).Items.ElementAt(0).Price);
        }

        [Test]
        public void ShouldSerializeInterfaceArray()
        {
            // Given
            KnownTypesContainer knownTypes = new KnownTypesContainer().Add<Milk>("milk").Add<Bread>("bread");
            var milk = new Milk {Caption = "Первый вкус", Protein = 2.9f};
            var bread = new Bread {Caption = "Бородинский", Richness = 365};
            var target = new IProduct[] {milk, bread};

            // When
            IProduct[] result = SerializeAndDeserialize(target, knownTypes);

            // Then
            Assert.IsNotNull(result);
            Assert.AreEqual(2, result.Length);
            Assert.IsInstanceOf<Milk>(result[0]);
            Assert.AreEqual(milk.Caption, ((Milk) result[0]).Caption);
            Assert.AreEqual(milk.Protein, ((Milk) result[0]).Protein);
            Assert.IsInstanceOf<Bread>(result[1]);
            Assert.AreEqual(bread.Caption, ((Bread) result[1]).Caption);
            Assert.AreEqual(bread.Richness, ((Bread) result[1]).Richness);
        }

        [Test]
        public void ShouldSerializeInterfaceArrayReference()
        {
            // Given
            KnownTypesContainer knownTypes = new KnownTypesContainer().Add<Milk>("milk").Add<Bread>("bread");
            var milk = new Milk {Caption = "Первый вкус", Protein = 2.9f};
            var bread = new Bread {Caption = "Бородинский", Richness = 365};
            var target = new ProductCategory {Products = new IProduct[] {milk, bread}};

            // When
            ProductCategory result = SerializeAndDeserialize(target, knownTypes);

            // Then
            Assert.IsNotNull(result);
            Assert.IsNotNull(result.Products);
            Assert.AreEqual(2, result.Products.Count());
            Assert.IsInstanceOf<Milk>(result.Products.ElementAt(0));
            Assert.AreEqual(milk.Caption, ((Milk) result.Products.ElementAt(0)).Caption);
            Assert.AreEqual(milk.Protein, ((Milk) result.Products.ElementAt(0)).Protein);
            Assert.IsInstanceOf<Bread>(result.Products.ElementAt(1));
            Assert.AreEqual(bread.Caption, ((Bread) result.Products.ElementAt(1)).Caption);
            Assert.AreEqual(bread.Richness, ((Bread) result.Products.ElementAt(1)).Richness);
        }

        [Test]
        public void ShouldSerializeInterfaceReference()
        {
            // Given
            KnownTypesContainer knownTypes = new KnownTypesContainer().Add<Milk>("milk");
            var milk = new Milk {Caption = "Первый вкус", Protein = 2.9f};
            var target = new OrderItem {Product = milk, Count = 1, Price = 45.5f};

            // When
            OrderItem result = SerializeAndDeserialize(target, knownTypes);

            // Then
            Assert.IsNotNull(result);
            Assert.IsInstanceOf<Milk>(result.Product);
            Assert.AreEqual(milk.Caption, ((Milk) result.Product).Caption);
            Assert.AreEqual(milk.Protein, ((Milk) result.Product).Protein);
            Assert.AreEqual(target.Count, result.Count);
            Assert.AreEqual(target.Price, result.Price);
        }

        [Test]
        public void ShouldSerializeKnownType()
        {
            // Given
            KnownTypesContainer knownTypes = new KnownTypesContainer().Add<Bread>("bread");
            var target = new Bread {Caption = "Бородинский", Richness = 365};

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
            KnownTypesContainer knownTypes = new KnownTypesContainer().Add<Bread>("bread");
            var target = new[] {new Bread {Caption = "Бородинский", Richness = 365}};

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
            var target = new Bread {Caption = "Бородинский", Richness = 365};

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
            var target = new[] {new Bread {Caption = "Бородинский", Richness = 365}};

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
            KnownTypesContainer knownTypes =
                new KnownTypesContainer().Add<ProductCategory>("Abstraction1").Add<Milk>("milk").Add<Bread>("bread");
            var milk = new Milk {Caption = "Первый вкус", Protein = 2.9f};
            var bread = new Bread {Caption = "Бородинский", Richness = 365};
            var target = new ProductCategory {Products = new IProduct[] {milk, bread}};

            // When
            ProductCategory result = SerializeAndDeserialize(target, knownTypes);

            // Then
            Assert.IsNotNull(result);
            Assert.IsNotNull(result.Products);
            Assert.AreEqual(2, result.Products.Count());
            Assert.IsInstanceOf<Milk>(result.Products.ElementAt(0));
            Assert.AreEqual(milk.Caption, ((Milk) result.Products.ElementAt(0)).Caption);
            Assert.AreEqual(milk.Protein, ((Milk) result.Products.ElementAt(0)).Protein);
            Assert.IsInstanceOf<Bread>(result.Products.ElementAt(1));
            Assert.AreEqual(bread.Caption, ((Bread) result.Products.ElementAt(1)).Caption);
            Assert.AreEqual(bread.Richness, ((Bread) result.Products.ElementAt(1)).Richness);
        }
    }


    internal interface IProduct
    {
        string Caption { get; set; }
    }

    internal class Bread : IProduct
    {
        public int Richness { get; set; }
        public string Caption { get; set; }
    }

    internal class Milk : IProduct
    {
        public float Protein { get; set; }
        public string Caption { get; set; }
    }


    internal abstract class Person
    {
        public string FirstName { get; set; }
    }

    internal class Employee : Person
    {
        public string JobTitle { get; set; }
    }

    internal class Bum : Person
    {
        public string Address { get; set; }
    }


    internal class OrderItem
    {
        public IProduct Product { get; set; }
        public int Count { get; set; }
        public float Price { get; set; }
    }

    internal class Order
    {
        public IEnumerable<OrderItem> Items { get; set; }
        public Person Client { get; set; }
    }

    internal class ProductCategory
    {
        public IEnumerable<IProduct> Products { get; set; }
    }

    internal class OrderHistory : IEnumerable<Order>
    {
        private readonly List<Order> _orders;

        public OrderHistory(IEnumerable<Order> orders = null)
        {
            _orders = (orders != null) ? new List<Order>(orders) : new List<Order>();
        }


        public IEnumerator<Order> GetEnumerator()
        {
            return _orders.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return _orders.GetEnumerator();
        }

        public void AddOrder(Order order)
        {
            _orders.Add(order);
        }
    }

    internal class Account
    {
        public OrderHistory OrderHistory { get; set; }
    }
}
﻿using System;
using System.Collections.Generic;
using System.Linq;
using InfinniPlatform.Core.Properties;
using InfinniPlatform.Index.ElasticSearch.Factories;
using InfinniPlatform.Index.ElasticSearch.Implementation.ElasticProviders;
using InfinniPlatform.Index.ElasticSearch.Tests.Builders;
using InfinniPlatform.Sdk.Dynamic;
using NUnit.Framework;

namespace InfinniPlatform.Index.ElasticSearch.Tests.ElasticWrappers
{
    [TestFixture]
    [Category(TestCategories.UnitTest)]
    public sealed class ElasticSearchCrudProviderBehavior
    {
        [Test]
        public void ShouldCrudOperations()
        {
            var elasticTypeManager = ElasticFactoryBuilder.ElasticTypeManager.Value;
            elasticTypeManager.DeleteType("elasticcrudbehavior", "testtype");
            elasticTypeManager.CreateType("elasticcrudbehavior", "testtype");
            elasticTypeManager.DeleteType("elasticcrudbehavior", "testtype1");
            elasticTypeManager.CreateType("elasticcrudbehavior", "testtype1");
            elasticTypeManager.DeleteType("elasticcrudbehavior1", "testtype");
            elasticTypeManager.CreateType("elasticcrudbehavior1", "testtype");
            elasticTypeManager.DeleteType("elasticcrudbehavior1", "testtype_another");
            elasticTypeManager.CreateType("elasticcrudbehavior1", "testtype_another");

            var elasticFactory = ElasticFactoryBuilder.GetElasticFactory();
            var elasticProvider1 = elasticFactory.BuildCrudOperationProvider("elasticcrudbehavior", "testtype");
            var elasticProvider2 = elasticFactory.BuildCrudOperationProvider("elasticcrudbehavior", "testtype1");
            var elasticProvider3 = elasticFactory.BuildCrudOperationProvider("elasticcrudbehavior1", "testtype");
            var elasticProvider4 = elasticFactory.BuildCrudOperationProvider("elasticcrudbehavior1", "testtype_another");

            dynamic instance1 = new DynamicWrapper();
            string instance1Id = "1elasticcrudbehavior_testtype_first";
            string simpleField1 = "first element (string!)";
            instance1.Id = instance1Id;
            instance1.SimpleField = simpleField1;

            elasticProvider1.Set(instance1);
            elasticProvider1.Refresh();

            IEnumerable<dynamic> items = elasticProvider1.GetItems(new[] {instance1Id});

            Assert.AreEqual(1, items.Count());

            dynamic item = elasticProvider1.GetItem(instance1Id);

            Assert.IsNotNull(item);
            Assert.AreEqual(simpleField1, item.SimpleField);

            int count = elasticProvider1.GetTotalCount();
            Assert.AreEqual(1, count);

            //добавим вторую сущность в индекс
            dynamic instance2 = new DynamicWrapper();
            string instance2Id = "2elasticcrudbehavior_testtype_second";
            string simpleField2 = "second element (string!)";
            instance2.Id = instance2Id;
            instance2.SimpleField = simpleField2;
            elasticProvider1.Set(instance2);
            elasticProvider1.Refresh();

            item = elasticProvider1.GetItem(instance2Id);
            Assert.IsNotNull(item);
            Assert.AreEqual(item.SimpleField, simpleField2);

            items = elasticProvider1.GetItems(new[] {instance1Id, instance2Id});
            Assert.AreEqual(2, items.Count());

            //проверяем сортировку по возрастанию в результате
            Assert.AreEqual(instance1Id, items.First().Id);
            Assert.AreEqual(instance2Id, items.Skip(1).First().Id);

            //проверяем апдейт записей
            string simpleFieldUpdate = "field updated";
            instance1.SimpleField = simpleFieldUpdate;

            elasticProvider1.Set(instance1);
            elasticProvider1.Refresh();

            item = elasticProvider1.GetItem(instance1Id);
            Assert.AreEqual(simpleFieldUpdate, item.SimpleField);

            //проверяем, что новых записей не появилось
            count = elasticProvider1.GetTotalCount();
            Assert.AreEqual(2, count);

            //проверяем индексирование пачки объектов
            dynamic instance3 = new DynamicWrapper();
            string instance3Id = "3elasticcrudbehavior_testtype_third";
            string simpleField3 = "third element (string!)";
            instance3.Id = instance3Id;
            instance3.SimpleField = simpleField3;
            //добавляем поле для последующей проверки того, что не удастся индексировать сущность с несовместимым маппингом поля
            instance3.ForMappingCheck = 1;

            elasticProvider1.SetItems(new[] {instance1, instance2, instance3});
            elasticProvider1.Refresh();

            count = elasticProvider1.GetTotalCount();
            Assert.AreEqual(3, count);

            dynamic[] itemsArr = elasticProvider1.GetItems(new[] {instance1Id, instance2Id, instance3Id}).ToArray();
            Assert.AreEqual(3, itemsArr.Count());
            Assert.True(itemsArr.Any(a => a.Id == instance1Id));
            Assert.True(itemsArr.Any(a => a.Id == instance2Id));
            Assert.True(itemsArr.Any(a => a.Id == instance3Id));

            //добавляем в индекс данные других типов
            dynamic instance4 = new DynamicWrapper();
            string instance4Id = "4elasticcrudbehavior_testtype1_fourth";
            string simpleField4 = "fourth element (string!)";
            instance4.Id = instance4Id;
            instance4.SimpleField = simpleField4;

            elasticProvider2.Set(instance4);
            elasticProvider2.Refresh();

            items = elasticProvider2.GetItems(new[] {instance4Id});

            Assert.AreEqual(1, items.Count());
            Assert.AreEqual(instance4Id, items.First().Id);

            count = elasticProvider2.GetTotalCount();
            Assert.AreEqual(1, count);

            //проверяем, что другие провайдеры не находят новых данных
            count = elasticProvider1.GetTotalCount();
            Assert.AreEqual(3, count);

            //индексируем данные с другим маппингом и получаем ошибку
            dynamic failInstance = new DynamicWrapper();
            string failId = "5-" + Guid.NewGuid();
            failInstance.Id = failId;
            failInstance.ForMappingCheck = DateTime.Now;

            Assert.Throws<ArgumentException>(() => elasticProvider1.Set(failInstance),
                                             Resources.InappropriateItemMapping);

            //создаем новую версию маппинга для elasticcrudbehavior, testtype
            elasticTypeManager.CreateType("elasticcrudbehavior", "testtype");

            //успешно добавляем в него инстанс, который ранее выдавал ошибку маппинга
            elasticProvider1 = ElasticFactoryBuilder.GetElasticFactory().BuildCrudOperationProvider("elasticcrudbehavior", "testtype");
			elasticProvider1.Set(failInstance);
            elasticProvider1.Refresh();

            //проверяем, что выборка содержит данные и из старой, и из новой версии маппинга
            count = elasticProvider1.GetTotalCount();
            Assert.AreEqual(4, count);

            itemsArr = elasticProvider1.GetItems(new[] {instance1Id, instance2Id, instance3Id, failId}).ToArray();

            //проверяем case-insensitive поиск по индексу

            item = elasticProvider1.GetItem(instance1Id.ToUpperInvariant());
            Assert.IsNotNull(item);

            //проверяем поиск по Guid
            item = elasticProvider1.GetItem(failId);
            Assert.IsNotNull(item);

            Assert.AreEqual(4, itemsArr.Count());

            //проверяем, что другие провайдеры не находят данных
            IEnumerable<dynamic> notExistingItems = elasticProvider2.GetItems(new[] {instance1Id});
            Assert.AreEqual(0, notExistingItems.Count());
            notExistingItems = elasticProvider3.GetItems(new[] {instance1Id});
            Assert.AreEqual(0, notExistingItems.Count());
            notExistingItems = elasticProvider4.GetItems(new[] {instance1Id});
            Assert.AreEqual(0, notExistingItems.Count());

            //удаляем данные

            elasticProvider1.Remove(instance1Id);
            elasticProvider1.Refresh();

            //проверяем, что сущность с указанным идентификатором удалилась
            item = elasticProvider1.GetItem(instance1Id);
            Assert.IsNull(item);

            //проверяем, что в списке нет удаленной сущности
            items = elasticProvider1.GetItems(new[] {instance1Id});
            Assert.AreEqual(0, items.Count());
        }
    }
}
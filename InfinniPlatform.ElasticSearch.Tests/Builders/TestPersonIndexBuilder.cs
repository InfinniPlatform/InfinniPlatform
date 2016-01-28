using InfinniPlatform.Sdk.Dynamic;

namespace InfinniPlatform.ElasticSearch.Tests.Builders
{
    public static class TestPersonIndexBuilder
    {
        public static void BuildIndexObjectForSearchModelAndSetItems(string indexName, string typeName)
        {
            var elasticConnection = ElasticFactoryBuilder.ElasticConnection.Value;

            dynamic test = new DynamicWrapper();
            test.Id = "898989";
            test.LastName = "Иванов";
            test.FirstName = "Степан";
            test.Patronimic = "Степанович";
            test.NestedObj = new DynamicWrapper();
            test.NestedObjId = "111";
            test.NestedObj.Code = "12345";
            test.NestedObj.Name = "test123";
            elasticConnection.Client.Index((object)test, i => i.Index(indexName.ToLower()).Type(typeName.ToLower()));

            dynamic test1 = new DynamicWrapper();
            test1.Id = "24342";
            test1.LastName = "Иванов";
            test1.FirstName = "Владимир";
            test1.Patronimic = "Степанович";
            test1.NestedObj = new DynamicWrapper();
            test1.NestedObj.Id = "112";
            test1.NestedObj.Code = "12345";
            test1.NestedObj.Name = "test12345";
            elasticConnection.Client.Index((object)test1, i => i.Index(indexName.ToLower()).Type(typeName.ToLower()));

            dynamic test2 = new DynamicWrapper();
            test2.Id = "83453";
            test2.LastName = "Петров";
            test2.FirstName = "Федор";
            test2.Patronimic = "Сергеевич";
            test2.NestedObj = new DynamicWrapper();
            test2.NestedObj.Id = "235";
            test2.NestedObj.Code = "1232342";
            test2.NestedObj.Name = "test2456";
            elasticConnection.Client.Index((object)test2, i => i.Index(indexName.ToLower()).Type(typeName.ToLower()));

            elasticConnection.Refresh();
        }
    }
}
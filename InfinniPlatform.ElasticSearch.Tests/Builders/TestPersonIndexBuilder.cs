using InfinniPlatform.Sdk.Dynamic;

namespace InfinniPlatform.ElasticSearch.Tests.Builders
{
    public static class TestPersonIndexBuilder
    {
        public static void BuildIndexObjectForSearchModelAndSetItems(string indexName, string typeName)
        {
            var elasticConnection = ElasticFactoryBuilder.ElasticConnection.Value;

            dynamic test = ElasticDocument.Create();
            test.Values.Id = "898989";
            test.Values.LastName = "Иванов";
            test.Values.FirstName = "Степан";
            test.Values.Patronimic = "Степанович";
            test.Values.NestedObj = new DynamicWrapper();
            test.Values.NestedObjId = "111";
            test.Values.NestedObj.Code = "12345";
            test.Values.NestedObj.Name = "test123";
            elasticConnection.Client.Index((object)test, i => i.Index(indexName.ToLower()).Type(typeName.ToLower()));

            dynamic test1 = ElasticDocument.Create();
            test1.Values.Id = "24342";
            test1.Values.LastName = "Иванов";
            test1.Values.FirstName = "Владимир";
            test1.Values.Patronimic = "Степанович";
            test1.Values.NestedObj = new DynamicWrapper();
            test1.Values.NestedObj.Id = "112";
            test1.Values.NestedObj.Code = "12345";
            test1.Values.NestedObj.Name = "test12345";
            elasticConnection.Client.Index((object)test1, i => i.Index(indexName.ToLower()).Type(typeName.ToLower()));

            dynamic test2 = ElasticDocument.Create();
            test2.Values.Id = "83453";
            test2.Values.LastName = "Петров";
            test2.Values.FirstName = "Федор";
            test2.Values.Patronimic = "Сергеевич";
            test2.Values.NestedObj = new DynamicWrapper();
            test2.Values.NestedObj.Id = "235";
            test2.Values.NestedObj.Code = "1232342";
            test2.Values.NestedObj.Name = "test2456";
            elasticConnection.Client.Index((object)test2, i => i.Index(indexName.ToLower()).Type(typeName.ToLower()));

            elasticConnection.Refresh();
        }
    }
}
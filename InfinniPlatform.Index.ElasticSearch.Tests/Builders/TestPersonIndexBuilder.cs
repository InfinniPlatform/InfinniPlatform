using System;
using System.Collections.Generic;
using System.Linq;
using InfinniPlatform.Api.RestApi.Auth;
using InfinniPlatform.Index.ElasticSearch.Factories;
using InfinniPlatform.Sdk.Dynamic;
using InfinniPlatform.Sdk.Environment.Index;

namespace InfinniPlatform.Index.ElasticSearch.Tests.Builders
{
    public class TestPersonIndexBuilder
    {

        private ICrudOperationProvider _elasticSearchProvider;
        private IIndexStateProvider _indexStateProvider;


        public void BuildTestPersonIndex(string indexName)
        {
            var factory = new ElasticFactory();
            _indexStateProvider = factory.BuildIndexStateProvider();
			_indexStateProvider.RecreateIndex(indexName,indexName);

			_elasticSearchProvider = factory.BuildCrudOperationProvider(indexName, indexName, null);
        }

		public void BuildIndexObjectForTestPersonsAndSetItems(string indexName)
		{
			BuildTestPersonIndex(indexName);
			var items = GetItemsToIndex().ToList();
			_elasticSearchProvider.SetItems(items);
			_elasticSearchProvider.Refresh();
		}

		public void BuildIndexObjectForSearchModelAndSetItems(string indexName)
		{
			BuildTestPersonIndex(indexName);
			_elasticSearchProvider.SetItems(GetItemsToSearchByModel());
			_elasticSearchProvider.Refresh();
		}

		public void BuildIndexObjectTimestamps()
		{
			BuildTestPersonIndex("testperson");
			_elasticSearchProvider.SetItems(GetItemsStatistical());
			_elasticSearchProvider.Refresh();			
		}


		public IEnumerable<dynamic> GetItemsToIndexVersionFirst()
		{
            dynamic test = new DynamicWrapper();
			test.Id = "898989";
			test.LastName = "ИвановFirst";
			test.FirstName = "Степан";
			test.Patronimic = "Степанович";
            test.NestedObj = new DynamicWrapper();
			test.NestedObj.Id = "111";
			test.NestedObj.Code = "12345";
			test.NestedObj.Name = "test123";

            dynamic test1 = new DynamicWrapper();
			test1.Id = "83453";
			test1.LastName = "Петров";
			test1.FirstName = "Владимир";
			test1.Patronimic = "Степанович";
            test1.NestedObj = new DynamicWrapper();
			test1.NestedObj.Id = "235";
			test1.NestedObj.Code = "1232342";
			test1.NestedObj.Name = "test2456";

			return new dynamic[]
                {
                    test,
                    test1,
                };

		} 

		public IEnumerable<dynamic> GetItemsToIndexVersionSecond()
		{

            dynamic test2 = new DynamicWrapper();
			test2.Id = "8989810";
			test2.LastName = "ИвановSecond";
			test2.FirstName = "Степан";
			test2.Patronimic = "Степанович";
            test2.NestedObj = new DynamicWrapper();
			test2.NestedObj.Id = "222";
			test2.NestedObj.Code = "67890";
			test2.NestedObj.Name = "test456";

            dynamic test3 = new DynamicWrapper();
			test3.Id = "324323";
			test3.LastName = "Николаев";
			test3.FirstName = "Ivan";
			test3.Patronimic = "Федорович";
            test3.NestedObj = new DynamicWrapper();
			test3.NestedObj.Id = "438";
			test3.NestedObj.Code = "samecode";
			test3.NestedObj.Name = "test2342";
			test3.NestedObj.Count = 2;

			return new dynamic[]
                {
                    test2,
                    test3
                };

		} 

		public IEnumerable<dynamic> GetItemsToSearchByModel()
		{
            dynamic test = new DynamicWrapper();
			test.Id = "898989";
			test.LastName = "Иванов";
			test.FirstName = "Степан";
			test.Patronimic = "Степанович";
            test.NestedObj = new DynamicWrapper();
			test.NestedObjId = "111";
			test.NestedObj.Code = "12345";
			test.NestedObj.Name = "test123";

            dynamic test1 = new DynamicWrapper();
			test1.Id = "24342";
			test1.LastName = "Иванов";
			test1.FirstName = "Владимир";
			test1.Patronimic = "Степанович";
            test1.NestedObj = new DynamicWrapper();
			test1.NestedObj.Id = "112";
			test1.NestedObj.Code = "12345";
			test1.NestedObj.Name = "test12345";

            dynamic test2 = new DynamicWrapper();
			test2.Id = "83453";
			test2.LastName = "Петров";
			test2.FirstName = "Федор";
			test2.Patronimic = "Сергеевич";
            test2.NestedObj = new DynamicWrapper();
			test2.NestedObj.Id = "235";
			test2.NestedObj.Code = "1232342";
			test2.NestedObj.Name = "test2456";

			return new List<dynamic>()
				       {
					       test,
						   test1,
						   test2
				       };
		} 


		public IEnumerable<dynamic> GetItemsStatistical()
		{
            dynamic id1_v1 = new DynamicWrapper();
			id1_v1.Id = "3091F1AD-90DC-4A59-A0E3-BB4AE1AD1D6A";
			id1_v1.TimeStamp = new DateTime(2001, 12, 12);
			id1_v1.Version = "id1_v1";
			id1_v1.CriteriaField = "Иванов";
			id1_v1.CommonField = "1";

            dynamic id1_v2 = new DynamicWrapper();
			id1_v2.Id = "3091F1AD-90DC-4A59-A0E3-BB4AE1AD1D6A";
			id1_v2.TimeStamp = new DateTime(2002, 12, 12);
			id1_v2.Version = "id1_v2";
			id1_v2.CriteriaField = "Иванов";
			id1_v2.CommonField = "1";

            dynamic id1_v3 = new DynamicWrapper();
			id1_v3.Id = "3091F1AD-90DC-4A59-A0E3-BB4AE1AD1D6A";
			id1_v3.TimeStamp = new DateTime(2003, 12, 12);
			id1_v3.Version = "id1_v3";
			id1_v3.CriteriaField = "Иванов";
			id1_v3.CommonField = "1";

            dynamic id2_v1 = new DynamicWrapper();
			id2_v1.Id = "B9988DDF-A562-4279-89E5-2B6F500EA693";
			id2_v1.TimeStamp = new DateTime(2005, 12, 12);
			id2_v1.Version = "id2_v1";
			id2_v1.CriteriaField = "Петров";
			id2_v1.CommonField = "1";

            dynamic id2_v2 = new DynamicWrapper();
			id2_v2.Id = "B9988DDF-A562-4279-89E5-2B6F500EA693";
			id2_v2.TimeStamp = new DateTime(2001, 12, 12);
			id2_v2.Version = "id2_v2";
			id2_v2.CriteriaField = "Петров";
			id2_v2.CommonField = "1";

            dynamic id3_v1 = new DynamicWrapper();
			id3_v1.Id = "F1455A44-F153-43CA-AFAC-A6BA2403ABF9";
			id3_v1.TimeStamp = new DateTime(2005, 12, 12);
			id3_v1.Version = "id3_v1";
			id3_v1.CriteriaField = "Сидоров";
			id3_v1.CommonField = "1";

            dynamic id3_v2 = new DynamicWrapper();
			id3_v2.Id = "F1455A44-F153-43CA-AFAC-A6BA2403ABF9";
			id3_v2.TimeStamp = new DateTime(2001, 12, 12);
			id3_v2.Version = "id3_v2";
			id3_v2.CriteriaField = "Сидоров";
			id3_v2.CommonField = "1";

			return new List<dynamic>()
				       {
					       id1_v1,
					       id1_v2,
					       id1_v3,
					       id2_v1,
					       id2_v2,
					       id3_v1,
					       id3_v2
				       };
		} 



        public IEnumerable<dynamic> GetItemsToIndex()
        {
            dynamic test = new DynamicWrapper();
	        test.Id = "898989";
	        test.LastName = "Иванов";
	        test.FirstName = "Степан";
	        test.Patronimic = "Степанович";
            test.NestedObj = new DynamicWrapper();
			test.NestedObj.Id = "111";
			test.NestedObj.Code = "12345";
			test.NestedObj.Name = "test123";

            dynamic test1 = new DynamicWrapper();
	        test1.Id = "83453";
	        test1.LastName = "Петров";
	        test1.FirstName = "Владимир";
	        test1.Patronimic = "Степанович";
            test1.NestedObj = new DynamicWrapper();
			test1.NestedObj.Id = "235";
			test1.NestedObj.Code = "1232342";
			test1.NestedObj.Name = "test2456";

            dynamic test2 = new DynamicWrapper();
	        test2.Id = "7fed07db-5d53-d98a-783a-e52ba01f319a";
	        test2.LastName = "Сидоров";
	        test2.FirstName = "Петр";
	        test2.Patronimic = "Федорович";
	        test2.Another = "234233";
            test2.NestedObj = new DynamicWrapper();
			test2.NestedObj.Id = "438";
			test2.NestedObj.Code = "samecode";
			test2.NestedObj.Count = 7;
			test2.NestedObj.Name = "test1464";

            dynamic test3 = new DynamicWrapper();
	        test3.Id = "324323";
	        test3.LastName = "Николаев";
	        test3.FirstName = "Ivan";
	        test3.Patronimic = "Федорович";
            test3.NestedObj = new DynamicWrapper();
			test3.NestedObj.Id = "438";
	        test3.NestedObj.Code = "samecode";
			test3.NestedObj.Name = "test2342";
			test3.NestedObj.Count = 2;

               return new []
                {
                    test,
                    test1,
                    test2,
                    test3
                };
        }
    }
}

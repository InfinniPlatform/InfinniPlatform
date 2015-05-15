
namespace InfinniPlatform.Hosting.WebApi.Tests.Builders
{
    public class TestPerson
    {
		//[ElasticProperty(AddSortField = true, Index = FieldIndexOption.not_analyzed)]
        public string Id { get; set; }

        //[ElasticProperty(AddSortField = true, Index = FieldIndexOption.not_analyzed)]
        public string FirstName { get; set; }

		//[ElasticProperty(AddSortField = true, Index = FieldIndexOption.not_analyzed)]
		public string LastNameSort { get; set; }

        //[ElasticProperty(AddSortField = true, Index = FieldIndexOption.not_analyzed)]
        public string Patronimic { get; set; }

        //[ElasticProperty(AddSortField = true)]
        public string LastName { get; set; }

        //[ElasticProperty(Type = FieldType.nested)]
        public NestedObj NestedObj { get; set; }

		//[ElasticProperty(AddSortField = true, Index = FieldIndexOption.not_analyzed)]
		public string HospitalId { get; set; }

        public string Another { get; set; }
    }

    public class NestedObj
    {
        public string Id { get; set; }

        public string Code { get; set; }

        public string Name { get; set; }

        public int Count { get; set; }
    }

}

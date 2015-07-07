using System;

namespace InfinniPlatform.Index
{
    public class IndexObject
    {
        //не удается создать case-insensitive Id
        //метод ES Get<> использует внутренний формируемый _id для поиска, который не case-insensitive (см. http://elasticsearch-users.115913.n3.nabble.com/Lowercase-id-field-td2843108.html)
        
        public string Id { get; set; }

        public string TenantId { get; set; }
	
		public DateTime TimeStamp { get; set; }
        
        public dynamic Values { get; set; }

        public string Status { get; set; }
    }
}

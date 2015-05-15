using System.Collections.Generic;

namespace InfinniPlatform.Core.Tests.Events.Builders.Entities
{
	public class ObjectMetadataRecord  {

		public virtual int Id { get; set; }

        public virtual FieldRecord FieldCode { get; set; }

        public virtual FieldRecord FieldName { get; set; }

		public virtual IEnumerable<ObjectMetadataRecord> DetailTables { get; set; }

		//public IEnumerable<ActionMetadata> ActionsMetadata { get; set; } 
	}
}
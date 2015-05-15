namespace InfinniPlatform.Core.Tests.Events.Builders.Entities
{
	public class FieldRecord
	{
		public virtual int Id { get; set; }

		public virtual FieldMetadataRecord FieldMetadata { get; set; }

		public virtual string Value { get; set; }

		
	}
}
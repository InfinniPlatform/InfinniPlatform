namespace InfinniPlatform.Core.Tests.Events.Builders.Entities {

    public class FieldMetadataRecord     
    {
        public virtual int Id { get; set; }

		public virtual bool IdTree { get; set; }

		public virtual bool IdTreeParent { get; set; }

		public virtual ObjectMetadataRecord Parent { get; set; }

        public virtual string MetadataId { get; set; }

        public virtual string MetadataName { get; set; }

        public virtual MetadataDataType MetadataDataType { get; set; }

		public virtual bool IsEditable { get; set; }

		public virtual bool IsFilterable { get; set; }		

		public virtual string FilterControlType { get; set; }

		public virtual string DataFieldName { get; set; }

		public virtual bool IsIdentifier { get; set; }

		public virtual VisualTemplate VisualTemplate { get; set; }
    }

}
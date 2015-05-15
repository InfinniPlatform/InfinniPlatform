using InfinniPlatform.Core.Tests.Events.Builders.Entities;

namespace InfinniPlatform.Core.Tests.Events.Builders.Extensibility.FormsMetadata
{
	public class GridColumnMetadata
	{
		public string ColumnName { get; set; }

		public bool IsVisible { get; set; }

		public bool IsEditable { get; set; }

		public string SortOrder { get; set; }

		public int Position { get; set; }

		public int Width { get; set; }

		public string Caption { get; set; }

		public FieldMetadataRecord DataField { get; set; }

		public string FieldName { get; set; }

		public string EditorType { get; set; }
	}
}
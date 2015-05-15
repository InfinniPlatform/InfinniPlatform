using System.Collections.Generic;

namespace InfinniPlatform.Core.Tests.Events.Builders.Entities
{
	public class MetadataDataType
	{
		/// <summary>
		///   "SimpleType" or "ReferenceType"
		/// </summary>
		public virtual string MetadataTypeKind { get; set; }

		/// <summary>
		///   "string" or "REF_VIDAL" or...
		/// </summary>
		public virtual string MetadataIdentifier { get; set; }

		/// <summary>
		///   predefined metadata values
		/// </summary>
		public virtual IEnumerable<object> MetadataValues { get; set; }
	}
}

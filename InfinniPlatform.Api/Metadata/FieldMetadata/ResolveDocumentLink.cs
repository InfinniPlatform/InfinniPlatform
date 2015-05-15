using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InfinniPlatform.Api.Metadata.FieldMetadata
{
	/// <summary>
	///   Объект жесткой ссылки на подчиненный документ
	/// </summary>
	public sealed class ResolveDocumentLink
	{
		private readonly string _propertyName;
		private readonly string _configId;
		private readonly string _documentId;
		private readonly dynamic _parentDocument;
		private readonly string _childDocumentId;

		public ResolveDocumentLink(string propertyName, string configId, string documentId, dynamic parentDocument, string childDocumentId)
		{
			_propertyName = propertyName;
			_configId = configId;
			_documentId = documentId;
			_parentDocument = parentDocument;
			_childDocumentId = childDocumentId;
		}

		public void Resolve(dynamic resolvedReference)
		{
			_parentDocument[_propertyName] = resolvedReference;
		}

		public string ConfigId
		{
			get { return _configId; }
		}

		public string DocumentId
		{
			get { return _documentId; }
		}

		public dynamic ParentDocument
		{
			get { return _parentDocument; }
		}

		public string ChildDocumentId
		{
			get { return _childDocumentId; }
		}
	}
}

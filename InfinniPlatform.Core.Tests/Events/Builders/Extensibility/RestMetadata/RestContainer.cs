using System.Collections.Generic;

namespace InfinniPlatform.Core.Tests.Events.Builders.Extensibility.RestMetadata
{
	public class RestContainer
	{
		private readonly string _containerType;
		private readonly string _metadataId;
		private readonly string _methodType;
		private readonly string _controllerName;
		private readonly IEnumerable<string> _arguments;
		private readonly string _caption;

		public RestContainer(string containerType, string metadataId, string methodType, string controllerName, IEnumerable<string> arguments, string caption = null) {
			_containerType = containerType;
			_metadataId = metadataId;
			_methodType = methodType;
			_controllerName = controllerName;
			_arguments = arguments;
			_caption = caption;
		}

		public bool Satisfy(string metadataId) {
			return MetadataId == metadataId;
		}

		public string ContainerType {
			get { return _containerType; }		
		}


		public IEnumerable<string> Arguments {
			get { return _arguments; }
		}

		public string ControllerName {
			get { return _controllerName; }
		}

		public string MethodType {
			get { return _methodType; }
		}

		public string MetadataId {
			get { return _metadataId; }
		}

		public string Caption {
			get { return _caption; }
		}
	}
}
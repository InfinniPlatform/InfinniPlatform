using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using InfinniPlatform.Api.ContextComponents;
using InfinniPlatform.Api.Factories;
using InfinniPlatform.BlobStorage;

namespace InfinniPlatform.ContextComponents
{
	public sealed class BlobStorageComponent : IBlobStorageComponent
	{
		private readonly IBlobStorage _blobStorage;

		public BlobStorageComponent(IBlobStorage blobStorage)
		{
			_blobStorage = blobStorage;
		}

		public IBlobStorage GetBlobStorage()
		{
			return _blobStorage;
		}
	}
}

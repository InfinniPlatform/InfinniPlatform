using System;
using System.Net;
using System.Net.Http;
using System.Web.Http;

using InfinniPlatform.BlobStorage;
using InfinniPlatform.Factories;

namespace InfinniPlatform.Hosting.WebApi.Implementation.Controllers
{
	public class FileController : ApiController
	{
		private readonly IBlobStorage _blobStorage;

		public FileController(IBlobStorageFactory blobStorageFactory)
		{
			_blobStorage = blobStorageFactory.CreateBlobStorage();
		}

		[HttpGet]
		public HttpResponseMessage Download(Guid id)
		{
			var data = _blobStorage.GetData(id, "Integration.DocumentContent");

			return new HttpResponseMessage(HttpStatusCode.OK)
				 {
					 Content = data != null ? new ByteArrayContent(data) : new ByteArrayContent(new byte[] { })
				 };
		}
	}
}
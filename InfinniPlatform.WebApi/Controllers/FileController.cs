using System;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Web.Http;
using InfinniPlatform.Api.Factories;
using InfinniPlatform.BlobStorage;
using InfinniPlatform.Factories;

namespace InfinniPlatform.WebApi.Controllers
{
	public class FileController : ApiController
	{
		public FileController(IBlobStorageFactory blobStorageFactory)
		{
			_blobStorage = blobStorageFactory.CreateBlobStorage();
		}


		private readonly IBlobStorage _blobStorage;


		[HttpGet]
		public HttpResponseMessage Download(Guid id)
		{
			var blobData = _blobStorage.GetBlobData(id);

			if (blobData != null)
			{
				return new HttpResponseMessage(HttpStatusCode.OK)
					   {
						   Content = new ByteArrayContent(blobData.Data)
									 {
										 Headers =
										 {
											 ContentLength = blobData.Info.Size,
											 ContentType = new MediaTypeHeaderValue(blobData.Info.Type)
										 }
									 }
					   };
			}

			return new HttpResponseMessage(HttpStatusCode.NotFound);
		}
	}
}
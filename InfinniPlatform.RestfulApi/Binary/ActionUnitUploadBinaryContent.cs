using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using InfinniPlatform.Api.ContextComponents;
using InfinniPlatform.Api.ContextTypes;
using InfinniPlatform.Api.Dynamic;
using InfinniPlatform.Api.RestApi.DataApi;
using InfinniPlatform.Api.SearchOptions.Builders;
using InfinniPlatform.Api.Transactions;

namespace InfinniPlatform.RestfulApi.Binary
{
	/// <summary>
	///   Модуль загрузки бинарных данных на сервер
	/// </summary>
	public sealed class ActionUnitUploadBinaryContent
	{
		private byte[] fileContent;

		private static byte[] ReadAllBytes(Stream input)
		{
			byte[] buffer = new byte[16 * 1024];
			using (MemoryStream ms = new MemoryStream())
			{
				int read;
				while ((read = input.Read(buffer, 0, buffer.Length)) > 0)
				{
					ms.Write(buffer, 0, read);
				}
				return ms.ToArray();
			}
		}

		public void Action(IUploadContext target)
		{

            dynamic documentWithBinaryField = new DocumentApi().GetDocument(target.LinkedData.InstanceId);

			if (documentWithBinaryField == null)
			{
				target.IsValid = false;
				target.ValidationMessage = string.Format("Document with identifier {0}, not found.",
					target.LinkedData.InstanceId);
				return;
			}

            fileContent = ReadAllBytes(target.FileContent);

		    new BinaryManager(target.Context.GetComponent<IBlobStorageComponent>().GetBlobStorage()).SaveBinary(new [] {documentWithBinaryField}, 
                documentWithBinaryField.__ConfigId,
                documentWithBinaryField.__DocumentId,
		        target.LinkedData.FieldName, fileContent);
		}
	}
}

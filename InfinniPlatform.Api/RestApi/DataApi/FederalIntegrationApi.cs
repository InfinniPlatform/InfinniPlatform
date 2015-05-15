using System;
using System.Net;
using InfinniPlatform.Api.Dynamic;
using InfinniPlatform.Api.RestQuery;
using InfinniPlatform.Api.RestQuery.RestQueryBuilders;
using RestSharp;

namespace InfinniPlatform.Api.RestApi.DataApi
{
	public static class FederalIntegrationApi
	{
		public static dynamic PublishPatient(object item, bool replacePatient)
		{
			var builder = new RestQueryBuilder("federalintegration", "patient", "publish", null);

			dynamic param = item.ToDynamic();

			param.ReplacePatient = replacePatient;

			var response = builder.QueryPost(null, item, false, null);

			if (!response.IsAllOk)
			{
				throw new ArgumentException(response.Content);
			}
			return response.ToDynamic();
		}

        public static dynamic SearchPatient(object item)
        {
            var builder = new RestQueryBuilder("federalintegration", "patient", "search", null);

            var response = builder.QueryPost(null, item, false, null);

            if (!response.IsAllOk)
            {
                throw new ArgumentException(response.Content);
            }
            return response.ToDynamic();
        }

		public static dynamic PublishDocument(object documentId)
		{
			var builder = new RestQueryBuilder("federalintegration", "document", "publish", null);

			var response = builder.QueryPost(null, new
				                                       {
					                                       DocumentId = documentId
				                                       }, false, null);

			if (!response.IsAllOk)
			{
				throw new ArgumentException(response.Content);
			}
			return response.ToDynamic();
		}

		public static dynamic SearchDocument(string clientEntityId, string organizationOid, string documentId)
		{
			var builder = new RestQueryBuilder("federalintegration", "document", "search", null);

			var response = builder.QueryPost(null, new
				                                       {
					                                       DocumentId = documentId,
					                                       ClientEntityId = clientEntityId,
					                                       OrganizationOid = organizationOid
				                                       }, false, null);

			if (!response.IsAllOk)
			{
				throw new ArgumentException(response.Content);
			}
			
			return response.ToDynamic();
		}

		public static dynamic SearchDocumentList(string clientEntityId, string organizationOid,string patientId)
		{
			var builder = new RestQueryBuilder("federalintegration", "document", "list", null);

			var response = builder.QueryPost(null, new
				                                       {
					                                       PatientId = patientId,
					                                       ClientEntityId = clientEntityId,
					                                       OrganizationOid = organizationOid
				                                       }, false, null);

			if (!response.IsAllOk)
			{
				throw new ArgumentException(response.Content);
			}
			return response.ToDynamic();
		}

		public static void PublishDocumentFederal(object documentModel)
		{
			string routing = ControllerRoutingFactory.Instance.GetCustomRouting("Api/FederalIntegration/PublishDocument");
			var restClient = new RestClient(routing);
			IRestResponse restResponse = restClient.Post(new RestRequest { RequestFormat = DataFormat.Json }
															.AddBody(documentModel));
			if (restResponse.StatusCode != HttpStatusCode.OK)
			{
				throw new ArgumentException(restResponse.Content);
			}
		}

		public static dynamic SearchDocumentFederal(string clientEntityId, string documentId)
		{
			string routing = ControllerRoutingFactory.Instance.GetCustomRouting("Api/FederalIntegration/SearchDocument");
			var restClient = new RestClient(routing);
			IRestResponse restResponse = restClient.Post(new RestRequest { RequestFormat = DataFormat.Json }
															.AddBody(new {
																			DocumentUniqueIdentifier = documentId,
    																		ClientEntityId = clientEntityId
																         }));
			if (restResponse.StatusCode != HttpStatusCode.OK)
			{
				throw new ArgumentException(restResponse.Content);
			}
			return restResponse.Content;
		}

		public static dynamic SearchDocumentListFederal(object searchListModel)
		{
			string routing = ControllerRoutingFactory.Instance.GetCustomRouting("Api/FederalIntegration/SearchDocumentList");
			var restClient = new RestClient(routing);
			IRestResponse restResponse = restClient.Post(new RestRequest { RequestFormat = DataFormat.Json }
															.AddBody(searchListModel));
			if (restResponse.StatusCode != HttpStatusCode.OK)
			{
				throw new ArgumentException(restResponse.Content);
			}
			return restResponse.Content;
		}
	}
}

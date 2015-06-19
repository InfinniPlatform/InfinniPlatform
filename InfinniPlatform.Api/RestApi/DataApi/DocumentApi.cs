using InfinniPlatform.Api.Dynamic;
using InfinniPlatform.Api.Linq;
using InfinniPlatform.Api.RestApi.CommonApi;
using System;
using System.Collections.Generic;
using System.Linq;
using InfinniPlatform.Api.SearchOptions.Builders;

namespace InfinniPlatform.Api.RestApi.DataApi
{
    public class DocumentApi
    {
	    private readonly bool _secured;

	    public DocumentApi(bool secured = true)
		{
			_secured = secured;
		}

	    public IEnumerable<dynamic> GetDocumentByQuery(string queryText, bool denormalizeResult = false)
		{
			dynamic body = new DynamicWrapper();
			body.QueryText = queryText;
		    body.DenormalizeResult = denormalizeResult;
			var response = RestQueryApi.QueryPostJsonRaw("RestfulApi", "configuration", "getbyquery", null, body,false);

			return response.ToDynamicList();
		}

        /// <summary>
        /// Получение количества документов
        /// </summary>
        public int GetNumberOfDocuments(string configuration, string metadata, dynamic filter)
        {
            var response = RestQueryApi.QueryPostJsonRaw("RestfulApi", "configuration", "getnumberofdocuments", null, new
            {
                Configuration = configuration,
                Metadata = metadata,
                Filter = filter,
                Secured = _secured
            });

            try
            {
                return Convert.ToInt32(response.ToDynamic()[0].NumberOfDocuments);
            }
            catch (Exception)
            {
                throw new ArgumentException(response.Content);
            }
        }

        /// <summary>
        /// Получение количества документов
        /// </summary>
        public int GetNumberOfDocuments(string configuration, string metadata, Action<FilterBuilder> filter)
        {
            var filterBuilder = new FilterBuilder();

            if (filter != null)
            {
                filter.Invoke(filterBuilder);
            }
            
            return GetNumberOfDocuments(configuration, metadata, filterBuilder.GetFilter());     
        }

		public IEnumerable<dynamic> GetDocument(string configuration,
			string metadata,
			dynamic filter,
			int pageNumber,
			int pageSize,
            IEnumerable<dynamic> ignoreResolve = null,
			dynamic sorting = null)
		{
			var response = RestQueryApi.QueryPostJsonRaw("RestfulApi", "configuration", "getdocument", null, new
			{
				Configuration = configuration,
				Metadata = metadata,
				Filter = filter,
				PageNumber = pageNumber,
				PageSize = pageSize,
                IgnoreResolve = ignoreResolve,
				Sorting = sorting,
				Secured = _secured
			});

			try
			{
				return response.ToDynamicList();
			}
			catch (Exception)
			{
				throw new ArgumentException(response.Content);
			}
		}


        public IEnumerable<dynamic> GetDocument(
            string configuration, 
            string metadata, 
            Action<FilterBuilder> filter, 
            int pageNumber, 
            int pageSize,
            Action<SortingBuilder> sorting = null)
        {
            return GetDocument(configuration, metadata, filter, pageNumber, pageSize,null, sorting);
        }

        public IEnumerable<dynamic> GetDocument(
            string configuration,
            string metadata,
            Action<FilterBuilder> filter,
            int pageNumber,
            int pageSize,
            IEnumerable<dynamic> ignoreResolve,
            Action<SortingBuilder> sorting = null)
        {
            var filterBuilder = new FilterBuilder();

            if (filter != null)
            {
                filter.Invoke(filterBuilder);
            }

            var sortingBuilder = new SortingBuilder();

            if (sorting != null)
            {
                sorting.Invoke(sortingBuilder);
            }

            return GetDocument(configuration, metadata, filterBuilder.GetFilter(), pageNumber, pageSize, ignoreResolve,
                               sortingBuilder.GetSorting());              
        } 

        /// <summary>
        /// Получение документов из разных конфигураций
        /// </summary>
        public IEnumerable<dynamic> GetDocumentCrossConfig(
            Action<FilterBuilder> filter,
            int pageNumber,
            int pageSize,            
            IEnumerable<string> configurations,
            IEnumerable<string> documents,
			Action<SortingBuilder> sorting = null)
        {
            var filterBuilder = new FilterBuilder();

            if (filter != null)
            {
                filter.Invoke(filterBuilder);
            }

            var sortingBuilder = new SortingBuilder();

            if (sorting != null)
            {
                sorting.Invoke(sortingBuilder);
            }
            
            var response = RestQueryApi.QueryPostJsonRaw("RestfulApi", "configuration", "getdocumentcrossconfig", null, new
            {
                Configurations = configurations,
                Documents = documents,
                Filter = filter == null ? null : filterBuilder.GetFilter(),
                PageNumber = pageNumber,
                PageSize = pageSize,
                Sorting = sorting == null ? null : sortingBuilder.GetSorting(),
				Secured = _secured
            });

            try
            {
                return response.ToDynamicList();
            }
            catch (Exception)
            {
                throw new ArgumentException(response.Content);
            }
        }

		public dynamic CreateDocument(string configuration, string metadata)
		{
			return RestQueryApi.QueryPostJsonRaw("RestfulApi", "configuration", "createdocument", null, new
				                                                                                     {
					                                                                                     Configuration = configuration,
					                                                                                     Metadata = metadata					                                                                                     
				                                                                                     }).ToDynamic();
		}

        public void DeleteDocument(string configuration, string metadata, string documentId)
        {
            RestQueryApi.QueryPostJsonRaw("RestfulApi", "configuration", "deletedocument", null, new
            {
                Configuration = configuration,
                Metadata = metadata,
                Id = documentId,
				Secured = _secured
            });

        }

        public dynamic SetDocument(string configuration, string metadata, dynamic item, bool ignoreWarnings = false, bool allowNonSchemaProperties = false)
        {

			object transactionMarker = ObjectHelper.GetProperty(item, "TransactionMarker");
			if (transactionMarker != null && !string.IsNullOrEmpty(transactionMarker.ToString()))
			{
				item.TransactionMarker = null;
			}
            

            return RestQueryApi.QueryPostJsonRaw("RestfulApi", "configuration", "setdocument", null, new
            {
                Configuration = configuration,
                Metadata = metadata,
				IgnoreWarnings = ignoreWarnings,
                AllowNonSchemaProperties = allowNonSchemaProperties,
                Document = item,
                TransactionMarker = transactionMarker,
				Secured = _secured
            }).ToDynamic();
        }


        public void SetDocuments(string configuration, string metadata, IEnumerable<object> item, int batchSize = 200, bool allowNonSchemaProperties = false)
        {
            var batches = LinqExtensions.Batch(item, batchSize);

            foreach (var batch in batches)
            {
	            object transactionMarker = null;
	            foreach (dynamic document in batch.ToArray())
	            {
					transactionMarker = ObjectHelper.GetProperty(document, "TransactionMarker");
					if (transactionMarker != null && !string.IsNullOrEmpty(transactionMarker.ToString()))
					{
						document.TransactionMarker = null;
					}
	            }

                var response = RestQueryApi.QueryPostJsonRaw("RestfulApi", "configuration", "setdocument", null, new
                {
                    Configuration = configuration,
                    Metadata = metadata,
                    Documents = batch,
                    AllowNonSchemaProperties = allowNonSchemaProperties,
					TransactionMarker = transactionMarker,
					Secured = _secured
                });

                if (!string.IsNullOrEmpty(response.Content))
                {
                    dynamic dynamicContent = response.Content.ToDynamic();

                    if (dynamicContent != null &&
                        dynamicContent.IsValid != null &&
                        dynamicContent.IsValid == false)
                    {
                        throw new ArgumentException(response.Content);
                    }
                }
            }
        }

    }
}

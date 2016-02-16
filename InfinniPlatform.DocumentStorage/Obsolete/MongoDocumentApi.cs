using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

using InfinniPlatform.Core.Documents;
using InfinniPlatform.Sdk.BlobStorage;
using InfinniPlatform.Sdk.Documents;
using InfinniPlatform.Sdk.Dynamic;

namespace InfinniPlatform.DocumentStorage.Obsolete
{
    internal sealed class MongoDocumentApi : IDocumentApi
    {
        public MongoDocumentApi(Func<string, IDocumentStorage> storageFactory, ISetDocumentExecutor setDocumentExecutor, IBlobStorage blobStorage)
        {
            _storageFactory = storageFactory;
            _setDocumentExecutor = setDocumentExecutor;
            _blobStorage = blobStorage;
        }


        private readonly Func<string, IDocumentStorage> _storageFactory;
        private readonly ISetDocumentExecutor _setDocumentExecutor;
        private readonly IBlobStorage _blobStorage;


        public long GetNumberOfDocuments(string documentType, Action<FilterBuilder> filter)
        {
            var documentStorage = _storageFactory(documentType);
            return documentStorage.Count(CreateDocumentStorageFilter(filter.ToFilterCriterias()));
        }

        public long GetNumberOfDocuments(string documentType, IEnumerable<FilterCriteria> filter)
        {
            var documentStorage = _storageFactory(documentType);
            return documentStorage.Count(CreateDocumentStorageFilter(filter));
        }


        public dynamic GetDocumentById(string documentType, string documentId)
        {
            var documentStorage = _storageFactory(documentType);
            var document = documentStorage.Find(f => f.Eq("_id", documentId)).FirstOrDefault();

            if (document != null)
            {
                document["Id"] = document["_id"];
            }

            return document;
        }


        public IEnumerable<dynamic> GetDocument(string documentType, Action<FilterBuilder> filter, int pageNumber, int pageSize, Action<SortingBuilder> sorting = null)
        {
            var documentStorage = _storageFactory(documentType);
            var documentFindCursor = documentStorage.Find(CreateDocumentStorageFilter(filter.ToFilterCriterias())).Skip(pageNumber * pageSize).Limit(pageSize);
            var documentFindSortedCursor = SortDocumentFindCursor(documentFindCursor, sorting.ToSortingCriterias());
            var documents = documentFindSortedCursor.ToList();
            SetDocumentIds(documents);
            return documents;
        }

        public IEnumerable<dynamic> GetDocuments(string documentType, IEnumerable<FilterCriteria> filter, int pageNumber, int pageSize, IEnumerable<SortingCriteria> sorting = null)
        {
            var documentStorage = _storageFactory(documentType);
            var documentFindCursor = documentStorage.Find(CreateDocumentStorageFilter(filter)).Skip(pageNumber * pageSize).Limit(pageSize);
            var documentFindSortedCursor = SortDocumentFindCursor(documentFindCursor, sorting);
            var documents = documentFindSortedCursor.ToList();
            SetDocumentIds(documents);
            return documents;
        }


        public dynamic SetDocument(string documentType, object document)
        {
            return _setDocumentExecutor.SaveDocument(documentType, document);
        }

        public dynamic SetDocuments(string documentType, IEnumerable<object> documents)
        {
            return _setDocumentExecutor.SaveDocuments(documentType, documents);
        }


        public dynamic DeleteDocument(string documentType, string instanceId)
        {
            return _setDocumentExecutor.DeleteDocument(documentType, instanceId);
        }


        public void AttachFile(string documentType, string documentId, string fileProperty, Stream fileStream)
        {
            // Получение документа

            object document = GetDocumentById(documentType, documentId);

            // Получение свойства

            dynamic filePropertyValue = document.GetProperty(fileProperty);

            if (filePropertyValue == null)
            {
                filePropertyValue = new DynamicWrapper();
                filePropertyValue.Info = new DynamicWrapper();
                ObjectHelper.SetProperty(document, fileProperty, filePropertyValue);
            }

            if (filePropertyValue.Info == null)
            {
                filePropertyValue.Info = new DynamicWrapper();
            }

            string fileId = filePropertyValue.Info.ContentId;

            // Сохранение файла

            if (string.IsNullOrEmpty(fileId))
            {
                fileId = _blobStorage.CreateBlob(fileProperty, string.Empty, fileStream);

                filePropertyValue.Info.ContentId = fileId;
            }
            else
            {
                _blobStorage.UpdateBlob(fileId, fileProperty, string.Empty, fileStream);
            }

            // Сохранение документа

            SetDocument(documentType, document);
        }


        private static void SetDocumentIds(IEnumerable<DynamicWrapper> documents)
        {
            foreach (var document in documents)
            {
                document["Id"] = document["_id"];
            }
        }

        private static Func<IDocumentFilterBuilder, object> CreateDocumentStorageFilter(IEnumerable<FilterCriteria> criterias)
        {
            if (criterias != null)
            {
                return f =>
                       {
                           var conditions = new List<object>();

                           foreach (var criteria in criterias)
                           {
                               switch (criteria.CriteriaType)
                               {
                                   case CriteriaType.IsEquals:
                                       conditions.Add(f.Eq(criteria.Property, criteria.Value));
                                       break;
                                   case CriteriaType.IsNotEquals:
                                       conditions.Add(f.NotEq(criteria.Property, criteria.Value));
                                       break;
                                   case CriteriaType.IsMoreThan:
                                       conditions.Add(f.Gt(criteria.Property, criteria.Value));
                                       break;
                                   case CriteriaType.IsMoreThanOrEquals:
                                       conditions.Add(f.Gte(criteria.Property, criteria.Value));
                                       break;
                                   case CriteriaType.IsLessThan:
                                       conditions.Add(f.Lt(criteria.Property, criteria.Value));
                                       break;
                                   case CriteriaType.IsLessThanOrEquals:
                                       conditions.Add(f.Lte(criteria.Property, criteria.Value));
                                       break;
                                   case CriteriaType.IsEmpty:
                                       conditions.Add(f.Or(f.Exists(criteria.Property, false), f.Eq(criteria.Property, ""), f.Eq<object>(criteria.Property, null)));
                                       break;
                                   case CriteriaType.IsNotEmpty:
                                       conditions.Add(f.And(f.Exists(criteria.Property), f.NotEq(criteria.Property, ""), f.NotEq<object>(criteria.Property, null)));
                                       break;
                                   case CriteriaType.IsContains:
                                       conditions.Add(f.Regex(criteria.Property, new Regex($"{criteria.Value}", RegexOptions.IgnoreCase)));
                                       break;
                                   case CriteriaType.IsNotContains:
                                       conditions.Add(f.Not(f.Regex(criteria.Property, new Regex($"{criteria.Value}", RegexOptions.IgnoreCase))));
                                       break;
                                   case CriteriaType.IsStartsWith:
                                       conditions.Add(f.Regex(criteria.Property, new Regex($"^{criteria.Value}", RegexOptions.IgnoreCase)));
                                       break;
                                   case CriteriaType.IsNotStartsWith:
                                       conditions.Add(f.Not(f.Regex(criteria.Property, new Regex($"^{criteria.Value}", RegexOptions.IgnoreCase))));
                                       break;
                                   case CriteriaType.IsEndsWith:
                                       conditions.Add(f.Regex(criteria.Property, new Regex($"{criteria.Value}$", RegexOptions.IgnoreCase)));
                                       break;
                                   case CriteriaType.IsNotEndsWith:
                                       conditions.Add(f.Not(f.Regex(criteria.Property, new Regex($"{criteria.Value}$", RegexOptions.IgnoreCase))));
                                       break;
                                   case CriteriaType.IsIn:
                                       conditions.Add(f.In(criteria.Property, ((IEnumerable)criteria.Value).Cast<object>()));
                                       break;
                                   case CriteriaType.IsIdIn:
                                       conditions.Add(f.In("_id", ((IEnumerable)criteria.Value).Cast<object>()));
                                       break;
                                   case CriteriaType.FullTextSearch:
                                       conditions.Add(f.Text((string)criteria.Value));
                                       break;
                               }
                           }

                           return (conditions.Count > 0) ? f.And(conditions) : null;
                       };
            }

            return null;
        }

        private static IDocumentFindCursor SortDocumentFindCursor(IDocumentFindCursor documentFindCursor, IEnumerable<SortingCriteria> sort)
        {
            var result = documentFindCursor;

            if (sort != null)
            {
                IDocumentFindSortedCursor sortedCursor = null;

                foreach (var criteria in sort)
                {
                    if (string.Equals(criteria.SortingOrder, "ascending", StringComparison.OrdinalIgnoreCase))
                    {
                        if (sortedCursor == null)
                        {
                            sortedCursor = result.SortBy(criteria.PropertyName);
                            result = sortedCursor;
                        }
                        else
                        {
                            sortedCursor = sortedCursor.ThenBy(criteria.PropertyName);
                            result = sortedCursor;
                        }
                    }
                    else if (string.Equals(criteria.SortingOrder, "descending", StringComparison.OrdinalIgnoreCase))
                    {
                        if (sortedCursor == null)
                        {
                            sortedCursor = result.SortByDescending(criteria.PropertyName);
                            result = sortedCursor;
                        }
                        else
                        {
                            sortedCursor = sortedCursor.ThenByDescending(criteria.PropertyName);
                            result = sortedCursor;
                        }
                    }
                }
            }

            return result;
        }
    }
}
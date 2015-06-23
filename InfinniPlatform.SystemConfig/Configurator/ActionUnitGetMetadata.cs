using System;
using System.Collections.Generic;
using InfinniPlatform.Api.ContextComponents;
using InfinniPlatform.Api.Index.SearchOptions;
using InfinniPlatform.Api.RestApi.Auth;
using InfinniPlatform.Api.SearchOptions;
using InfinniPlatform.Index.ElasticSearch.Implementation.Filters;
using InfinniPlatform.Index.QueryLanguage.Implementation;
using InfinniPlatform.Sdk.Contracts;
using InfinniPlatform.Sdk.Dynamic;
using Newtonsoft.Json.Linq;

namespace InfinniPlatform.SystemConfig.Configurator
{
    public sealed class ActionUnitGetMetadata
    {
        private static readonly IFilterBuilder FilterFactory;
        private JsonQueryExecutor _jsonQueryExecutor;

        static ActionUnitGetMetadata()
        {
            FilterFactory = FilterBuilderFactory.GetInstance();
        }

        public void Action(IApplyResultContext target)
        {
            _jsonQueryExecutor =
                new JsonQueryExecutor(target.Context.GetComponent<IIndexComponent>(target.Version).IndexFactory,
                                      FilterFactory,
                                      target.Context.GetComponent<ISecurityComponent>(target.Version)
                                            .GetClaim(AuthorizationStorageExtensions.OrganizationClaim, target.UserName));

            var fullprofiler =
                target.Context.GetComponent<IProfilerComponent>(target.Version)
                      .GetOperationProfiler("get metadata", string.Empty);
            fullprofiler.Reset();

            dynamic jquery = new DynamicWrapper();
            if (target.Item.From == null)
            {
                jquery.From = new DynamicWrapper();
                jquery.From.Index = "systemconfig";
                jquery.From.Type = "metadata";
            }
            else
            {
                if (target.Item.From.Index == null)
                {
                    throw new ArgumentException("From.Index");
                }

                if (target.Item.From.Type == null)
                {
                    throw new ArgumentException("From.Type");
                }


                jquery.From = target.Item.From;
                jquery.From.Index = jquery.From.Index.ToLowerInvariant();
                jquery.From.Type = jquery.From.Type.ToLowerInvariant();
            }

            if (target.Item.Where == null)
            {
                target.Item.Where = new List<dynamic>();
            }


            if (target.Item.ConfigId != null && target.Item.DocumentId == null)
            {
                dynamic filterConfig = new DynamicWrapper();
                dynamic id =
                    target.Context.GetComponent<ISystemComponent>(target.Version)
                          .ManagerIdentifiers.GetConfigurationUid(target.Version, target.Item.ConfigId);
                filterConfig.Property = "Id";
                filterConfig.Value = id;
                filterConfig.CriteriaType = CriteriaType.IsEquals;

                target.Item.Where.Add(filterConfig);
            }

            if (target.Item.DoNotCheckVersion == null || target.Item.DoNotCheckVersion == false)
            {
                dynamic filterConfigVersion = new DynamicWrapper();
                filterConfigVersion.Property = "Version";
                filterConfigVersion.Value = target.Version;
                filterConfigVersion.CriteriaType = CriteriaType.IsEquals;

                target.Item.Where.Add(filterConfigVersion);
            }


            if (target.Item.ConfigId != null && target.Item.DocumentId != null)
            {
                var configIdProvider =
                    target.Context.GetComponent<IProfilerComponent>(target.Version)
                          .GetOperationProfiler("get config id",
                                                string.Format("ConfigId: {0}, DocumentId {1}", target.Item.ConfigId,
                                                              target.Item.DocumentId));
                configIdProvider.Reset();

                dynamic filterConfig = new DynamicWrapper();
                dynamic id =
                    target.Context.GetComponent<ISystemComponent>(target.Version)
                          .ManagerIdentifiers.GetConfigurationUid(target.Version, target.Item.ConfigId);
                filterConfig.Property = "ParentId";
                filterConfig.Value = id;
                filterConfig.CriteriaType = CriteriaType.IsEquals;
                target.Item.Where.Add(filterConfig);
                configIdProvider.TakeSnapshot();


                var documentIdProvider =
                    target.Context.GetComponent<IProfilerComponent>(target.Version)
                          .GetOperationProfiler("get document id",
                                                string.Format("ConfigId: {0}, DocumentId {1}", target.Item.ConfigId,
                                                              target.Item.DocumentId));
                documentIdProvider.Reset();

                dynamic filterDocument = new DynamicWrapper();
                id =
                    target.Context.GetComponent<ISystemComponent>(target.Version)
                          .ManagerIdentifiers.GetDocumentUid(target.Version, target.Item.ConfigId,
                                                             target.Item.DocumentId);
                filterDocument.Property = "Id";
                filterDocument.Value = id;
                filterDocument.CriteriaType = CriteriaType.IsContains;
                target.Item.Where.Add(filterDocument);

                documentIdProvider.TakeSnapshot();
            }


            //нужна абстракция более высокого уровня!
            dynamic filterDeleted = new DynamicWrapper();
            filterDeleted.Property = "Status";
            filterDeleted.Value = "Deleted";
            filterDeleted.CriteriaType = CriteriaType.IsNotEquals;
            target.Item.Where.Add(filterDeleted);

            jquery.Limit = new DynamicWrapper();
            jquery.Limit.StartPage = 0;
            jquery.Limit.PageSize = 10000;
            jquery.Limit.Skip = 0;

            jquery.Where = target.Item.Where;

            if (target.Item.Join != null)
            {
                jquery.Join = new DynamicWrapper();
                jquery.Join = target.Item.Join;

                foreach (dynamic join in jquery.Join)
                {
                    if (join.Alias == null)
                    {
                        throw new ArgumentException("Join.Alias");
                    }

                    if (join.Index == null)
                    {
                        throw new ArgumentException("Join.Index");
                    }

                    if (join.Type == null)
                    {
                        throw new ArgumentException("Join.Type");
                    }

                    join.Index = join.Index.ToLowerInvariant();
                    join.Type = join.Type.ToLowerInvariant();
                }
            }


            if (target.Item.Select != null)
            {
                jquery.Select = new DynamicWrapper();
                jquery.Select = target.Item.Select;
            }

            target.Result = new DynamicWrapper();
            target.Result.Query = jquery;

            var profiler =
                target.Context.GetComponent<IProfilerComponent>(target.Version)
                      .GetOperationProfiler("Run json query", jquery.ToString());
            profiler.Reset();

            target.Result.QueryResult = _jsonQueryExecutor.ExecuteQuery(JObject.FromObject(jquery));
            profiler.TakeSnapshot();

            fullprofiler.TakeSnapshot();
        }
    }
}
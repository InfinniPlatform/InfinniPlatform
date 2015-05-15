using System;
using System.Collections.Generic;
using System.Linq;
using InfinniPlatform.Api.Dynamic;
using InfinniPlatform.Api.Index.SearchOptions;
using InfinniPlatform.Api.SearchOptions;
using Newtonsoft.Json.Linq;

namespace InfinniPlatform.Index.QueryLanguage.Implementation
{
    public class QuerySyntaxTree
    {
        private readonly dynamic _fromObject;

        private readonly dynamic _joinObject;

        private readonly dynamic _whereObject;

        private readonly dynamic _selectObject;

        private readonly dynamic _limitsObject;


        public QuerySyntaxTree(JObject queryTree)
        {
            dynamic query = queryTree.ToDynamic();
            _fromObject = query.From;
            _joinObject = query.Join;
            _selectObject = query.Select;
            _whereObject = query.Where;
            _limitsObject = query.Limit;
        }

        public IEnumerable<ProjectionObject> GetProjectionObjects()
        {
            if (_selectObject != null)
            {
                IEnumerable<dynamic> select = DynamicWrapperExtensions.ToEnumerable(_selectObject);
                return select.Select(s => new ProjectionObject()
                {
                    ProjectionPath = GetProjectionPath(s)
                });
            }
            return new List<ProjectionObject>();
        }

        private string GetProjectionPath(string projection)
        {
            var referencedObjects = GetReferenceObjects();
            foreach (var referencedObject in referencedObjects)
            {
                //sample: DocumentsFull.$.Scenarios DocumentsFull = алиас 
                var names = projection.Split(new[] {'.'}, StringSplitOptions.RemoveEmptyEntries);
                if (referencedObject.Alias == names.FirstOrDefault())
                {
                    //удаляем элемент, соответствующий наименованию алиаса (он будет заменен на путь проекции)
                    var additionalPath = names.ToList();
                    additionalPath.RemoveAt(0);
                    additionalPath.Insert(0, referencedObject.GetProjectionPath());
                    //и добавляем его к пути проекции
                    return string.Join(".", additionalPath);
                }
            }
            return projection;
        }

        public IEnumerable<WhereObject> GetWhereObjects()
        {
            if (_whereObject != null)
            {
                IEnumerable<dynamic> where = DynamicWrapperExtensions.ToEnumerable(_whereObject);
                return where.Select(w => new WhereObject()
                {
                    Property = GetProjectionPath(w.Property),
                    Value = w.Value != null ? ((JValue) w.Value).Value : null,
                    CriteriaType = (CriteriaType) w.CriteriaType,
                    RawProperty = w.Property
                }).ToList();

            }
            return new List<WhereObject>();
        }



        public IEnumerable<ReferencedObject> GetReferenceObjects()
        {
            if (_joinObject != null)
            {
                IEnumerable<dynamic> joinObjects = DynamicWrapperExtensions.ToEnumerable(_joinObject);
                return joinObjects.Select(j => new ReferencedObject()
                {
                    Alias = j.Alias,
                    Index = j.Index,
                    Path = j.Path,
                    Type = j.Type
                });

            }
            return new List<ReferencedObject>();
        }

        public IEnumerable<Criteria> GetConditionCriteria()
        {
            if (_whereObject != null)
            {
                IEnumerable<dynamic> whereObjects = DynamicWrapperExtensions.ToEnumerable(_whereObject);
                return whereObjects.Select(w => new Criteria()
                {
                    CriteriaType = (CriteriaType) Convert.ToInt32(w.CriteriaType),
                    Property = w.Property,
                    Value = w.Value
                }).ToList();
            }
            return new List<Criteria>();
        }

        public ReferencedObject GetFrom()
        {
            if (_fromObject != null)
            {
                return new ReferencedObject()
                {
                    Alias = _fromObject.Alias,
                    Index = _fromObject.Index,
                    Type = _fromObject.Type
                };
            }
            return null;
        }

        public ResultLimits GetLimits()
        {
            ResultLimits result = null;
            if (_limitsObject != null)
            {
                var fromPage = (int)_limitsObject.StartPage;

                var pageSize = (int)_limitsObject.PageSize;

                var skip = (int)_limitsObject.Skip;

                result = new ResultLimits(fromPage, pageSize, skip);
            }
            return result;
        }

        /// <summary>
        ///   Получить список полей для составления критериев по эластику, не являющихся алиасами JOIN
        /// </summary>
        /// <param name="aliases">Указанные в запросе алиасы JOIN</param>
        /// <param name="checkedProperties">Проверяемые поля</param>
        /// <returns>Спиоск полей, не являющихся полями JOIN - объектов</returns>
        public static IEnumerable<string> GetNotAliasedFields(IEnumerable<string> aliases,
            IEnumerable<string> checkedProperties)
        {
            var aliasedFields = new List<string>();
            foreach (var alias in aliases)
            {
                foreach (var checkedProperty in checkedProperties)
                {
                    var path = checkedProperty.Split(new char[] {'.'}, StringSplitOptions.RemoveEmptyEntries);
                    if (path[0].ToLowerInvariant() == alias.ToLowerInvariant())
                    {
                        aliasedFields.Add(checkedProperty);
                    }
                }
            }
            aliasedFields = aliasedFields.Distinct().ToList();
            return checkedProperties.Except(aliasedFields).ToList();
        }
    }

}
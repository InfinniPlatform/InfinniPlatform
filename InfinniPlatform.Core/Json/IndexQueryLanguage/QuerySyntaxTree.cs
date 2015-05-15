using System;
using System.Collections.Generic;
using System.Linq;
using InfinniPlatform.SearchOptions;
using Newtonsoft.Json.Linq;

namespace InfinniPlatform.Json.IndexQueryLanguage
{
	public class QuerySyntaxTree
	{
		private const string From = "from";

		private const string Join = "join";

		private const string Where = "where";

		private const string Select = "select";

		private readonly JToken _fromObject;

		private readonly JToken _joinObject;

		private readonly JToken _whereObject;

		private readonly JToken _selectObject;

		private readonly JsonParser _jsonParser = new JsonParser();


		public QuerySyntaxTree(JObject queryTree)
		{
			_fromObject = _jsonParser.FindJsonPath(queryTree, From).FirstOrDefault();
			_joinObject = _jsonParser.FindJsonPath(queryTree, Join).FirstOrDefault();
			_selectObject = _jsonParser.FindJsonPath(queryTree, Select).FirstOrDefault();
			_whereObject = _jsonParser.FindJsonPath(queryTree, Where).FirstOrDefault();
		}

		public IEnumerable<ProjectionObject> GetProjectionObjects()
		{
			if (_selectObject != null)
			{
				return _selectObject.Values()
				                    .Select(f => new ProjectionObject()
					                                 {
						                                 ProjectionName = ((JProperty) f).Name.ToString(),
						                                 ProjectionPath =
							                                 ((JProperty) f).Value != null ? ((JProperty) f).Value.ToString() : string.Empty
					                                 }).ToList();
			}
			return new List<ProjectionObject>();
		}


		public IEnumerable<ReferencedObject> GetReferenceObjects()
		{
			if (_joinObject != null)
			{
				return _joinObject.Values().Select(j => new ReferencedObject()
					                                        {
						                                        Alias = _jsonParser.FindJsonPath(j, "Alias").GetPropertyValueToString(),
						                                        Index = _jsonParser.FindJsonPath(j, "Index").GetPropertyValueToString(),
						                                        Path = _jsonParser.FindJsonPath(j, "Path").GetPropertyValueToString(),
					                                        });

			}
			return new List<ReferencedObject>();
		}

		public IEnumerable<Criteria> GetConditionCriteria()
		{
			if (_whereObject != null)
			{
				return _whereObject.Values().Select(j => new Criteria()
					                                         {
																 CriteriaType = (CriteriaType) Convert.ToInt32(_jsonParser.FindJsonPath(j,"CriteriaType").GetPropertyValueObject()),
																 Property = _jsonParser.FindJsonPath(j, "Property").GetPropertyValueToString(),
																 Value = _jsonParser.FindJsonPath(j,"Value").GetPropertyValueObject()
					                                         });
			}
			return new List<Criteria>();
		}
 
		public ReferencedObject GetFrom()
		{
			if (_fromObject != null)
			{
				return new ReferencedObject()
					       {
						       Alias = _jsonParser.FindJsonPath(_fromObject, "Alias").GetPropertyValueToString(),
						       Index = _jsonParser.FindJsonPath(_fromObject, "Index").GetPropertyValueToString()
					       };
			}
			return null;
		}

	}

}
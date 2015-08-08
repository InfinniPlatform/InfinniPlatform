using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using InfinniPlatform.Json;
using InfinniPlatform.Sdk.Environment.Index;
using Newtonsoft.Json.Linq;

namespace InfinniPlatform.Index.QueryLanguage.Implementation
{
	public sealed class ReferenceBuilder
	{
		private readonly IIndexFactory _indexFactory;
		private readonly string _tenantId;

		public ReferenceBuilder(IIndexFactory indexFactory, string tenantId)
		{
			_indexFactory = indexFactory;
			_tenantId = tenantId;
		}


		private static bool IsIdentifierValue(IEnumerable<JToken> tokens)
		{
			return tokens != null && tokens.Any() && tokens.First() is JProperty;


		}

		private static JProperty GetIdentifier(JToken token)
		{
			return ((JProperty)token);
		}

		private static string GetIdentifierValue(JToken token)
		{
			return GetIdentifier(token).Value.ToString();
		}


		//var aliases = new Dictionary<string, string>();
		//aliases.Add("ReferenceId","Reference");

		//var indexDescriptions = new Dictionary<string, string>();
		//indexDescriptions.Add("ReferenceId", "ReferenceIndex");

		public void FillReference(JObject jsonObject, ReferencedObject[] referencedObjects, IEnumerable<WhereObject> whereObjects, IFilterBuilder filterBuilder)
		{
			var jsonParser = new JsonParser();

			var executors = new Dictionary<string, IIndexQueryExecutor>();

			foreach (var reference in referencedObjects)
			{
				executors.Add(reference.Path, _indexFactory.BuildIndexQueryExecutor(reference.Index.ToLowerInvariant(), reference.Type, _tenantId));
			}


			foreach (var reference in referencedObjects)
			{
				var pathInJson = reference.Path;
				var identifierValue = jsonParser.FindJsonToken(jsonObject, pathInJson).ToArray();
				if (!identifierValue.Any() || !IsIdentifierValue(identifierValue))
				{
					continue;
				}
				IEnumerable<dynamic> items = executors[reference.Path].Query(FillReferencedObjectsFilter(reference, whereObjects, filterBuilder)).Items;

				items.ToList().ForEach(i => i.TimeStamp = null);

				foreach (var identifier in identifierValue)
				{
					var immediateParent = identifier.Parent;
					
					var item = items.FirstOrDefault(i => i.Id != null && i.Id.ToString().ToLowerInvariant() == GetIdentifierValue(identifier).ToLowerInvariant());

					//token.Remove();
					if (item != null)
					{
						if (immediateParent is JObject)
						{
							((JObject)immediateParent).Add(reference.Alias, JObject.FromObject(item));
						}
						else
						{
							throw new ArgumentException("unknown immediate parent type");
						}
					}
				}

			}

		}

		private SearchModel FillReferencedObjectsFilter(ReferencedObject reference, IEnumerable<WhereObject> whereObjects, IFilterBuilder filterBuilder)
	    {
		    var searchModel = new SearchModel();
			searchModel.SetPageSize(10000000);
		    foreach (var whereObject in whereObjects)
		    {
				//полный путь к свойству (ex: "DocumentFull.SomeObject.Name")
			    string[] paths = whereObject.RawProperty.Split(new[] {"."}, StringSplitOptions.RemoveEmptyEntries);

			    var aliasName = paths.FirstOrDefault();

			    var fieldName = string.Join(".",
				    paths.Skip(1).ToList());

				if (reference.Alias == aliasName)
				{
					searchModel.AddFilter(filterBuilder.Get(fieldName, whereObject.Value, CriteriaType.IsEquals));
					
				}
			    
		    }
			return searchModel;
	    }
	}
}
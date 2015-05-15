using System;
using System.Collections.Generic;
using System.Linq;
using InfinniPlatform.Api.Dynamic;
using InfinniPlatform.Factories;
using InfinniPlatform.Index;
using Newtonsoft.Json.Linq;

namespace InfinniPlatform.Json.IndexQueryLanguage
{
    public sealed class ReferenceBuilder
    {
        private readonly IIndexFactory _indexFactory;

        public ReferenceBuilder(IIndexFactory indexFactory)
        {
            _indexFactory = indexFactory;
        }



        private static bool IsIdentifierValue(IEnumerable<JToken> tokens)
        {
            return tokens != null && tokens.Any() && tokens.First() is JProperty;


        }

        private static JProperty GetIdentifier(IEnumerable<JToken> tokens)
        {
            return ((JProperty) tokens.First());
        }

        private static string GetIdentifierValue(IEnumerable<JToken> tokens)
        {
            return GetIdentifier(tokens).Value.ToString();
        }


            //var aliases = new Dictionary<string, string>();
            //aliases.Add("ReferenceId","Reference");

            //var indexDescriptions = new Dictionary<string, string>();
            //indexDescriptions.Add("ReferenceId", "ReferenceIndex");

        public void FillReference(JObject jsonObject, ReferencedObject[] referencedObject)
        {
            var jsonParser = new JsonParser();

            var providers = new Dictionary<string, ICrudOperationProvider>();

	        foreach (var reference in referencedObject)
            {
                providers.Add(reference.Path, _indexFactory.BuildElasticSearchProvider(reference.Index.ToLowerInvariant()));
            }

			foreach (var reference in referencedObject)
            {
                var pathInJson = reference.Path;
                var identifierValue = jsonParser.FindJsonPath(jsonObject, pathInJson).ToArray();
                if (!identifierValue.Any() || !IsIdentifierValue(identifierValue))
                {
                    throw new ArgumentException(string.Format("can't found alias specified: {0}",pathInJson));                    
                }
                var idToken = GetIdentifier(identifierValue);
                var immediateParent = idToken.Parent;
                var item = providers[reference.Path].GetItem(GetIdentifierValue(identifierValue));
                idToken.Remove();
                if (item != null)
                {
                    if (immediateParent is JObject)
                    {
                        ((JObject) immediateParent).Add(reference.Alias, DynamicInstanceExtensions.ToJObject(item));
                    }
                    else
                    {
                        throw new ArgumentException("unknown immediate parent type");
                    }
                }
            }

        }
    }
}
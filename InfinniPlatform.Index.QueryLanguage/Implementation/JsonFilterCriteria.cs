﻿using System;
using System.Linq;

using InfinniPlatform.Api.Validation;
using InfinniPlatform.Json;

using Newtonsoft.Json.Linq;

namespace InfinniPlatform.Index.QueryLanguage.Implementation
{
    internal sealed class JsonFilterCriteria
    {
        private readonly WhereObject _criteria;
        private readonly JToken _jsonToken;
        private readonly JsonParser _jsonParser;

        public JsonFilterCriteria(WhereObject criteria, JToken jsonToken)
        {
            _criteria = criteria;
            _jsonToken = jsonToken;
            _jsonParser = new JsonParser();
        }

        public JToken JsonToken
        {
            get { return _jsonToken; }
        }

	    public string Property
	    {
		    get { return _criteria.Property; }
	    }

        public void RemoveUnsatisfiedTokens(IValidationOperator clientCriteria)
        {
            var foundToken = _jsonParser.FindJsonToken(JsonToken, _criteria.Property).ToList();

            var checkTokens = foundToken.ToList();
            foreach (var token in checkTokens)
            {               
                var propertyValue = token.GetPropertyValueObject();

                if (propertyValue != null &&  !clientCriteria.Validate(propertyValue))  //!_criteria.Value.ToString().Equals(propertyValue))
                {
                    if (token.Parent != null && token.Parent.Parent != null && token.Parent.Parent is JArray)
                    {
	                    token.Parent.Remove();
                    }
                    else if (token.Parent is JObject && token.Parent.Parent != null)
                    {						
	                    token.Parent.Parent.Remove();
                    }
					else if (token.Parent is JObject)
					{
						token.Remove();
					}
					else
					{
						throw new ArgumentException("parent for JToken not found.");
					}
                }                
            }
        }
    }

}
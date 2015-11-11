using System;
using System.Collections;

namespace InfinniPlatform.Sdk.Environment.Log
{
    public static class ExceptionExtensions
    {
        public static T AddContextData<T>(this T ex, IDictionary contextData) 
            where T : Exception 
        {
            foreach (var key in contextData.Keys)
            {
                ex.Data.Add(key, contextData[key]);
            }

            return ex;
        }
    }
}
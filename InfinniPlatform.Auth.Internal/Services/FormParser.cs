using System;
using System.Collections.Generic;

using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Primitives;

namespace InfinniPlatform.Auth.Services
{
    /// <summary>
    /// Parser for HTTP request form, copied from Microsoft.Owin.Infrastructure.OwinHelpers.
    /// </summary>
    public static class FormParser
    {
        public static IFormCollection ParseForm(string text)
        {
            var store = new Dictionary<string, StringValues>(StringComparer.OrdinalIgnoreCase);
            var dictionary = new Dictionary<string, List<string>>(StringComparer.OrdinalIgnoreCase);

            ParseDelimited(text, new[] { '&' }, AppendItemCallback, dictionary);

            foreach (var keyValuePair in dictionary)
            {
                store.Add(keyValuePair.Key, keyValuePair.Value.ToArray());
            }
            return new FormCollection(store);
        }

        private static readonly Action<string, string, object> AppendItemCallback = (name, value, state) =>
                                                                                    {
                                                                                        var dictionary = (IDictionary<string, List<string>>)state;

                                                                                        List<string> stringList;

                                                                                        if (!dictionary.TryGetValue(name, out stringList))
                                                                                        {
                                                                                            dictionary.Add(name, new List<string>(1) { value });
                                                                                        }
                                                                                        else
                                                                                        {
                                                                                            stringList.Add(value);
                                                                                        }
                                                                                    };

        private static void ParseDelimited(string text, char[] delimiters, Action<string, string, object> callback, object state)
        {
            var length = text.Length;
            var num = text.IndexOf('=');
            if (num == -1)
            {
                num = length;
            }
            int startIndex1;
            for (var startIndex2 = 0; startIndex2 < length; startIndex2 = startIndex1 + 1)
            {
                startIndex1 = text.IndexOfAny(delimiters, startIndex2);
                if (startIndex1 == -1)
                {
                    startIndex1 = length;
                }
                if (num < startIndex1)
                {
                    while (startIndex2 != num && char.IsWhiteSpace(text[startIndex2]))
                    {
                        ++startIndex2;
                    }
                    var str1 = text.Substring(startIndex2, num - startIndex2);
                    var str2 = text.Substring(num + 1, startIndex1 - num - 1);
                    callback(Uri.UnescapeDataString(str1.Replace('+', ' ')), Uri.UnescapeDataString(str2.Replace('+', ' ')), state);
                    num = text.IndexOf('=', startIndex1);
                    if (num == -1)
                    {
                        num = length;
                    }
                }
            }
        }
    }
}
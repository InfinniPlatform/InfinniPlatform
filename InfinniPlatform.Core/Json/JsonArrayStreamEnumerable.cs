using System.Collections;
using System.Collections.Generic;
using System.IO;

namespace InfinniPlatform.Json
{
    /// <summary>
    ///     Предоставляет перечисление с поддержкой перебора элементов JSON-массива из потока.
    /// </summary>
    public sealed class JsonArrayStreamEnumerable : IEnumerable<object>
    {
        private readonly JsonArrayStreamEnumerator _enumerator;

        public JsonArrayStreamEnumerable(Stream stream)
        {
            _enumerator = new JsonArrayStreamEnumerator(stream);
        }

        IEnumerator<object> IEnumerable<object>.GetEnumerator()
        {
            return _enumerator;
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return _enumerator;
        }
    }
}
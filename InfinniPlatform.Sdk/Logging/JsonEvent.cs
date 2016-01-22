using System;
using System.Collections.Generic;

using InfinniPlatform.Sdk.Serialization;

namespace InfinniPlatform.Sdk.Logging
{
    // ReSharper disable InconsistentNaming

    [Serializable]
    public class JsonEvent
    {
        public JsonEvent(string message, Dictionary<string, object> context, Exception exception = null)
        {
            msg = message;
            ctx = context;

            ex = (exception != null) ? new ExceptionWrapper(exception) : null;
        }

        [NonSerialized]
        private string _toString;

        public Dictionary<string, object> ctx;
        public ExceptionWrapper ex;
        public string msg;

        public override string ToString()
        {
            return _toString ?? (_toString = JsonObjectSerializer.Default.ConvertToString(this));
        }


        [Serializable]
        public class ExceptionWrapper
        {
            public ExceptionWrapper(Exception ex)
            {
                msg = ex.GetFullMessage();
                stackTrace = ex.GetFullStackTrace();
            }

            public string msg;
            public string stackTrace;
        }
    }

    // ReSharper restore InconsistentNaming
}
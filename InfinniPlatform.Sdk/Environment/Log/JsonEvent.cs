using System;
using System.Collections.Generic;

using Newtonsoft.Json;

namespace InfinniPlatform.Sdk.Environment.Log
{
    // ReSharper disable InconsistentNaming
    [Serializable]
    public class JsonEvent
    {
        [Serializable]
        public class ExceptionWrapper
        {
            public string msg;
            public string stackTrace;

            public ExceptionWrapper(Exception ex)
            {
                msg = ex.Message;
                stackTrace = ex.StackTrace;
            }
        }

        public string msg;
        public Dictionary<string, Object> ctx;
        public ExceptionWrapper ex;

        [NonSerialized]
        private string _toString;

        public JsonEvent(string message, Dictionary<string, Object> context)
        {
            msg = message;
            ctx = context;
        }

        public JsonEvent(string message, Dictionary<string, Object> context, Exception exception)
        {
            msg = message;
            ctx = context;

            ex = exception != null ? new ExceptionWrapper(exception) : null;
        }

        public override string ToString()
        {
            return _toString ?? (_toString = JsonConvert.SerializeObject(this));
        }
    }
    // ReSharper restore InconsistentNaming
}
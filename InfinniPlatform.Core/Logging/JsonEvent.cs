using System;
using System.Collections.Generic;

using InfinniPlatform.Sdk.Logging;
using InfinniPlatform.Sdk.Serialization;

namespace InfinniPlatform.Core.Logging
{
    // ReSharper disable InconsistentNaming
    // ReSharper disable NotAccessedField.Global
    // ReSharper disable MemberCanBePrivate.Global

    [Serializable]
    internal sealed class JsonEvent
    {
        public JsonEvent(object message, Dictionary<string, object> context, Exception exception = null)
        {
            _message = message;

            ctx = context;
            ex = (exception != null) ? new ExceptionWrapper(exception) : null;
        }


        [NonSerialized]
        private readonly object _message;
        [NonSerialized]
        private string _toString;

        public string msg;
        public ExceptionWrapper ex;
        public Dictionary<string, object> ctx;


        public override string ToString()
        {
            if (_toString == null)
            {
                if (_message != null)
                {
                    msg = _message.ToString();
                }

                _toString = JsonObjectSerializer.Default.ConvertToString(this);
            }

            return _toString;
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

    // ReSharper restore MemberCanBePrivate.Global
    // ReSharper restore NotAccessedField.Global
    // ReSharper restore InconsistentNaming
}
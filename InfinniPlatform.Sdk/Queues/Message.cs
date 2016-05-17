using System;

namespace InfinniPlatform.Sdk.Queues
{
    public class Message<T> : IMessage<T> where T : class
    {
        public Message(T body)
        {
            Body = body;
        }

        public T Body { get; }

        public object GetBody()
        {
            return Body;
        }

        public Type GetBodyType()
        {
            return Body.GetType();
        }
    }
}
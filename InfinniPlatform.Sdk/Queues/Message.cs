using InfinniPlatform.Sdk.Dynamic;

namespace InfinniPlatform.Sdk.Queues
{
    public class Message : Message<DynamicWrapper>
    {
        public Message(DynamicWrapper body) : base(body)
        {
        }
    }
}
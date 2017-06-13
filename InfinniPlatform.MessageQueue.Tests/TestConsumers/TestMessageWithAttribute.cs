namespace InfinniPlatform.MessageQueue.TestConsumers
{
    [QueueName("TestMessageWithAttributeTestQueue")]
    public class TestMessageWithAttribute
    {
        public TestMessageWithAttribute(string someString)
        {
            SomeString = someString;
        }

        public string SomeString { get; }

        public override bool Equals(object obj)
        {
            var message = obj as TestMessageWithAttribute;
            return message != null &&
                   string.Equals(SomeString, message.SomeString);
        }

        public override int GetHashCode()
        {
            var hashCode = SomeString?.GetHashCode() ?? 0;
            return hashCode;
        }
    }
}
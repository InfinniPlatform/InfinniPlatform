using System;

using InfinniPlatform.Sdk.Queues;

namespace InfinniPlatform.MessageQueue.Tests.IntegrationTests.TestConsumers
{
    public class TestMessage
    {
        public TestMessage(string someString,
                           int someInt,
                           DateTime someDateTime)
        {
            SomeString = someString;
            SomeInt = someInt;
            SomeDateTime = someDateTime;
        }

        public string SomeString { get; internal set; }

        public int SomeInt { get; internal set; }

        public DateTime SomeDateTime { get; internal set; }

        public override bool Equals(object obj)
        {
            var message = obj as TestMessage;
            return message != null &&
                   Equals(this, message);
        }

        protected bool Equals(TestMessage other)
        {
            return string.Equals(SomeString, other.SomeString) && SomeInt == other.SomeInt && SomeDateTime.Equals(other.SomeDateTime);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                var hashCode = SomeString != null
                                   ? SomeString.GetHashCode()
                                   : 0;
                hashCode = (hashCode * 397) ^ SomeInt;
                hashCode = (hashCode * 397) ^ SomeDateTime.GetHashCode();
                return hashCode;
            }
        }
    }
}
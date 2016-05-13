using System;

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

        public string SomeString { get; }

        public int SomeInt { get; }

        public DateTime SomeDateTime { get; }

        public override string ToString()
        {
            return $"{SomeString} - {SomeInt} - {SomeDateTime.ToString("g")}";
        }

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
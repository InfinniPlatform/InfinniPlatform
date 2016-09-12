using System;

namespace InfinniPlatform.MessageQueue.Tests.IntegrationTests.TestConsumers
{
    public class TestMessage : IEquatable<TestMessage>
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

        public bool Equals(TestMessage other)
        {
            if (ReferenceEquals(null, other))
            {
                return false;
            }
            if (ReferenceEquals(this, other))
            {
                return true;
            }
            return SomeString.Equals(other.SomeString) &&
                   (SomeInt == other.SomeInt) &&
                   SomeDateTime.Equals(other.SomeDateTime);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj))
            {
                return false;
            }
            if (ReferenceEquals(this, obj))
            {
                return true;
            }
            if (!(obj is TestMessage))
            {
                return false;
            }
            return Equals((TestMessage)obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                var hashCode = SomeString?.GetHashCode() ?? 0;
                hashCode = (hashCode * 397) ^ SomeInt;
                hashCode = (hashCode * 397) ^ SomeDateTime.GetHashCode();
                return hashCode;
            }
        }

        public static bool operator ==(TestMessage left, TestMessage right)
        {
            return Equals(left, right);
        }

        public static bool operator !=(TestMessage left, TestMessage right)
        {
            return !Equals(left, right);
        }
    }
}
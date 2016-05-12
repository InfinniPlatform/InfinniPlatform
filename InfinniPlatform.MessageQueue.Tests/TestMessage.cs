using System;

namespace InfinniPlatform.MessageQueue.Tests
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

        public string SomeString { get; set; }

        public int SomeInt { get; set; }

        public DateTime SomeDateTime { get; set; }

        public override string ToString()
        {
            return $"{SomeString} - {SomeInt} - {SomeDateTime.ToString("g")}";
        }
    }
}
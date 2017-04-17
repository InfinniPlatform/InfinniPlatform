using System;

using InfinniPlatform.Core.Abstractions.Serialization;
using InfinniPlatform.Core.Serialization;

using NUnit.Framework;

namespace InfinniPlatform.Core.Tests.Serialization
{
    [TestFixture]
    [Category(TestCategories.UnitTest)]
    public class IgnoreSerializerErrorHandlerTest
    {
        private const string NormalJson = @"{""Property1"":1,""Property2"":2}";

        private const string ShortJson = @"{""Property1"":1}";

        private static readonly Tuple<object, string>[] SerializeTestCases =
        {
            new Tuple<object, string>(new Normal { Property1 = 1, Property2 = 2 }, NormalJson),
            new Tuple<object, string>(new BadGetter { Property1 = 1 }, ShortJson),
            new Tuple<object, string>(new BadSetter { Property1 = 1 }, ShortJson),
            new Tuple<object, string>(new BadGetterAndBadSetter { Property1 = 1 }, ShortJson)
        };

        private static readonly Tuple<string, Type, Func<dynamic, bool>>[] DeserializeTestCases =
        {
            new Tuple<string, Type, Func<dynamic, bool>>(NormalJson, typeof(Normal), i => i.Property1 == 1),
            new Tuple<string, Type, Func<dynamic, bool>>(NormalJson, typeof(BadGetter), i => i.Property1 == 1),
            new Tuple<string, Type, Func<dynamic, bool>>(NormalJson, typeof(BadSetter), i => i.Property1 == 1),
            new Tuple<string, Type, Func<dynamic, bool>>(NormalJson, typeof(BadGetterAndBadSetter), i => i.Property1 == 1)
        };


        [Test]
        [TestCaseSource(nameof(SerializeTestCases))]
        public void ShouldIgnoreExceptionWhenSerialize(Tuple<object, string> testCase)
        {
            // Given
            var errorHandler = new IgnoreSerializerErrorHandler();
            var serializer = new JsonObjectSerializer(false, errorHandlers: new[] { errorHandler });

            // When
            var result = serializer.ConvertToString(testCase.Item1);

            // Then
            Assert.AreEqual(testCase.Item2, result);
        }

        [Test]
        [TestCaseSource(nameof(DeserializeTestCases))]
        public void ShouldIgnoreExceptionWhenDeserialize(Tuple<string, Type, Func<dynamic, bool>> testCase)
        {
            // Given
            var errorHandler = new IgnoreSerializerErrorHandler();
            var serializer = new JsonObjectSerializer(false, errorHandlers: new[] { errorHandler });

            // When
            var result = serializer.Deserialize(testCase.Item1, testCase.Item2);

            // Then
            Assert.IsTrue(testCase.Item3(result));
        }


        // ReSharper disable UnusedAutoPropertyAccessor.Local
        // ReSharper disable ValueParameterNotUsed
        // ReSharper disable UnusedMember.Local

        class Normal
        {
            public object Property1 { get; set; }

            public object Property2 { get; set; }
        }


        class BadGetter
        {
            public object Property1 { get; set; }

            public object Property2
            {
                get { throw new Exception(); }
                set { }
            }
        }


        class BadSetter
        {
            public object Property1 { get; set; }

            public object Property2
            {
                get { return null; }
                set { throw new Exception(); }
            }
        }


        class BadGetterAndBadSetter
        {
            public object Property1 { get; set; }

            public object Property2
            {
                get { throw new Exception(); }
                set { throw new Exception(); }
            }
        }

        // ReSharper restore UnusedMember.Local
        // ReSharper restore ValueParameterNotUsed
        // ReSharper restore UnusedAutoPropertyAccessor.Local
    }
}
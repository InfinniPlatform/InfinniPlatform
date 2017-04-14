using InfinniPlatform.DocumentStorage.HttpService.QuerySyntax;

using NUnit.Framework;

namespace InfinniPlatform.DocumentStorage.Tests.HttpService.QuerySyntax
{
    [TestFixture]
    [Category(TestCategories.UnitTest)]
    public class QueryTokenReaderTest
    {
        [Test]
        [TestCase("eq", QueryTokenKind.Identifier, "eq")]
        [TestCase("lt", QueryTokenKind.Identifier, "lt")]
        [TestCase("(", QueryTokenKind.OpenBracket, '(')]
        [TestCase(")", QueryTokenKind.CloseBracket, ')')]
        [TestCase(",", QueryTokenKind.ArgumentSeparator, ',')]
        [TestCase("null", QueryTokenKind.Null, null)]
        [TestCase("true", QueryTokenKind.Boolean, true)]
        [TestCase("false", QueryTokenKind.Boolean, false)]
        [TestCase("123", QueryTokenKind.Integer, 123)]
        [TestCase("+123", QueryTokenKind.Integer, 123)]
        [TestCase("-123", QueryTokenKind.Integer, -123)]
        [TestCase(".456", QueryTokenKind.Float, .456)]
        [TestCase("+.456", QueryTokenKind.Float, .456)]
        [TestCase("-.456", QueryTokenKind.Float, -.456)]
        [TestCase("123.456", QueryTokenKind.Float, 123.456)]
        [TestCase("+123.456", QueryTokenKind.Float, 123.456)]
        [TestCase("-123.456", QueryTokenKind.Float, -123.456)]
        [TestCase("'Abc'", QueryTokenKind.String, "Abc")]
        [TestCase(@"'\'A\'b\'c\''", QueryTokenKind.String, "'A'b'c'")]
        [TestCase(@"'Ab\nc'", QueryTokenKind.String, "Ab\nc")]
        [TestCase(@"'Ab\tc'", QueryTokenKind.String, "Ab\tc")]
        public void ShouldReadTokens(string query, object expectedKind, object expectedValue)
        {
            // Given
            var reader = new QueryTokenReader(query);

            // When
            var token = reader.ReadNext();

            // Then
            Assert.IsNotNull(token);
            Assert.AreEqual(expectedKind, token.Kind);
            Assert.AreEqual(expectedValue, token.Value);
        }

        [Test]
        public void ShouldReadComplexQuery()
        {
            // Given
            var reader = new QueryTokenReader("eq('Company', 'Abc'), lt('Price', 100)");

            // When & Then

            Assert.IsTrue(reader.CanRead());
            var token = reader.ReadNext();
            Assert.IsNotNull(token);
            Assert.AreEqual(QueryTokenKind.Identifier, token.Kind);
            Assert.AreEqual("eq", token.Value);

            Assert.IsTrue(reader.CanRead());
            token = reader.ReadNext();
            Assert.IsNotNull(token);
            Assert.AreEqual(QueryTokenKind.OpenBracket, token.Kind);
            Assert.AreEqual('(', token.Value);

            Assert.IsTrue(reader.CanRead());
            token = reader.ReadNext();
            Assert.IsNotNull(token);
            Assert.AreEqual(QueryTokenKind.String, token.Kind);
            Assert.AreEqual("Company", token.Value);

            Assert.IsTrue(reader.CanRead());
            token = reader.ReadNext();
            Assert.IsNotNull(token);
            Assert.AreEqual(QueryTokenKind.ArgumentSeparator, token.Kind);
            Assert.AreEqual(',', token.Value);

            Assert.IsTrue(reader.CanRead());
            token = reader.ReadNext();
            Assert.IsNotNull(token);
            Assert.AreEqual(QueryTokenKind.String, token.Kind);
            Assert.AreEqual("Abc", token.Value);

            Assert.IsTrue(reader.CanRead());
            token = reader.ReadNext();
            Assert.IsNotNull(token);
            Assert.AreEqual(QueryTokenKind.CloseBracket, token.Kind);
            Assert.AreEqual(')', token.Value);

            Assert.IsTrue(reader.CanRead());
            token = reader.ReadNext();
            Assert.IsNotNull(token);
            Assert.AreEqual(QueryTokenKind.ArgumentSeparator, token.Kind);
            Assert.AreEqual(',', token.Value);

            Assert.IsTrue(reader.CanRead());
            token = reader.ReadNext();
            Assert.IsNotNull(token);
            Assert.AreEqual(QueryTokenKind.Identifier, token.Kind);
            Assert.AreEqual("lt", token.Value);

            Assert.IsTrue(reader.CanRead());
            token = reader.ReadNext();
            Assert.IsNotNull(token);
            Assert.AreEqual(QueryTokenKind.OpenBracket, token.Kind);
            Assert.AreEqual('(', token.Value);

            Assert.IsTrue(reader.CanRead());
            token = reader.ReadNext();
            Assert.IsNotNull(token);
            Assert.AreEqual(QueryTokenKind.String, token.Kind);
            Assert.AreEqual("Price", token.Value);

            Assert.IsTrue(reader.CanRead());
            token = reader.ReadNext();
            Assert.IsNotNull(token);
            Assert.AreEqual(QueryTokenKind.ArgumentSeparator, token.Kind);
            Assert.AreEqual(',', token.Value);

            Assert.IsTrue(reader.CanRead());
            token = reader.ReadNext();
            Assert.IsNotNull(token);
            Assert.AreEqual(QueryTokenKind.Integer, token.Kind);
            Assert.AreEqual(100, token.Value);

            Assert.IsTrue(reader.CanRead());
            token = reader.ReadNext();
            Assert.IsNotNull(token);
            Assert.AreEqual(QueryTokenKind.CloseBracket, token.Kind);
            Assert.AreEqual(')', token.Value);

            Assert.IsFalse(reader.CanRead());
            token = reader.ReadNext();
            Assert.IsNull(token);
        }
    }
}
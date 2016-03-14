using System;
using System.Diagnostics;
using System.Linq;

using InfinniPlatform.DocumentStorage.Services.QuerySyntax;

using NUnit.Framework;

namespace InfinniPlatform.DocumentStorage.Tests.Services.QuerySyntax
{
    [TestFixture]
    [Category(TestCategories.UnitTest)]
    public class QuerySyntaxTreeParserTest
    {
        private static readonly ExpressionTestCase[] Expressions
            =
        {
            NullConstantExpression(),
            TrueConstantExpression(),
            FalseConstantExpression(),
            IntegerConstantExpression(),
            FloatConstantExpression(),
            StringConstantExpression(),
            IdentifierExpression(),

            MethodExpressionWithoutParameters(),
            MethodExpressionWithIdentifier(),
            MethodExpressionWithOneParameter(),
            MethodExpressionWithTwoParameters(),
            MethodExpressionWithThreeParameters(),
            MethodExpressionWithOneNestedMethod(),
            MethodExpressionWithTwoNestedMethod()
        };


        [Test]
        public void ShouldParseEmptyString()
        {
            // Given
            var parser = new QuerySyntaxTreeParser();

            // When
            var result1 = parser.Parse("");
            var result2 = parser.Parse(null);

            // Then
            Assert.IsTrue(result1 == null || !result1.Any());
            Assert.IsTrue(result2 == null || !result2.Any());
        }

        [Test]
        public void ShouldParseOneExpression([ValueSource(nameof(Expressions))] ExpressionTestCase expression)
        {
            // Given
            var parser = new QuerySyntaxTreeParser();

            // When
            var result = parser.Parse(expression.Expression)?.ToArray();

            // Then
            Assert.IsNotNull(result);
            Assert.AreEqual(1, result.Length);
            expression.Assertion(result[0]);
        }

        [Test]
        public void ShouldParseTwoExpression([ValueSource(nameof(Expressions))] ExpressionTestCase expression1,
                                             [ValueSource(nameof(Expressions))] ExpressionTestCase expression2)
        {
            // Given
            var parser = new QuerySyntaxTreeParser();

            // When
            var result = parser.Parse($"{expression1.Expression}, {expression2.Expression}")?.ToArray();

            // Then
            Assert.IsNotNull(result);
            Assert.AreEqual(2, result.Length);
            expression1.Assertion(result[0]);
            expression2.Assertion(result[1]);
        }


        private static ExpressionTestCase NullConstantExpression()
        {
            return new ExpressionTestCase
            {
                Expression = "null",
                Assertion = e =>
                {
                    var constant = e as LiteralQuerySyntaxNode;
                    Assert.IsNotNull(constant);
                    Assert.AreEqual(null, constant.Value);
                }
            };
        }

        private static ExpressionTestCase TrueConstantExpression()
        {
            return new ExpressionTestCase
            {
                Expression = "true",
                Assertion = e =>
                {
                    var constant = e as LiteralQuerySyntaxNode;
                    Assert.IsNotNull(constant);
                    Assert.AreEqual(true, constant.Value);
                }
            };
        }

        private static ExpressionTestCase FalseConstantExpression()
        {
            return new ExpressionTestCase
            {
                Expression = "false",
                Assertion = e =>
                {
                    var constant = e as LiteralQuerySyntaxNode;
                    Assert.IsNotNull(constant);
                    Assert.AreEqual(false, constant.Value);
                }
            };
        }

        private static ExpressionTestCase IntegerConstantExpression()
        {
            return new ExpressionTestCase
            {
                Expression = "123",
                Assertion = e =>
                {
                    var constant = e as LiteralQuerySyntaxNode;
                    Assert.IsNotNull(constant);
                    Assert.AreEqual(123, constant.Value);
                }
            };
        }

        private static ExpressionTestCase FloatConstantExpression()
        {
            return new ExpressionTestCase
            {
                Expression = "123.456",
                Assertion = e =>
                {
                    var constant = e as LiteralQuerySyntaxNode;
                    Assert.IsNotNull(constant);
                    Assert.AreEqual(123.456, constant.Value);
                }
            };
        }

        private static ExpressionTestCase StringConstantExpression()
        {
            return new ExpressionTestCase
            {
                Expression = @"'A,b,c\t-1+2/3'",
                Assertion = e =>
                {
                    var constant = e as LiteralQuerySyntaxNode;
                    Assert.IsNotNull(constant);
                    Assert.AreEqual("A,b,c\t-1+2/3", constant.Value);
                }
            };
        }

        private static ExpressionTestCase IdentifierExpression()
        {
            return new ExpressionTestCase
            {
                Expression = @"Property1.SubProperty1",
                Assertion = e =>
                {
                    var constant = e as IdentifierNameQuerySyntaxNode;
                    Assert.IsNotNull(constant);
                    Assert.AreEqual("Property1.SubProperty1", constant.Identifier);
                }
            };
        }

        private static ExpressionTestCase MethodExpressionWithoutParameters()
        {
            return new ExpressionTestCase
            {
                Expression = "some()",
                Assertion = e =>
                {
                    var method = e as InvocationQuerySyntaxNode;
                    Assert.IsNotNull(method);
                    Assert.AreEqual("some", method.Name);
                    Assert.IsNotNull(method.Arguments);
                    Assert.AreEqual(0, method.Arguments.Count);
                }
            };
        }

        private static ExpressionTestCase MethodExpressionWithIdentifier()
        {
            return new ExpressionTestCase
            {
                Expression = "some(Property1.SubProperty1)",
                Assertion = e =>
                {
                    var method = e as InvocationQuerySyntaxNode;
                    Assert.IsNotNull(method);
                    Assert.AreEqual("some", method.Name);
                    Assert.IsNotNull(method.Arguments);
                    Assert.AreEqual(1, method.Arguments.Count);

                    var arg1 = method.Arguments[0] as IdentifierNameQuerySyntaxNode;
                    Assert.IsNotNull(arg1);
                    Assert.AreEqual("Property1.SubProperty1", arg1.Identifier);
                }
            };
        }

        private static ExpressionTestCase MethodExpressionWithOneParameter()
        {
            return new ExpressionTestCase
            {
                Expression = "one('Arg1')",
                Assertion = e =>
                {
                    var method = e as InvocationQuerySyntaxNode;
                    Assert.IsNotNull(method);
                    Assert.AreEqual("one", method.Name);
                    Assert.IsNotNull(method.Arguments);
                    Assert.AreEqual(1, method.Arguments.Count);

                    var arg1 = method.Arguments[0] as LiteralQuerySyntaxNode;
                    Assert.IsNotNull(arg1);
                    Assert.AreEqual("Arg1", arg1.Value);
                }
            };
        }

        private static ExpressionTestCase MethodExpressionWithTwoParameters()
        {
            return new ExpressionTestCase
            {
                Expression = "two(123, 45.67)",
                Assertion = e =>
                {
                    var method = e as InvocationQuerySyntaxNode;
                    Assert.IsNotNull(method);
                    Assert.AreEqual("two", method.Name);
                    Assert.IsNotNull(method.Arguments);
                    Assert.AreEqual(2, method.Arguments.Count);

                    var arg1 = method.Arguments[0] as LiteralQuerySyntaxNode;
                    Assert.IsNotNull(arg1);
                    Assert.AreEqual(123, arg1.Value);

                    var arg2 = method.Arguments[1] as LiteralQuerySyntaxNode;
                    Assert.IsNotNull(arg2);
                    Assert.AreEqual(45.67, arg2.Value);
                }
            };
        }

        private static ExpressionTestCase MethodExpressionWithThreeParameters()
        {
            return new ExpressionTestCase
            {
                Expression = "three(456.78, null, true)",
                Assertion = e =>
                {
                    var method = e as InvocationQuerySyntaxNode;
                    Assert.IsNotNull(method);
                    Assert.AreEqual("three", method.Name);
                    Assert.IsNotNull(method.Arguments);
                    Assert.AreEqual(3, method.Arguments.Count);

                    var arg1 = method.Arguments[0] as LiteralQuerySyntaxNode;
                    Assert.IsNotNull(arg1);
                    Assert.AreEqual(456.78, arg1.Value);

                    var arg2 = method.Arguments[1] as LiteralQuerySyntaxNode;
                    Assert.IsNotNull(arg2);
                    Assert.AreEqual(null, arg2.Value);

                    var arg3 = method.Arguments[2] as LiteralQuerySyntaxNode;
                    Assert.IsNotNull(arg3);
                    Assert.AreEqual(true, arg3.Value);
                }
            };
        }

        private static ExpressionTestCase MethodExpressionWithOneNestedMethod()
        {
            return new ExpressionTestCase
            {
                Expression = "some(one(123))",
                Assertion = e =>
                {
                    var method1 = e as InvocationQuerySyntaxNode;
                    Assert.IsNotNull(method1);
                    Assert.AreEqual("some", method1.Name);
                    Assert.IsNotNull(method1.Arguments);
                    Assert.AreEqual(1, method1.Arguments.Count);

                    var method2 = method1.Arguments[0] as InvocationQuerySyntaxNode;
                    Assert.IsNotNull(method2);
                    Assert.AreEqual("one", method2.Name);
                    Assert.IsNotNull(method2.Arguments);
                    Assert.AreEqual(1, method2.Arguments.Count);

                    var arg1 = method2.Arguments[0] as LiteralQuerySyntaxNode;
                    Assert.IsNotNull(arg1);
                    Assert.AreEqual(123, arg1.Value);
                }
            };
        }

        private static ExpressionTestCase MethodExpressionWithTwoNestedMethod()
        {
            return new ExpressionTestCase
            {
                Expression = "some(one(two('Abc')))",
                Assertion = e =>
                {
                    var method1 = e as InvocationQuerySyntaxNode;
                    Assert.IsNotNull(method1);
                    Assert.AreEqual("some", method1.Name);
                    Assert.IsNotNull(method1.Arguments);
                    Assert.AreEqual(1, method1.Arguments.Count);

                    var method2 = method1.Arguments[0] as InvocationQuerySyntaxNode;
                    Assert.IsNotNull(method2);
                    Assert.AreEqual("one", method2.Name);
                    Assert.IsNotNull(method2.Arguments);
                    Assert.AreEqual(1, method2.Arguments.Count);

                    var method3 = method2.Arguments[0] as InvocationQuerySyntaxNode;
                    Assert.IsNotNull(method3);
                    Assert.AreEqual("two", method3.Name);
                    Assert.IsNotNull(method3.Arguments);
                    Assert.AreEqual(1, method3.Arguments.Count);

                    var arg1 = method3.Arguments[0] as LiteralQuerySyntaxNode;
                    Assert.IsNotNull(arg1);
                    Assert.AreEqual("Abc", arg1.Value);
                }
            };
        }


        [DebuggerDisplay("{Expression}")]
        public class ExpressionTestCase
        {
            public string Expression { get; set; }

            public Action<IQuerySyntaxNode> Assertion { get; set; }

            public override string ToString()
            {
                return Expression;
            }
        }
    }
}
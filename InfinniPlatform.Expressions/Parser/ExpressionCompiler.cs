using System.Text;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace InfinniPlatform.Expressions.Parser
{
    public static class ExpressionCompiler
    {
        private static readonly ExpressionSyntaxVisitor Visitor = new ExpressionSyntaxVisitor();

        public static ICompiledExpression Compile(string expression)
        {
            var expressionSyntax = Parse(expression);
            var compiledExpression = expressionSyntax.Accept(Visitor);

            return compiledExpression;
        }

        private static StatementSyntax Parse(string expression)
        {
            StatementSyntax result = null;

            var code = new StringBuilder("class SomeClass { public void SomeMethod(){").Append(expression).Append(";}}");
            var syntaxTree = CSharpSyntaxTree.ParseText(code.ToString());

            SyntaxNode syntaxNode;

            if (syntaxTree.TryGetRoot(out syntaxNode))
            {
                var classNodes = syntaxNode.ChildNodes();

                if (classNodes != null)
                {
                    foreach (var classNode in classNodes)
                    {
                        var methodNodes = classNode.ChildNodes();

                        if (methodNodes != null)
                        {
                            foreach (var methodNode in methodNodes)
                            {
                                var methodSyntax = methodNode as MethodDeclarationSyntax;

                                if (methodSyntax != null && methodSyntax.Body != null &&
                                    methodSyntax.Body.Statements.Count > 0)
                                {
                                    result = methodSyntax.Body.Statements[0];
                                }

                                break;
                            }
                        }

                        break;
                    }
                }
            }

            return result;
        }
    }
}
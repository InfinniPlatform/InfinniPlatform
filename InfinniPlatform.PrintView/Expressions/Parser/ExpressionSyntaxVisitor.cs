using System;
using System.Collections.Generic;
using System.Linq;

using InfinniPlatform.Core.Dynamic;
using InfinniPlatform.PrintView.Expressions.CompiledExpressions;
using InfinniPlatform.PrintView.Properties;

using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace InfinniPlatform.PrintView.Expressions.Parser
{
    internal class ExpressionSyntaxVisitor : CSharpSyntaxVisitor<ICompiledExpression>
    {
        private static string GetIdentifierValue(SimpleNameSyntax node)
        {
            return GetTextValue(node.Identifier);
        }

        private static string GetTextValue(SyntaxToken node)
        {
            return node.ValueText.Trim();
        }

        private static Type GetTypeValue(SyntaxNode node, bool throwOnError = true)
        {
            return (node != null) ? ParserHelpers.ParseType(node.ToFullString(), throwOnError) : null;
        }

        private static Exception IllegalExpression(SyntaxNode node)
        {
            return new NotSupportedException(string.Format(Resources.IllegalExpression, node.ToFullString()));
        }

        public override ICompiledExpression Visit(SyntaxNode node)
        {
            if (node != null)
            {
                return base.Visit(node);
            }

            return ConstantExpression.Null;
        }

        public override ICompiledExpression DefaultVisit(SyntaxNode node)
        {
            return base.DefaultVisit(node);
        }

        /// <summary>
        ///     Called when the visitor visits a IdentifierNameSyntax node.
        /// </summary>
        public override ICompiledExpression VisitIdentifierName(IdentifierNameSyntax node)
        {
            var type = GetTypeValue(node, false);

            if (type != null)
            {
                return new ConstantExpression(type);
            }

            return new GetVariableExpression(GetTextValue(node.Identifier));
        }

        /// <summary>
        ///     Called when the visitor visits a QualifiedNameSyntax node.
        /// </summary>
        public override ICompiledExpression VisitQualifiedName(QualifiedNameSyntax node)
        {
            var type = GetTypeValue(node);
            return new ConstantExpression(type);
        }

        /// <summary>
        ///     Called when the visitor visits a GenericNameSyntax node.
        /// </summary>
        public override ICompiledExpression VisitGenericName(GenericNameSyntax node)
        {
            return base.VisitGenericName(node);
        }

        /// <summary>
        ///     Called when the visitor visits a TypeArgumentListSyntax node.
        /// </summary>
        public override ICompiledExpression VisitTypeArgumentList(TypeArgumentListSyntax node)
        {
            return base.VisitTypeArgumentList(node);
        }

        /// <summary>
        ///     Called when the visitor visits a AliasQualifiedNameSyntax node.
        /// </summary>
        public override ICompiledExpression VisitAliasQualifiedName(AliasQualifiedNameSyntax node)
        {
            var type = GetTypeValue(node);
            return new ConstantExpression(type);
        }

        /// <summary>
        ///     Called when the visitor visits a PredefinedTypeSyntax node.
        /// </summary>
        public override ICompiledExpression VisitPredefinedType(PredefinedTypeSyntax node)
        {
            var type = GetTypeValue(node);
            return new ConstantExpression(type);
        }

        /// <summary>
        ///     Called when the visitor visits a ArrayTypeSyntax node.
        /// </summary>
        public override ICompiledExpression VisitArrayType(ArrayTypeSyntax node)
        {
            return base.VisitArrayType(node);
        }

        /// <summary>
        ///     Called when the visitor visits a ArrayRankSpecifierSyntax node.
        /// </summary>
        public override ICompiledExpression VisitArrayRankSpecifier(ArrayRankSpecifierSyntax node)
        {
            return base.VisitArrayRankSpecifier(node);
        }

        /// <summary>
        ///     Called when the visitor visits a PointerTypeSyntax node.
        /// </summary>
        public override ICompiledExpression VisitPointerType(PointerTypeSyntax node)
        {
            return base.VisitPointerType(node);
        }

        /// <summary>
        ///     Called when the visitor visits a NullableTypeSyntax node.
        /// </summary>
        public override ICompiledExpression VisitNullableType(NullableTypeSyntax node)
        {
            return base.VisitNullableType(node);
        }

        /// <summary>
        ///     Called when the visitor visits a OmittedTypeArgumentSyntax node.
        /// </summary>
        public override ICompiledExpression VisitOmittedTypeArgument(OmittedTypeArgumentSyntax node)
        {
            return base.VisitOmittedTypeArgument(node);
        }

        /// <summary>
        ///     Called when the visitor visits a ParenthesizedExpressionSyntax node.
        /// </summary>
        public override ICompiledExpression VisitParenthesizedExpression(ParenthesizedExpressionSyntax node)
        {
            return Visit(node.Expression);
        }

        /// <summary>
        ///     Called when the visitor visits a PrefixUnaryExpressionSyntax node.
        /// </summary>
        public override ICompiledExpression VisitPrefixUnaryExpression(PrefixUnaryExpressionSyntax node)
        {
            var operand = Visit(node.Operand);
            var operationKind = node.Kind();

            switch (operationKind)
            {
                case SyntaxKind.UnaryMinusExpression:
                    return new UnaryMinusExpression(operand);
                case SyntaxKind.UnaryPlusExpression:
                    return new UnaryPlusExpression(operand);
                case SyntaxKind.LogicalNotExpression:
                    return new LogicalNotExpression(operand);
            }

            throw IllegalExpression(node);
        }

        /// <summary>
        ///     Called when the visitor visits a AwaitExpressionSyntax node.
        /// </summary>
        public override ICompiledExpression VisitAwaitExpression(AwaitExpressionSyntax node)
        {
            return base.VisitAwaitExpression(node);
        }

        /// <summary>
        ///     Called when the visitor visits a PostfixUnaryExpressionSyntax node.
        /// </summary>
        public override ICompiledExpression VisitPostfixUnaryExpression(PostfixUnaryExpressionSyntax node)
        {
            return base.VisitPostfixUnaryExpression(node);
        }

        /// <summary>
        ///     Called when the visitor visits a MemberAccessExpressionSyntax node.
        /// </summary>
        public override ICompiledExpression VisitMemberAccessExpression(MemberAccessExpressionSyntax node)
        {
            if (IsIdentifierExpression(node))
            {
                var type = GetTypeValue(node, false);

                if (type != null)
                {
                    return new ConstantExpression(type);
                }

                type = GetTypeValue(node.Expression, false);

                if (type != null)
                {
                    var typeExpression = new ConstantExpression(type);
                    var staticMemberName = GetIdentifierValue(node.Name);
                    return new MemberAccessExpression(typeExpression, staticMemberName);
                }
            }

            var instanceExpression = Visit(node.Expression);
            var instanceMemberName = GetIdentifierValue(node.Name);
            return new MemberAccessExpression(instanceExpression, instanceMemberName);
        }

        private static bool IsIdentifierExpression(ExpressionSyntax expression)
        {
            while (true)
            {
                var memberExpression = expression as MemberAccessExpressionSyntax;

                if (memberExpression != null)
                {
                    expression = memberExpression.Expression;
                }
                else
                {
                    return (expression is IdentifierNameSyntax);
                }
            }
        }

        /// <summary>
        ///     Called when the visitor visits a ConditionalAccessExpressionSyntax node.
        /// </summary>
        public override ICompiledExpression VisitConditionalAccessExpression(ConditionalAccessExpressionSyntax node)
        {
            return base.VisitConditionalAccessExpression(node);
        }

        /// <summary>
        ///     Called when the visitor visits a MemberBindingExpressionSyntax node.
        /// </summary>
        public override ICompiledExpression VisitMemberBindingExpression(MemberBindingExpressionSyntax node)
        {
            return base.VisitMemberBindingExpression(node);
        }

        /// <summary>
        ///     Called when the visitor visits a ElementBindingExpressionSyntax node.
        /// </summary>
        public override ICompiledExpression VisitElementBindingExpression(ElementBindingExpressionSyntax node)
        {
            return base.VisitElementBindingExpression(node);
        }

        /// <summary>
        ///     Called when the visitor visits a ImplicitElementAccessSyntax node.
        /// </summary>
        public override ICompiledExpression VisitImplicitElementAccess(ImplicitElementAccessSyntax node)
        {
            return base.VisitImplicitElementAccess(node);
        }

        /// <summary>
        ///     Called when the visitor visits a BinaryExpressionSyntax node.
        /// </summary>
        public override ICompiledExpression VisitBinaryExpression(BinaryExpressionSyntax node)
        {
            var left = Visit(node.Left);
            var rigth = Visit(node.Right);
            var operationKind = node.Kind();

            switch (operationKind)
            {
                case SyntaxKind.AddExpression:
                    return new AddExpression(left, rigth);
                case SyntaxKind.SubtractExpression:
                    return new SubtractExpression(left, rigth);
                case SyntaxKind.MultiplyExpression:
                    return new MultiplyExpression(left, rigth);
                case SyntaxKind.DivideExpression:
                    return new DivideExpression(left, rigth);
                case SyntaxKind.ModuloExpression:
                    return new ModuloExpression(left, rigth);
                case SyntaxKind.BitwiseAndExpression:
                    return new BitwiseAndExpression(left, rigth);
                case SyntaxKind.BitwiseOrExpression:
                    return new BitwiseOrExpression(left, rigth);
                case SyntaxKind.ExclusiveOrExpression:
                    return new ExclusiveOrExpression(left, rigth);
                case SyntaxKind.GreaterThanExpression:
                    return new GreaterThanExpression(left, rigth);
                case SyntaxKind.GreaterThanOrEqualExpression:
                    return new GreaterThanOrEqualExpression(left, rigth);
                case SyntaxKind.LessThanExpression:
                    return new LessThanExpression(left, rigth);
                case SyntaxKind.LessThanOrEqualExpression:
                    return new LessThanOrEqualExpression(left, rigth);
                case SyntaxKind.EqualsExpression:
                    return new EqualsExpression(left, rigth);
                case SyntaxKind.NotEqualsExpression:
                    return new NotEqualsExpression(left, rigth);
                case SyntaxKind.CoalesceExpression:
                    return new CoalesceExpression(left, rigth);
                case SyntaxKind.LogicalAndExpression:
                    return new LogicalAndExpression(left, rigth);
                case SyntaxKind.LogicalOrExpression:
                    return new LogicalOrExpression(left, rigth);
                case SyntaxKind.RightShiftExpression:
                    return new RightShiftExpression(left, rigth);
                case SyntaxKind.LeftShiftExpression:
                    return new LeftShiftExpression(left, rigth);
                case SyntaxKind.IsExpression:
                    return new IsExpression(left, rigth);
                case SyntaxKind.AsExpression:
                    return new AsExpression(left, rigth);
            }

            throw IllegalExpression(node);
        }

        /// <summary>
        ///     Called when the visitor visits a AssignmentExpressionSyntax node.
        /// </summary>
        public override ICompiledExpression VisitAssignmentExpression(AssignmentExpressionSyntax node)
        {
            return base.VisitAssignmentExpression(node);
        }

        /// <summary>
        ///     Called when the visitor visits a ConditionalExpressionSyntax node.
        /// </summary>
        public override ICompiledExpression VisitConditionalExpression(ConditionalExpressionSyntax node)
        {
            var condition = Visit(node.Condition);
            var whenTrue = Visit(node.WhenTrue);
            var whenFalse = Visit(node.WhenFalse);

            return new ConditionalExpression(condition, whenTrue, whenFalse);
        }

        /// <summary>
        ///     Called when the visitor visits a ThisExpressionSyntax node.
        /// </summary>
        public override ICompiledExpression VisitThisExpression(ThisExpressionSyntax node)
        {
            return new ThisExpression();
        }

        /// <summary>
        ///     Called when the visitor visits a BaseExpressionSyntax node.
        /// </summary>
        public override ICompiledExpression VisitBaseExpression(BaseExpressionSyntax node)
        {
            return base.VisitBaseExpression(node);
        }

        /// <summary>
        ///     Called when the visitor visits a LiteralExpressionSyntax node.
        /// </summary>
        public override ICompiledExpression VisitLiteralExpression(LiteralExpressionSyntax node)
        {
            return new ConstantExpression(node.Token.Value);
        }

        /// <summary>
        ///     Called when the visitor visits a MakeRefExpressionSyntax node.
        /// </summary>
        public override ICompiledExpression VisitMakeRefExpression(MakeRefExpressionSyntax node)
        {
            return base.VisitMakeRefExpression(node);
        }

        /// <summary>
        ///     Called when the visitor visits a RefTypeExpressionSyntax node.
        /// </summary>
        public override ICompiledExpression VisitRefTypeExpression(RefTypeExpressionSyntax node)
        {
            return base.VisitRefTypeExpression(node);
        }

        /// <summary>
        ///     Called when the visitor visits a RefValueExpressionSyntax node.
        /// </summary>
        public override ICompiledExpression VisitRefValueExpression(RefValueExpressionSyntax node)
        {
            return base.VisitRefValueExpression(node);
        }

        /// <summary>
        ///     Called when the visitor visits a CheckedExpressionSyntax node.
        /// </summary>
        public override ICompiledExpression VisitCheckedExpression(CheckedExpressionSyntax node)
        {
            return base.VisitCheckedExpression(node);
        }

        public override ICompiledExpression VisitDefaultExpression(DefaultExpressionSyntax node)
        {
            if (node.Type != null && !node.Type.IsMissing)
            {
                var defaultType = GetTypeValue(node.Type);
                var defaultValue = ReflectionExtensions.GetDefaultValue(defaultType);

                return new ConstantExpression(defaultValue);
            }

            throw IllegalExpression(node);
        }

        /// <summary>
        ///     Called when the visitor visits a TypeOfExpressionSyntax node.
        /// </summary>
        public override ICompiledExpression VisitTypeOfExpression(TypeOfExpressionSyntax node)
        {
            if (node.Type != null && !node.Type.IsMissing)
            {
                var type = GetTypeValue(node.Type);

                return new ConstantExpression(type);
            }

            throw IllegalExpression(node);
        }

        /// <summary>
        ///     Called when the visitor visits a SizeOfExpressionSyntax node.
        /// </summary>
        public override ICompiledExpression VisitSizeOfExpression(SizeOfExpressionSyntax node)
        {
            if (node.Type != null && !node.Type.IsMissing)
            {
                var type = GetTypeValue(node.Type);

                return new SizeOfExpression(type);
            }

            throw IllegalExpression(node);
        }

        /// <summary>
        ///     Called when the visitor visits a InvocationExpressionSyntax node.
        /// </summary>
        public override ICompiledExpression VisitInvocationExpression(InvocationExpressionSyntax node)
        {
            if (node.Expression != null)
            {
                var expressionNode = node.Expression as MemberAccessExpressionSyntax;

                if (expressionNode != null)
                {
                    var methodName = GetIdentifierValue(expressionNode.Name);
                    Type[] genericArguments = null;

                    var genericName = expressionNode.Name as GenericNameSyntax;

                    if (genericName != null && genericName.TypeArgumentList != null)
                    {
                        genericArguments = genericName.TypeArgumentList.Arguments.Select(t => GetTypeValue(t)).ToArray();
                    }

                    var invokeTarget = Visit(expressionNode.Expression);
                    IEnumerable<ICompiledExpression> invokeArguments = null;

                    if (node.ArgumentList != null)
                    {
                        invokeArguments = node.ArgumentList.Arguments.Select(i => Visit(i.Expression)).ToArray();
                    }

                    return new InvocationExpression(methodName, genericArguments, invokeTarget, invokeArguments);
                }
            }

            throw IllegalExpression(node);
        }

        public override ICompiledExpression VisitElementAccessExpression(ElementAccessExpressionSyntax node)
        {
            var expression = Visit(node.Expression);
            var indexes = node.ArgumentList.Arguments.Select(i => Visit(i.Expression)).ToArray();

            return new ElementAccessExpression(expression, indexes);
        }

        /// <summary>
        ///     Called when the visitor visits a ArgumentListSyntax node.
        /// </summary>
        public override ICompiledExpression VisitArgumentList(ArgumentListSyntax node)
        {
            return base.VisitArgumentList(node);
        }

        /// <summary>
        ///     Called when the visitor visits a BracketedArgumentListSyntax node.
        /// </summary>
        public override ICompiledExpression VisitBracketedArgumentList(BracketedArgumentListSyntax node)
        {
            return base.VisitBracketedArgumentList(node);
        }

        /// <summary>
        ///     Called when the visitor visits a ArgumentSyntax node.
        /// </summary>
        public override ICompiledExpression VisitArgument(ArgumentSyntax node)
        {
            return base.VisitArgument(node);
        }

        /// <summary>
        ///     Called when the visitor visits a NameColonSyntax node.
        /// </summary>
        public override ICompiledExpression VisitNameColon(NameColonSyntax node)
        {
            return base.VisitNameColon(node);
        }

        /// <summary>
        ///     Called when the visitor visits a CastExpressionSyntax node.
        /// </summary>
        public override ICompiledExpression VisitCastExpression(CastExpressionSyntax node)
        {
            if (node.Type != null && !node.Type.IsMissing)
            {
                var type = GetTypeValue(node.Type);
                var expression = Visit(node.Expression);

                return new CastExpression(type, expression);
            }

            throw IllegalExpression(node);
        }

        /// <summary>
        ///     Called when the visitor visits a AnonymousMethodExpressionSyntax node.
        /// </summary>
        public override ICompiledExpression VisitAnonymousMethodExpression(AnonymousMethodExpressionSyntax node)
        {
            IDictionary<string, Type> parameters = null;

            if (node.ParameterList != null)
            {
                parameters = new Dictionary<string, Type>();

                foreach (var parameter in node.ParameterList.Parameters)
                {
                    parameters.Add(GetParameterInfo(parameter));
                }
            }

            var body = Visit(node.Block);

            return new LambdaExpression(parameters, body);
        }

        /// <summary>
        ///     Called when the visitor visits a SimpleLambdaExpressionSyntax node.
        /// </summary>
        public override ICompiledExpression VisitSimpleLambdaExpression(SimpleLambdaExpressionSyntax node)
        {
            IDictionary<string, Type> parameters = null;

            if (node.Parameter != null)
            {
                parameters = new Dictionary<string, Type>();
                parameters.Add(GetParameterInfo(node.Parameter));
            }

            var body = Visit(node.Body);

            return new LambdaExpression(parameters, body);
        }

        /// <summary>
        ///     Called when the visitor visits a ParenthesizedLambdaExpressionSyntax node.
        /// </summary>
        public override ICompiledExpression VisitParenthesizedLambdaExpression(ParenthesizedLambdaExpressionSyntax node)
        {
            IDictionary<string, Type> parameters = null;

            if (node.ParameterList != null)
            {
                parameters = new Dictionary<string, Type>();

                foreach (var parameter in node.ParameterList.Parameters)
                {
                    parameters.Add(GetParameterInfo(parameter));
                }
            }

            var body = Visit(node.Body);

            return new LambdaExpression(parameters, body);
        }

        private static KeyValuePair<string, Type> GetParameterInfo(ParameterSyntax parameter)
        {
            var parameterName = GetTextValue(parameter.Identifier);
            return new KeyValuePair<string, Type>(parameterName, typeof(object));
        }

        /// <summary>
        ///     Called when the visitor visits a InitializerExpressionSyntax node.
        /// </summary>
        public override ICompiledExpression VisitInitializerExpression(InitializerExpressionSyntax node)
        {
            var initializerKind = node.Kind();

            switch (initializerKind)
            {
                case SyntaxKind.ComplexElementInitializerExpression:
                    {
                        var items = node.Expressions.Select(Visit).ToArray();
                        return
                            new FunctionExpression(
                                (dataContext, scope) =>
                                    items.Select(i => (i != null) ? i.Execute(dataContext, scope) : null).ToArray());
                    }
            }

            throw IllegalExpression(node);
        }

        /// <summary>
        ///     Called when the visitor visits a ObjectCreationExpressionSyntax node.
        /// </summary>
        public override ICompiledExpression VisitObjectCreationExpression(ObjectCreationExpressionSyntax node)
        {
            if (node.Type != null && !node.Type.IsMissing)
            {
                var type = GetTypeValue(node.Type);

                IEnumerable<ICompiledExpression> parameters = null;

                if (node.ArgumentList != null)
                {
                    parameters = node.ArgumentList.Arguments.Select(i => Visit(i.Expression)).ToArray();
                }

                IInstanceInitializer initializer = null;

                if (node.Initializer != null)
                {
                    var initializerKind = node.Initializer.Kind();

                    switch (initializerKind)
                    {
                        case SyntaxKind.ObjectInitializerExpression:
                            {
                                var properties = new Dictionary<string, ICompiledExpression>();

                                foreach (var expression in node.Initializer.Expressions)
                                {
                                    var assignmentExpression = expression as AssignmentExpressionSyntax;

                                    if (assignmentExpression != null)
                                    {
                                        var propertyName =
                                            GetIdentifierValue((IdentifierNameSyntax)assignmentExpression.Left);
                                        var propertyValue = Visit(assignmentExpression.Right);

                                        properties[propertyName] = propertyValue;
                                    }
                                }

                                initializer = new ObjectInitializer(properties);
                            }
                            break;
                        case SyntaxKind.CollectionInitializerExpression:
                            {
                                var items = node.Initializer.Expressions.Select(Visit).ToArray();

                                initializer = new CollectionInitializer(items);
                            }
                            break;
                    }
                }

                return new ObjectCreationExpression(type, parameters, initializer);
            }

            throw IllegalExpression(node);
        }

        /// <summary>
        ///     Called when the visitor visits a AnonymousObjectMemberDeclaratorSyntax node.
        /// </summary>
        public override ICompiledExpression VisitAnonymousObjectMemberDeclarator(
            AnonymousObjectMemberDeclaratorSyntax node)
        {
            return base.VisitAnonymousObjectMemberDeclarator(node);
        }

        /// <summary>
        ///     Called when the visitor visits a AnonymousObjectCreationExpressionSyntax node.
        /// </summary>
        public override ICompiledExpression VisitAnonymousObjectCreationExpression(
            AnonymousObjectCreationExpressionSyntax node)
        {
            var properties = new Dictionary<string, ICompiledExpression>();

            foreach (var property in node.Initializers)
            {
                var propertyName = GetIdentifierValue(property.NameEquals.Name);
                var propertyValue = Visit(property.Expression);

                properties[propertyName] = propertyValue;
            }

            return new AnonymousObjectCreationExpression(new ObjectInitializer(properties));
        }

        /// <summary>
        ///     Called when the visitor visits a ImplicitArrayCreationExpressionSyntax node.
        /// </summary>
        public override ICompiledExpression VisitImplicitArrayCreationExpression(
            ImplicitArrayCreationExpressionSyntax node)
        {
            Dictionary<int[], ICompiledExpression> initializers = null;

            if (node.Initializer != null)
            {
                initializers = new Dictionary<int[], ICompiledExpression>();
                GetArrayElementInitializer(initializers, new List<int>(), node.Initializer);
            }

            return new ImplicitArrayCreationExpression(initializers);
        }

        /// <summary>
        ///     Called when the visitor visits a ArrayCreationExpressionSyntax node.
        /// </summary>
        public override ICompiledExpression VisitArrayCreationExpression(ArrayCreationExpressionSyntax node)
        {
            var arrayType = node.Type;
            var elementType = GetTypeValue(arrayType.ElementType);
            var rankSpecifiers = new List<List<ICompiledExpression>>();

            foreach (var rankSpecifier in arrayType.RankSpecifiers)
            {
                var sizes = new List<ICompiledExpression>();

                foreach (var size in rankSpecifier.Sizes)
                {
                    sizes.Add(Visit(size));
                }

                rankSpecifiers.Add(sizes);
            }

            Dictionary<int[], ICompiledExpression> initializers = null;

            if (node.Initializer != null)
            {
                initializers = new Dictionary<int[], ICompiledExpression>();
                GetArrayElementInitializer(initializers, new List<int>(), node.Initializer);
            }

            return new ArrayCreationExpression(elementType, rankSpecifiers, initializers);
        }

        private void GetArrayElementInitializer(IDictionary<int[], ICompiledExpression> initializers,
            List<int> elementIndexes, ExpressionSyntax elementExpression)
        {
            var elementInitializer = elementExpression as InitializerExpressionSyntax;

            if (elementInitializer != null)
            {
                var index = 0;

                foreach (var expression in elementInitializer.Expressions)
                {
                    var subElementIndexes = new List<int>(elementIndexes) { index++ };
                    GetArrayElementInitializer(initializers, subElementIndexes, expression);
                }
            }
            else
            {
                var elementValue = Visit(elementExpression);
                initializers.Add(elementIndexes.ToArray(), elementValue);
            }
        }

        /// <summary>
        ///     Called when the visitor visits a StackAllocArrayCreationExpressionSyntax node.
        /// </summary>
        public override ICompiledExpression VisitStackAllocArrayCreationExpression(
            StackAllocArrayCreationExpressionSyntax node)
        {
            return base.VisitStackAllocArrayCreationExpression(node);
        }

        /// <summary>
        ///     Called when the visitor visits a QueryExpressionSyntax node.
        /// </summary>
        public override ICompiledExpression VisitQueryExpression(QueryExpressionSyntax node)
        {
            return base.VisitQueryExpression(node);
        }

        /// <summary>
        ///     Called when the visitor visits a QueryBodySyntax node.
        /// </summary>
        public override ICompiledExpression VisitQueryBody(QueryBodySyntax node)
        {
            return base.VisitQueryBody(node);
        }

        /// <summary>
        ///     Called when the visitor visits a FromClauseSyntax node.
        /// </summary>
        public override ICompiledExpression VisitFromClause(FromClauseSyntax node)
        {
            return base.VisitFromClause(node);
        }

        /// <summary>
        ///     Called when the visitor visits a LetClauseSyntax node.
        /// </summary>
        public override ICompiledExpression VisitLetClause(LetClauseSyntax node)
        {
            return base.VisitLetClause(node);
        }

        /// <summary>
        ///     Called when the visitor visits a JoinClauseSyntax node.
        /// </summary>
        public override ICompiledExpression VisitJoinClause(JoinClauseSyntax node)
        {
            return base.VisitJoinClause(node);
        }

        /// <summary>
        ///     Called when the visitor visits a JoinIntoClauseSyntax node.
        /// </summary>
        public override ICompiledExpression VisitJoinIntoClause(JoinIntoClauseSyntax node)
        {
            return base.VisitJoinIntoClause(node);
        }

        /// <summary>
        ///     Called when the visitor visits a WhereClauseSyntax node.
        /// </summary>
        public override ICompiledExpression VisitWhereClause(WhereClauseSyntax node)
        {
            return base.VisitWhereClause(node);
        }

        /// <summary>
        ///     Called when the visitor visits a OrderByClauseSyntax node.
        /// </summary>
        public override ICompiledExpression VisitOrderByClause(OrderByClauseSyntax node)
        {
            return base.VisitOrderByClause(node);
        }

        /// <summary>
        ///     Called when the visitor visits a OrderingSyntax node.
        /// </summary>
        public override ICompiledExpression VisitOrdering(OrderingSyntax node)
        {
            return base.VisitOrdering(node);
        }

        /// <summary>
        ///     Called when the visitor visits a SelectClauseSyntax node.
        /// </summary>
        public override ICompiledExpression VisitSelectClause(SelectClauseSyntax node)
        {
            return base.VisitSelectClause(node);
        }

        /// <summary>
        ///     Called when the visitor visits a GroupClauseSyntax node.
        /// </summary>
        public override ICompiledExpression VisitGroupClause(GroupClauseSyntax node)
        {
            return base.VisitGroupClause(node);
        }

        /// <summary>
        ///     Called when the visitor visits a QueryContinuationSyntax node.
        /// </summary>
        public override ICompiledExpression VisitQueryContinuation(QueryContinuationSyntax node)
        {
            return base.VisitQueryContinuation(node);
        }

        /// <summary>
        ///     Called when the visitor visits a OmittedArraySizeExpressionSyntax node.
        /// </summary>
        public override ICompiledExpression VisitOmittedArraySizeExpression(OmittedArraySizeExpressionSyntax node)
        {
            return base.VisitOmittedArraySizeExpression(node);
        }

        /// <summary>
        ///     Called when the visitor visits a GlobalStatementSyntax node.
        /// </summary>
        public override ICompiledExpression VisitGlobalStatement(GlobalStatementSyntax node)
        {
            return base.VisitGlobalStatement(node);
        }

        /// <summary>
        ///     Called when the visitor visits a BlockSyntax node.
        /// </summary>
        public override ICompiledExpression VisitBlock(BlockSyntax node)
        {
            if (node.Statements.Count > 0)
            {
                return Visit(node.Statements[0]);
            }

            return ConstantExpression.Null;
        }

        /// <summary>
        ///     Called when the visitor visits a LocalDeclarationStatementSyntax node.
        /// </summary>
        public override ICompiledExpression VisitLocalDeclarationStatement(LocalDeclarationStatementSyntax node)
        {
            return base.VisitLocalDeclarationStatement(node);
        }

        /// <summary>
        ///     Called when the visitor visits a VariableDeclarationSyntax node.
        /// </summary>
        public override ICompiledExpression VisitVariableDeclaration(VariableDeclarationSyntax node)
        {
            return base.VisitVariableDeclaration(node);
        }

        /// <summary>
        ///     Called when the visitor visits a VariableDeclaratorSyntax node.
        /// </summary>
        public override ICompiledExpression VisitVariableDeclarator(VariableDeclaratorSyntax node)
        {
            return base.VisitVariableDeclarator(node);
        }

        /// <summary>
        ///     Called when the visitor visits a EqualsValueClauseSyntax node.
        /// </summary>
        public override ICompiledExpression VisitEqualsValueClause(EqualsValueClauseSyntax node)
        {
            return base.VisitEqualsValueClause(node);
        }

        /// <summary>
        ///     Called when the visitor visits a ExpressionStatementSyntax node.
        /// </summary>
        public override ICompiledExpression VisitExpressionStatement(ExpressionStatementSyntax node)
        {
            return Visit(node.Expression);
        }

        /// <summary>
        ///     Called when the visitor visits a EmptyStatementSyntax node.
        /// </summary>
        public override ICompiledExpression VisitEmptyStatement(EmptyStatementSyntax node)
        {
            return ConstantExpression.Null;
        }

        /// <summary>
        ///     Called when the visitor visits a LabeledStatementSyntax node.
        /// </summary>
        public override ICompiledExpression VisitLabeledStatement(LabeledStatementSyntax node)
        {
            return base.VisitLabeledStatement(node);
        }

        /// <summary>
        ///     Called when the visitor visits a GotoStatementSyntax node.
        /// </summary>
        public override ICompiledExpression VisitGotoStatement(GotoStatementSyntax node)
        {
            return base.VisitGotoStatement(node);
        }

        /// <summary>
        ///     Called when the visitor visits a BreakStatementSyntax node.
        /// </summary>
        public override ICompiledExpression VisitBreakStatement(BreakStatementSyntax node)
        {
            return base.VisitBreakStatement(node);
        }

        /// <summary>
        ///     Called when the visitor visits a ContinueStatementSyntax node.
        /// </summary>
        public override ICompiledExpression VisitContinueStatement(ContinueStatementSyntax node)
        {
            return base.VisitContinueStatement(node);
        }

        /// <summary>
        ///     Called when the visitor visits a ReturnStatementSyntax node.
        /// </summary>
        public override ICompiledExpression VisitReturnStatement(ReturnStatementSyntax node)
        {
            return Visit(node.Expression);
        }

        /// <summary>
        ///     Called when the visitor visits a ThrowStatementSyntax node.
        /// </summary>
        public override ICompiledExpression VisitThrowStatement(ThrowStatementSyntax node)
        {
            return base.VisitThrowStatement(node);
        }

        /// <summary>
        ///     Called when the visitor visits a YieldStatementSyntax node.
        /// </summary>
        public override ICompiledExpression VisitYieldStatement(YieldStatementSyntax node)
        {
            return base.VisitYieldStatement(node);
        }

        /// <summary>
        ///     Called when the visitor visits a WhileStatementSyntax node.
        /// </summary>
        public override ICompiledExpression VisitWhileStatement(WhileStatementSyntax node)
        {
            return base.VisitWhileStatement(node);
        }

        /// <summary>
        ///     Called when the visitor visits a DoStatementSyntax node.
        /// </summary>
        public override ICompiledExpression VisitDoStatement(DoStatementSyntax node)
        {
            return base.VisitDoStatement(node);
        }

        /// <summary>
        ///     Called when the visitor visits a ForStatementSyntax node.
        /// </summary>
        public override ICompiledExpression VisitForStatement(ForStatementSyntax node)
        {
            return base.VisitForStatement(node);
        }

        /// <summary>
        ///     Called when the visitor visits a ForEachStatementSyntax node.
        /// </summary>
        public override ICompiledExpression VisitForEachStatement(ForEachStatementSyntax node)
        {
            return base.VisitForEachStatement(node);
        }

        /// <summary>
        ///     Called when the visitor visits a UsingStatementSyntax node.
        /// </summary>
        public override ICompiledExpression VisitUsingStatement(UsingStatementSyntax node)
        {
            return base.VisitUsingStatement(node);
        }

        /// <summary>
        ///     Called when the visitor visits a FixedStatementSyntax node.
        /// </summary>
        public override ICompiledExpression VisitFixedStatement(FixedStatementSyntax node)
        {
            return base.VisitFixedStatement(node);
        }

        /// <summary>
        ///     Called when the visitor visits a CheckedStatementSyntax node.
        /// </summary>
        public override ICompiledExpression VisitCheckedStatement(CheckedStatementSyntax node)
        {
            return base.VisitCheckedStatement(node);
        }

        /// <summary>
        ///     Called when the visitor visits a UnsafeStatementSyntax node.
        /// </summary>
        public override ICompiledExpression VisitUnsafeStatement(UnsafeStatementSyntax node)
        {
            return base.VisitUnsafeStatement(node);
        }

        /// <summary>
        ///     Called when the visitor visits a LockStatementSyntax node.
        /// </summary>
        public override ICompiledExpression VisitLockStatement(LockStatementSyntax node)
        {
            return base.VisitLockStatement(node);
        }

        /// <summary>
        ///     Called when the visitor visits a IfStatementSyntax node.
        /// </summary>
        public override ICompiledExpression VisitIfStatement(IfStatementSyntax node)
        {
            var condition = Visit(node.Condition);
            var whenTrue = Visit(node.Statement);
            var whenFalse = Visit(node.Else);

            return new ConditionalExpression(condition, whenTrue, whenFalse);
        }

        /// <summary>
        ///     Called when the visitor visits a ElseClauseSyntax node.
        /// </summary>
        public override ICompiledExpression VisitElseClause(ElseClauseSyntax node)
        {
            return Visit(node.Statement);
        }

        /// <summary>
        ///     Called when the visitor visits a SwitchStatementSyntax node.
        /// </summary>
        public override ICompiledExpression VisitSwitchStatement(SwitchStatementSyntax node)
        {
            var expression = Visit(node.Expression);
            var sections = new Dictionary<ICompiledExpression, ICompiledExpression>();

            foreach (var section in node.Sections)
            {
                if (section.Labels.Count > 0)
                {
                    var label = Visit(section.Labels[0]);
                    var value = (section.Statements.Count > 0) ? Visit(section.Statements[0]) : ConstantExpression.Null;
                    sections.Add(label, value);
                }
            }

            return new SwitchExpression(expression, sections);
        }

        /// <summary>
        ///     Called when the visitor visits a SwitchSectionSyntax node.
        /// </summary>
        public override ICompiledExpression VisitSwitchSection(SwitchSectionSyntax node)
        {
            return base.VisitSwitchSection(node);
        }

        /// <summary>
        ///     Called when the visitor visits a CaseSwitchLabelSyntax node.
        /// </summary>
        public override ICompiledExpression VisitCaseSwitchLabel(CaseSwitchLabelSyntax node)
        {
            return Visit(node.Value);
        }

        /// <summary>
        ///     Called when the visitor visits a DefaultSwitchLabelSyntax node.
        /// </summary>
        public override ICompiledExpression VisitDefaultSwitchLabel(DefaultSwitchLabelSyntax node)
        {
            return new DefaultSwitchLabelExpression();
        }

        /// <summary>
        ///     Called when the visitor visits a TryStatementSyntax node.
        /// </summary>
        public override ICompiledExpression VisitTryStatement(TryStatementSyntax node)
        {
            return base.VisitTryStatement(node);
        }

        /// <summary>
        ///     Called when the visitor visits a CatchClauseSyntax node.
        /// </summary>
        public override ICompiledExpression VisitCatchClause(CatchClauseSyntax node)
        {
            return base.VisitCatchClause(node);
        }

        /// <summary>
        ///     Called when the visitor visits a CatchDeclarationSyntax node.
        /// </summary>
        public override ICompiledExpression VisitCatchDeclaration(CatchDeclarationSyntax node)
        {
            return base.VisitCatchDeclaration(node);
        }

        /// <summary>
        ///     Called when the visitor visits a CatchFilterClauseSyntax node.
        /// </summary>
        public override ICompiledExpression VisitCatchFilterClause(CatchFilterClauseSyntax node)
        {
            return base.VisitCatchFilterClause(node);
        }

        /// <summary>
        ///     Called when the visitor visits a FinallyClauseSyntax node.
        /// </summary>
        public override ICompiledExpression VisitFinallyClause(FinallyClauseSyntax node)
        {
            return base.VisitFinallyClause(node);
        }

        /// <summary>
        ///     Called when the visitor visits a CompilationUnitSyntax node.
        /// </summary>
        public override ICompiledExpression VisitCompilationUnit(CompilationUnitSyntax node)
        {
            return base.VisitCompilationUnit(node);
        }

        /// <summary>
        ///     Called when the visitor visits a ExternAliasDirectiveSyntax node.
        /// </summary>
        public override ICompiledExpression VisitExternAliasDirective(ExternAliasDirectiveSyntax node)
        {
            return base.VisitExternAliasDirective(node);
        }

        /// <summary>
        ///     Called when the visitor visits a UsingDirectiveSyntax node.
        /// </summary>
        public override ICompiledExpression VisitUsingDirective(UsingDirectiveSyntax node)
        {
            return base.VisitUsingDirective(node);
        }

        /// <summary>
        ///     Called when the visitor visits a NamespaceDeclarationSyntax node.
        /// </summary>
        public override ICompiledExpression VisitNamespaceDeclaration(NamespaceDeclarationSyntax node)
        {
            return base.VisitNamespaceDeclaration(node);
        }

        /// <summary>
        ///     Called when the visitor visits a AttributeListSyntax node.
        /// </summary>
        public override ICompiledExpression VisitAttributeList(AttributeListSyntax node)
        {
            return base.VisitAttributeList(node);
        }

        /// <summary>
        ///     Called when the visitor visits a AttributeTargetSpecifierSyntax node.
        /// </summary>
        public override ICompiledExpression VisitAttributeTargetSpecifier(AttributeTargetSpecifierSyntax node)
        {
            return base.VisitAttributeTargetSpecifier(node);
        }

        /// <summary>
        ///     Called when the visitor visits a AttributeSyntax node.
        /// </summary>
        public override ICompiledExpression VisitAttribute(AttributeSyntax node)
        {
            return base.VisitAttribute(node);
        }

        /// <summary>
        ///     Called when the visitor visits a AttributeArgumentListSyntax node.
        /// </summary>
        public override ICompiledExpression VisitAttributeArgumentList(AttributeArgumentListSyntax node)
        {
            return base.VisitAttributeArgumentList(node);
        }

        /// <summary>
        ///     Called when the visitor visits a AttributeArgumentSyntax node.
        /// </summary>
        public override ICompiledExpression VisitAttributeArgument(AttributeArgumentSyntax node)
        {
            return base.VisitAttributeArgument(node);
        }

        /// <summary>
        ///     Called when the visitor visits a NameEqualsSyntax node.
        /// </summary>
        public override ICompiledExpression VisitNameEquals(NameEqualsSyntax node)
        {
            return base.VisitNameEquals(node);
        }

        /// <summary>
        ///     Called when the visitor visits a TypeParameterListSyntax node.
        /// </summary>
        public override ICompiledExpression VisitTypeParameterList(TypeParameterListSyntax node)
        {
            return base.VisitTypeParameterList(node);
        }

        /// <summary>
        ///     Called when the visitor visits a TypeParameterSyntax node.
        /// </summary>
        public override ICompiledExpression VisitTypeParameter(TypeParameterSyntax node)
        {
            return base.VisitTypeParameter(node);
        }

        /// <summary>
        ///     Called when the visitor visits a ClassDeclarationSyntax node.
        /// </summary>
        public override ICompiledExpression VisitClassDeclaration(ClassDeclarationSyntax node)
        {
            return base.VisitClassDeclaration(node);
        }

        /// <summary>
        ///     Called when the visitor visits a StructDeclarationSyntax node.
        /// </summary>
        public override ICompiledExpression VisitStructDeclaration(StructDeclarationSyntax node)
        {
            return base.VisitStructDeclaration(node);
        }

        /// <summary>
        ///     Called when the visitor visits a InterfaceDeclarationSyntax node.
        /// </summary>
        public override ICompiledExpression VisitInterfaceDeclaration(InterfaceDeclarationSyntax node)
        {
            return base.VisitInterfaceDeclaration(node);
        }

        /// <summary>
        ///     Called when the visitor visits a EnumDeclarationSyntax node.
        /// </summary>
        public override ICompiledExpression VisitEnumDeclaration(EnumDeclarationSyntax node)
        {
            return base.VisitEnumDeclaration(node);
        }

        /// <summary>
        ///     Called when the visitor visits a DelegateDeclarationSyntax node.
        /// </summary>
        public override ICompiledExpression VisitDelegateDeclaration(DelegateDeclarationSyntax node)
        {
            return base.VisitDelegateDeclaration(node);
        }

        /// <summary>
        ///     Called when the visitor visits a EnumMemberDeclarationSyntax node.
        /// </summary>
        public override ICompiledExpression VisitEnumMemberDeclaration(EnumMemberDeclarationSyntax node)
        {
            return base.VisitEnumMemberDeclaration(node);
        }

        /// <summary>
        ///     Called when the visitor visits a BaseListSyntax node.
        /// </summary>
        public override ICompiledExpression VisitBaseList(BaseListSyntax node)
        {
            return base.VisitBaseList(node);
        }

        /// <summary>
        ///     Called when the visitor visits a SimpleBaseTypeSyntax node.
        /// </summary>
        public override ICompiledExpression VisitSimpleBaseType(SimpleBaseTypeSyntax node)
        {
            return base.VisitSimpleBaseType(node);
        }

        /// <summary>
        ///     Called when the visitor visits a TypeParameterConstraintClauseSyntax node.
        /// </summary>
        public override ICompiledExpression VisitTypeParameterConstraintClause(TypeParameterConstraintClauseSyntax node)
        {
            return base.VisitTypeParameterConstraintClause(node);
        }

        /// <summary>
        ///     Called when the visitor visits a ConstructorConstraintSyntax node.
        /// </summary>
        public override ICompiledExpression VisitConstructorConstraint(ConstructorConstraintSyntax node)
        {
            return base.VisitConstructorConstraint(node);
        }

        /// <summary>
        ///     Called when the visitor visits a ClassOrStructConstraintSyntax node.
        /// </summary>
        public override ICompiledExpression VisitClassOrStructConstraint(ClassOrStructConstraintSyntax node)
        {
            return base.VisitClassOrStructConstraint(node);
        }

        /// <summary>
        ///     Called when the visitor visits a TypeConstraintSyntax node.
        /// </summary>
        public override ICompiledExpression VisitTypeConstraint(TypeConstraintSyntax node)
        {
            return base.VisitTypeConstraint(node);
        }

        /// <summary>
        ///     Called when the visitor visits a FieldDeclarationSyntax node.
        /// </summary>
        public override ICompiledExpression VisitFieldDeclaration(FieldDeclarationSyntax node)
        {
            return base.VisitFieldDeclaration(node);
        }

        /// <summary>
        ///     Called when the visitor visits a EventFieldDeclarationSyntax node.
        /// </summary>
        public override ICompiledExpression VisitEventFieldDeclaration(EventFieldDeclarationSyntax node)
        {
            return base.VisitEventFieldDeclaration(node);
        }

        /// <summary>
        ///     Called when the visitor visits a ExplicitInterfaceSpecifierSyntax node.
        /// </summary>
        public override ICompiledExpression VisitExplicitInterfaceSpecifier(ExplicitInterfaceSpecifierSyntax node)
        {
            return base.VisitExplicitInterfaceSpecifier(node);
        }

        /// <summary>
        ///     Called when the visitor visits a MethodDeclarationSyntax node.
        /// </summary>
        public override ICompiledExpression VisitMethodDeclaration(MethodDeclarationSyntax node)
        {
            return base.VisitMethodDeclaration(node);
        }

        /// <summary>
        ///     Called when the visitor visits a OperatorDeclarationSyntax node.
        /// </summary>
        public override ICompiledExpression VisitOperatorDeclaration(OperatorDeclarationSyntax node)
        {
            return base.VisitOperatorDeclaration(node);
        }

        /// <summary>
        ///     Called when the visitor visits a ConversionOperatorDeclarationSyntax node.
        /// </summary>
        public override ICompiledExpression VisitConversionOperatorDeclaration(ConversionOperatorDeclarationSyntax node)
        {
            return base.VisitConversionOperatorDeclaration(node);
        }

        /// <summary>
        ///     Called when the visitor visits a ConstructorDeclarationSyntax node.
        /// </summary>
        public override ICompiledExpression VisitConstructorDeclaration(ConstructorDeclarationSyntax node)
        {
            return base.VisitConstructorDeclaration(node);
        }

        /// <summary>
        ///     Called when the visitor visits a ConstructorInitializerSyntax node.
        /// </summary>
        public override ICompiledExpression VisitConstructorInitializer(ConstructorInitializerSyntax node)
        {
            return base.VisitConstructorInitializer(node);
        }

        /// <summary>
        ///     Called when the visitor visits a DestructorDeclarationSyntax node.
        /// </summary>
        public override ICompiledExpression VisitDestructorDeclaration(DestructorDeclarationSyntax node)
        {
            return base.VisitDestructorDeclaration(node);
        }

        /// <summary>
        ///     Called when the visitor visits a PropertyDeclarationSyntax node.
        /// </summary>
        public override ICompiledExpression VisitPropertyDeclaration(PropertyDeclarationSyntax node)
        {
            return base.VisitPropertyDeclaration(node);
        }

        /// <summary>
        ///     Called when the visitor visits a ArrowExpressionClauseSyntax node.
        /// </summary>
        public override ICompiledExpression VisitArrowExpressionClause(ArrowExpressionClauseSyntax node)
        {
            return base.VisitArrowExpressionClause(node);
        }

        /// <summary>
        ///     Called when the visitor visits a EventDeclarationSyntax node.
        /// </summary>
        public override ICompiledExpression VisitEventDeclaration(EventDeclarationSyntax node)
        {
            return base.VisitEventDeclaration(node);
        }

        /// <summary>
        ///     Called when the visitor visits a IndexerDeclarationSyntax node.
        /// </summary>
        public override ICompiledExpression VisitIndexerDeclaration(IndexerDeclarationSyntax node)
        {
            return base.VisitIndexerDeclaration(node);
        }

        /// <summary>
        ///     Called when the visitor visits a AccessorListSyntax node.
        /// </summary>
        public override ICompiledExpression VisitAccessorList(AccessorListSyntax node)
        {
            return base.VisitAccessorList(node);
        }

        /// <summary>
        ///     Called when the visitor visits a AccessorDeclarationSyntax node.
        /// </summary>
        public override ICompiledExpression VisitAccessorDeclaration(AccessorDeclarationSyntax node)
        {
            return base.VisitAccessorDeclaration(node);
        }

        /// <summary>
        ///     Called when the visitor visits a ParameterListSyntax node.
        /// </summary>
        public override ICompiledExpression VisitParameterList(ParameterListSyntax node)
        {
            return base.VisitParameterList(node);
        }

        /// <summary>
        ///     Called when the visitor visits a BracketedParameterListSyntax node.
        /// </summary>
        public override ICompiledExpression VisitBracketedParameterList(BracketedParameterListSyntax node)
        {
            return base.VisitBracketedParameterList(node);
        }

        /// <summary>
        ///     Called when the visitor visits a ParameterSyntax node.
        /// </summary>
        public override ICompiledExpression VisitParameter(ParameterSyntax node)
        {
            return base.VisitParameter(node);
        }

        /// <summary>
        ///     Called when the visitor visits a IncompleteMemberSyntax node.
        /// </summary>
        public override ICompiledExpression VisitIncompleteMember(IncompleteMemberSyntax node)
        {
            return base.VisitIncompleteMember(node);
        }

        /// <summary>
        ///     Called when the visitor visits a SkippedTokensTriviaSyntax node.
        /// </summary>
        public override ICompiledExpression VisitSkippedTokensTrivia(SkippedTokensTriviaSyntax node)
        {
            return base.VisitSkippedTokensTrivia(node);
        }

        /// <summary>
        ///     Called when the visitor visits a DocumentationCommentTriviaSyntax node.
        /// </summary>
        public override ICompiledExpression VisitDocumentationCommentTrivia(DocumentationCommentTriviaSyntax node)
        {
            return base.VisitDocumentationCommentTrivia(node);
        }

        /// <summary>
        ///     Called when the visitor visits a TypeCrefSyntax node.
        /// </summary>
        public override ICompiledExpression VisitTypeCref(TypeCrefSyntax node)
        {
            return base.VisitTypeCref(node);
        }

        /// <summary>
        ///     Called when the visitor visits a QualifiedCrefSyntax node.
        /// </summary>
        public override ICompiledExpression VisitQualifiedCref(QualifiedCrefSyntax node)
        {
            return base.VisitQualifiedCref(node);
        }

        /// <summary>
        ///     Called when the visitor visits a NameMemberCrefSyntax node.
        /// </summary>
        public override ICompiledExpression VisitNameMemberCref(NameMemberCrefSyntax node)
        {
            return base.VisitNameMemberCref(node);
        }

        /// <summary>
        ///     Called when the visitor visits a IndexerMemberCrefSyntax node.
        /// </summary>
        public override ICompiledExpression VisitIndexerMemberCref(IndexerMemberCrefSyntax node)
        {
            return base.VisitIndexerMemberCref(node);
        }

        /// <summary>
        ///     Called when the visitor visits a OperatorMemberCrefSyntax node.
        /// </summary>
        public override ICompiledExpression VisitOperatorMemberCref(OperatorMemberCrefSyntax node)
        {
            return base.VisitOperatorMemberCref(node);
        }

        /// <summary>
        ///     Called when the visitor visits a ConversionOperatorMemberCrefSyntax node.
        /// </summary>
        public override ICompiledExpression VisitConversionOperatorMemberCref(ConversionOperatorMemberCrefSyntax node)
        {
            return base.VisitConversionOperatorMemberCref(node);
        }

        /// <summary>
        ///     Called when the visitor visits a CrefParameterListSyntax node.
        /// </summary>
        public override ICompiledExpression VisitCrefParameterList(CrefParameterListSyntax node)
        {
            return base.VisitCrefParameterList(node);
        }

        /// <summary>
        ///     Called when the visitor visits a CrefBracketedParameterListSyntax node.
        /// </summary>
        public override ICompiledExpression VisitCrefBracketedParameterList(CrefBracketedParameterListSyntax node)
        {
            return base.VisitCrefBracketedParameterList(node);
        }

        /// <summary>
        ///     Called when the visitor visits a CrefParameterSyntax node.
        /// </summary>
        public override ICompiledExpression VisitCrefParameter(CrefParameterSyntax node)
        {
            return base.VisitCrefParameter(node);
        }

        /// <summary>
        ///     Called when the visitor visits a XmlElementSyntax node.
        /// </summary>
        public override ICompiledExpression VisitXmlElement(XmlElementSyntax node)
        {
            return base.VisitXmlElement(node);
        }

        /// <summary>
        ///     Called when the visitor visits a XmlElementStartTagSyntax node.
        /// </summary>
        public override ICompiledExpression VisitXmlElementStartTag(XmlElementStartTagSyntax node)
        {
            return base.VisitXmlElementStartTag(node);
        }

        /// <summary>
        ///     Called when the visitor visits a XmlElementEndTagSyntax node.
        /// </summary>
        public override ICompiledExpression VisitXmlElementEndTag(XmlElementEndTagSyntax node)
        {
            return base.VisitXmlElementEndTag(node);
        }

        /// <summary>
        ///     Called when the visitor visits a XmlEmptyElementSyntax node.
        /// </summary>
        public override ICompiledExpression VisitXmlEmptyElement(XmlEmptyElementSyntax node)
        {
            return base.VisitXmlEmptyElement(node);
        }

        /// <summary>
        ///     Called when the visitor visits a XmlNameSyntax node.
        /// </summary>
        public override ICompiledExpression VisitXmlName(XmlNameSyntax node)
        {
            return base.VisitXmlName(node);
        }

        /// <summary>
        ///     Called when the visitor visits a XmlPrefixSyntax node.
        /// </summary>
        public override ICompiledExpression VisitXmlPrefix(XmlPrefixSyntax node)
        {
            return base.VisitXmlPrefix(node);
        }

        /// <summary>
        ///     Called when the visitor visits a XmlTextAttributeSyntax node.
        /// </summary>
        public override ICompiledExpression VisitXmlTextAttribute(XmlTextAttributeSyntax node)
        {
            return base.VisitXmlTextAttribute(node);
        }

        /// <summary>
        ///     Called when the visitor visits a XmlCrefAttributeSyntax node.
        /// </summary>
        public override ICompiledExpression VisitXmlCrefAttribute(XmlCrefAttributeSyntax node)
        {
            return base.VisitXmlCrefAttribute(node);
        }

        /// <summary>
        ///     Called when the visitor visits a XmlNameAttributeSyntax node.
        /// </summary>
        public override ICompiledExpression VisitXmlNameAttribute(XmlNameAttributeSyntax node)
        {
            return base.VisitXmlNameAttribute(node);
        }

        /// <summary>
        ///     Called when the visitor visits a XmlTextSyntax node.
        /// </summary>
        public override ICompiledExpression VisitXmlText(XmlTextSyntax node)
        {
            return base.VisitXmlText(node);
        }

        /// <summary>
        ///     Called when the visitor visits a XmlCDataSectionSyntax node.
        /// </summary>
        public override ICompiledExpression VisitXmlCDataSection(XmlCDataSectionSyntax node)
        {
            return base.VisitXmlCDataSection(node);
        }

        /// <summary>
        ///     Called when the visitor visits a XmlProcessingInstructionSyntax node.
        /// </summary>
        public override ICompiledExpression VisitXmlProcessingInstruction(XmlProcessingInstructionSyntax node)
        {
            return base.VisitXmlProcessingInstruction(node);
        }

        /// <summary>
        ///     Called when the visitor visits a XmlCommentSyntax node.
        /// </summary>
        public override ICompiledExpression VisitXmlComment(XmlCommentSyntax node)
        {
            return base.VisitXmlComment(node);
        }

        /// <summary>
        ///     Called when the visitor visits a IfDirectiveTriviaSyntax node.
        /// </summary>
        public override ICompiledExpression VisitIfDirectiveTrivia(IfDirectiveTriviaSyntax node)
        {
            return base.VisitIfDirectiveTrivia(node);
        }

        /// <summary>
        ///     Called when the visitor visits a ElifDirectiveTriviaSyntax node.
        /// </summary>
        public override ICompiledExpression VisitElifDirectiveTrivia(ElifDirectiveTriviaSyntax node)
        {
            return base.VisitElifDirectiveTrivia(node);
        }

        /// <summary>
        ///     Called when the visitor visits a ElseDirectiveTriviaSyntax node.
        /// </summary>
        public override ICompiledExpression VisitElseDirectiveTrivia(ElseDirectiveTriviaSyntax node)
        {
            return base.VisitElseDirectiveTrivia(node);
        }

        /// <summary>
        ///     Called when the visitor visits a EndIfDirectiveTriviaSyntax node.
        /// </summary>
        public override ICompiledExpression VisitEndIfDirectiveTrivia(EndIfDirectiveTriviaSyntax node)
        {
            return base.VisitEndIfDirectiveTrivia(node);
        }

        /// <summary>
        ///     Called when the visitor visits a RegionDirectiveTriviaSyntax node.
        /// </summary>
        public override ICompiledExpression VisitRegionDirectiveTrivia(RegionDirectiveTriviaSyntax node)
        {
            return base.VisitRegionDirectiveTrivia(node);
        }

        /// <summary>
        ///     Called when the visitor visits a EndRegionDirectiveTriviaSyntax node.
        /// </summary>
        public override ICompiledExpression VisitEndRegionDirectiveTrivia(EndRegionDirectiveTriviaSyntax node)
        {
            return base.VisitEndRegionDirectiveTrivia(node);
        }

        /// <summary>
        ///     Called when the visitor visits a ErrorDirectiveTriviaSyntax node.
        /// </summary>
        public override ICompiledExpression VisitErrorDirectiveTrivia(ErrorDirectiveTriviaSyntax node)
        {
            return base.VisitErrorDirectiveTrivia(node);
        }

        /// <summary>
        ///     Called when the visitor visits a WarningDirectiveTriviaSyntax node.
        /// </summary>
        public override ICompiledExpression VisitWarningDirectiveTrivia(WarningDirectiveTriviaSyntax node)
        {
            return base.VisitWarningDirectiveTrivia(node);
        }

        /// <summary>
        ///     Called when the visitor visits a BadDirectiveTriviaSyntax node.
        /// </summary>
        public override ICompiledExpression VisitBadDirectiveTrivia(BadDirectiveTriviaSyntax node)
        {
            return base.VisitBadDirectiveTrivia(node);
        }

        /// <summary>
        ///     Called when the visitor visits a DefineDirectiveTriviaSyntax node.
        /// </summary>
        public override ICompiledExpression VisitDefineDirectiveTrivia(DefineDirectiveTriviaSyntax node)
        {
            return base.VisitDefineDirectiveTrivia(node);
        }

        /// <summary>
        ///     Called when the visitor visits a UndefDirectiveTriviaSyntax node.
        /// </summary>
        public override ICompiledExpression VisitUndefDirectiveTrivia(UndefDirectiveTriviaSyntax node)
        {
            return base.VisitUndefDirectiveTrivia(node);
        }

        /// <summary>
        ///     Called when the visitor visits a LineDirectiveTriviaSyntax node.
        /// </summary>
        public override ICompiledExpression VisitLineDirectiveTrivia(LineDirectiveTriviaSyntax node)
        {
            return base.VisitLineDirectiveTrivia(node);
        }

        /// <summary>
        ///     Called when the visitor visits a PragmaWarningDirectiveTriviaSyntax node.
        /// </summary>
        public override ICompiledExpression VisitPragmaWarningDirectiveTrivia(PragmaWarningDirectiveTriviaSyntax node)
        {
            return base.VisitPragmaWarningDirectiveTrivia(node);
        }

        /// <summary>
        ///     Called when the visitor visits a PragmaChecksumDirectiveTriviaSyntax node.
        /// </summary>
        public override ICompiledExpression VisitPragmaChecksumDirectiveTrivia(PragmaChecksumDirectiveTriviaSyntax node)
        {
            return base.VisitPragmaChecksumDirectiveTrivia(node);
        }

        /// <summary>
        ///     Called when the visitor visits a ReferenceDirectiveTriviaSyntax node.
        /// </summary>
        public override ICompiledExpression VisitReferenceDirectiveTrivia(ReferenceDirectiveTriviaSyntax node)
        {
            return base.VisitReferenceDirectiveTrivia(node);
        }

        public override ICompiledExpression VisitInterpolatedStringText(InterpolatedStringTextSyntax node)
        {
            return base.VisitInterpolatedStringText(node);
        }

        public override ICompiledExpression VisitInterpolatedStringExpression(InterpolatedStringExpressionSyntax node)
        {
            return base.VisitInterpolatedStringExpression(node);
        }

        public override ICompiledExpression VisitInterpolationAlignmentClause(InterpolationAlignmentClauseSyntax node)
        {
            return base.VisitInterpolationAlignmentClause(node);
        }

        public override ICompiledExpression VisitInterpolationFormatClause(InterpolationFormatClauseSyntax node)
        {
            return base.VisitInterpolationFormatClause(node);
        }

        public override ICompiledExpression VisitInterpolation(InterpolationSyntax node)
        {
            return base.VisitInterpolation(node);
        }
    }
}
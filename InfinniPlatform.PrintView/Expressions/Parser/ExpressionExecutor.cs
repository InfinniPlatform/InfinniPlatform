using System;
using System.Collections.Concurrent;
using System.Diagnostics;

using InfinniPlatform.PrintView.Properties;

namespace InfinniPlatform.PrintView.Expressions.Parser
{
    internal static class ExpressionExecutor
    {
        private static readonly ConcurrentDictionary<string, ICompiledExpression> Expressions
            = new ConcurrentDictionary<string, ICompiledExpression>();

        public static object Execute(string expression, object dataContext = null)
        {
            var compiledExpression = Compile(expression);

            return compiledExpression?.Execute(dataContext, new ExpressionScope());
        }

        private static ICompiledExpression Compile(string expression)
        {
            // Простейшая логика кэширования результата компиляции выражений
            // Возможно, в дальнейшем потребуется усовершенствовать этот код

            ICompiledExpression compiledExpression = null;

            if (!string.IsNullOrWhiteSpace(expression))
            {
                expression = expression.Trim();

                if (!Expressions.TryGetValue(expression, out compiledExpression))
                {
                    try
                    {
                        compiledExpression = ExpressionCompiler.Compile(expression);
                    }
                    catch (Exception exception)
                    {
                        // Пока просто игнорируем ошибки компиляции
                        compiledExpression = null;

                        // TODO: Trace.TraceWarning(Resources.CompilationError, expression, exception);
                    }

                    compiledExpression = Expressions.GetOrAdd(expression, compiledExpression);
                }
            }

            return compiledExpression;
        }
    }
}
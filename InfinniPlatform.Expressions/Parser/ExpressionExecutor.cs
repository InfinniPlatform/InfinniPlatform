using System.Collections.Generic;

namespace InfinniPlatform.Expressions.Parser
{
    public static class ExpressionExecutor
    {
        private static readonly Dictionary<string, ICompiledExpression> Expressions
            = new Dictionary<string, ICompiledExpression>();

        public static object Execute(string expression, object dataContext = null)
        {
            var compiledExpression = Compile(expression);

            if (compiledExpression != null)
            {
                return compiledExpression.Execute(dataContext, new ExpressionScope());
            }

            return null;
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
                    lock (Expressions)
                    {
                        if (!Expressions.TryGetValue(expression, out compiledExpression))
                        {
                            try
                            {
                                compiledExpression = ExpressionCompiler.Compile(expression);
                            }
                            catch
                            {
                                // Пока просто игнорируем ошибки компиляции
                                compiledExpression = null;
                            }

                            Expressions.Add(expression, compiledExpression);
                        }
                    }
                }
            }

            return compiledExpression;
        }
    }
}
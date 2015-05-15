using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace InfinniPlatform.Expressions.CompiledExpressions
{
	sealed class LambdaExpression : ICompiledExpression
	{
		public LambdaExpression(IDictionary<string, Type> parameters, ICompiledExpression body)
		{
			_parameters = (parameters != null) ? parameters.Select(p => CreateParameter(p.Key, p.Value)).ToArray() : EmptyParameters;
			_body = body;
		}


		private readonly ICompiledExpression _body;
		private readonly IList<ParameterExpression> _parameters;


		private static readonly ParameterExpression[] EmptyParameters
			= { };

		private static readonly MethodInfo ExecuteLambdaExpressionMethod
			= typeof(LambdaExpression).GetMethod("ExecuteLambdaExpression", BindingFlags.Instance | BindingFlags.NonPublic);

		private object ExecuteLambdaExpression(object dataContext, ExpressionScope scope, object[] arguments)
		{
			if (_body != null)
			{
				var lambdaScope = new ExpressionScope(scope);

				// Заполнение контекста параметрами функции

				if (arguments != null)
				{
					for (var i = 0; i < arguments.Length && i < _parameters.Count; ++i)
					{
						lambdaScope.DeclareVariable(_parameters[i].Name, arguments[i]);
					}
				}

				return _body.Execute(dataContext, lambdaScope);
			}

			return null;
		}

		private static ParameterExpression CreateParameter(string name, Type type)
		{
			return Expression.Parameter(type ?? typeof(object), name);
		}


		public object Execute(object dataContext, ExpressionScope scope)
		{
			// Формирование делегата для исполнения функции

			return Expression.Lambda(Expression.Call(Expression.Constant(this),
													 ExecuteLambdaExpressionMethod,
													 Expression.Constant(dataContext),
													 Expression.Constant(scope),
													 Expression.NewArrayInit(typeof(object), _parameters)),
									 _parameters)
							 .Compile();
		}
	}
}
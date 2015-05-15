﻿namespace InfinniPlatform.Expressions.CompiledExpressions
{
	sealed class ThisExpression : ICompiledExpression
	{
		public object Execute(object dataContext, ExpressionScope scope)
		{
			return dataContext;
		}
	}
}
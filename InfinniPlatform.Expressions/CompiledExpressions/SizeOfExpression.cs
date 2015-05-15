using System;
using System.Runtime.InteropServices;

namespace InfinniPlatform.Expressions.CompiledExpressions
{
	sealed class SizeOfExpression : ICompiledExpression
	{
		private readonly Type _type;

		public SizeOfExpression(Type type)
		{
			_type = type;
		}

		public object Execute(object dataContext, ExpressionScope scope)
		{
			return Marshal.SizeOf(_type);
		}
	}
}
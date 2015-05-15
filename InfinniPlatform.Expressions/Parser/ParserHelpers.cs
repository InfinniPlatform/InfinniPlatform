using System;
using System.Collections.Generic;
using System.Text;

using InfinniPlatform.Expressions.BuiltInTypes;
using InfinniPlatform.Expressions.Properties;

namespace InfinniPlatform.Expressions.Parser
{
	static class ParserHelpers
	{
		public static Type ParseType(object value, bool throwOnError = true)
		{
			var type = (value as Type);

			if (type == null && value is string)
			{
				var typeName = (value as string).Trim();

				if (!BuiltInTypes.TryGetValue(typeName, out type))
				{
					typeName = GetQualifiedTypeName(typeName);
					type = Type.GetType(typeName);
				}
			}

			if (type == null && throwOnError)
			{
				throw new ArgumentException(string.Format(Resources.UnknownType, value), "value");
			}

			return type;
		}

		private static string GetQualifiedTypeName(string value)
		{
			string result = null;

			if (!string.IsNullOrEmpty(value))
			{
				value = value.Trim();

				if (!string.IsNullOrEmpty(value))
				{
					var name = new StringBuilder();
					var types = new Stack<Tuple<string, List<string>>>();

					foreach (var c in value)
					{
						switch (c)
						{
							// Начало параметров generic-типа
							case '<':
								{
									var typeName = name.ToString();
									var genericTypes = new List<string>();
									types.Push(new Tuple<string, List<string>>(typeName, genericTypes));
									name.Clear();
								}
								break;
							// Очередной параметр generic-типа
							case ',':
								{
									// Добавление параметра родительскому типу
									if (types.Count > 0 && name.Length > 0)
									{
										var parentType = types.Peek();
										var typeName = name.ToString();
										var genericTypes = parentType.Item2;
										genericTypes.Add(typeName);
									}

									name.Clear();
								}
								break;
							// Окончание параметров generic-типа
							case '>':
								{
									if (types.Count > 0)
									{
										// Добавление параметра родительскому типу

										var parentType = types.Pop();
										var genericTypes = parentType.Item2;

										if (name.Length > 0)
										{
											var typeName = name.ToString();
											genericTypes.Add(typeName);
											name.Clear();
										}

										// Формирование полного имени родительского типа

										name.Append(parentType.Item1);

										if (genericTypes.Count > 0)
										{
											name.Append('`').Append(genericTypes.Count).Append('[');

											foreach (var t in genericTypes)
											{
												var gt = ResolveBuiltInType(t);
												name.Append(gt).Append(',');
											}

											name.Remove(name.Length - 1, 1);
											name.Append(']');
										}

										// Добавление параметра родительскому типу

										if (types.Count > 0)
										{
											parentType = types.Peek();
											genericTypes = parentType.Item2;

											var typeName = name.ToString();
											genericTypes.Add(typeName);
											name.Clear();
										}
									}
								}
								break;
							// Пропуск незначимых символов
							case ' ':
							case '\t':
							case '\r':
							case '\n':
								break;
							// Формирование имени типа
							default:
								{
									name.Append(c);
								}
								break;
						}
					}

					result = name.ToString();
				}
			}

			return result;
		}

		private static string ResolveBuiltInType(string value)
		{
			Type type;

			return BuiltInTypes.TryGetValue(value, out type) ? type.FullName : value;
		}

		private static readonly Dictionary<string, Type> BuiltInTypes
			= new Dictionary<string, Type>
			  {
				  { "bool", typeof(bool) },
				  { "byte", typeof(byte) },
				  { "sbyte", typeof(sbyte) },
				  { "char", typeof(char) },
				  { "decimal", typeof(decimal) },
				  { "double", typeof(double) },
				  { "float", typeof(float) },
				  { "int", typeof(int) },
				  { "uint", typeof(uint) },
				  { "long", typeof(long) },
				  { "ulong", typeof(ulong) },
				  { "object", typeof(object) },
				  { "short", typeof(short) },
				  { "ushort", typeof(ushort) },
				  { "string", typeof(string) },
				  { "Math", typeof(MathFunctions) },
				  { "Text", typeof(TextFunctions) },
				  { "DateTime", typeof(DateTimeFunctions) },
				  { "TimeSpan", typeof(TimeSpanFunctions) },
				  { "Condition", typeof(ConditionFunctions) },
				  { "Enumerable", typeof(EnumerableFunctions) }
			  };
	}
}
using System.Collections.Generic;
using System.Text;

using InfinniPlatform.Cassandra.Storage;

namespace InfinniPlatform.Cassandra.Client
{
	/// <summary>
	/// Provides a set of static methods for building SQL-queries.
	/// </summary>
	static class CassandraCqlStringBuilderExtensions
	{
		public static StringBuilder AppendTableName(this StringBuilder builder, string keyspace, string table)
		{
			builder.Append(' ');

			if (string.IsNullOrWhiteSpace(keyspace) == false)
			{
				builder.Append(keyspace).Append('.');
			}

			return builder.Append(table);
		}

		public static StringBuilder AppendInsertColumns(this StringBuilder builder, string[] columns)
		{
			builder.Append(" (");

			if (columns.IsNotNullOrEmpty())
			{
				foreach (var column in columns)
				{
					builder.Append(column).Append(',');
				}

				builder.Remove(builder.Length - 1, 1);
			}

			return builder.Append(')');
		}

		public static StringBuilder AppendUpdateColumns(this StringBuilder builder, string[] columns)
		{
			builder.Append(" SET ");

			if (columns.IsNotNullOrEmpty())
			{
				foreach (var column in columns)
				{
					builder.Append(column).Append("=?,");
				}

				builder.Remove(builder.Length - 1, 1);
			}

			return builder;
		}

		public static StringBuilder AppendDeleteColumns(this StringBuilder builder, string[] columns)
		{
			if (columns.IsNotNullOrEmpty())
			{
				builder.Append(' ');

				foreach (var column in columns)
				{
					builder.Append(column).Append(',');
				}

				builder.Remove(builder.Length - 1, 1);
			}

			return builder;
		}

		public static StringBuilder AppendSelectColumns(this StringBuilder builder, string[] columns)
		{
			builder.Append(' ');

			if (columns.IsNotNullOrEmpty())
			{
				foreach (var column in columns)
				{
					builder.Append(column).Append(',');
				}

				builder.Remove(builder.Length - 1, 1);
			}
			else
			{
				builder.Append('*');
			}

			return builder;
		}

		public static StringBuilder AppendDeclarationColumns(this StringBuilder builder, Column[] columns)
		{
			builder.Append(" (");

			if (columns.IsNotNullOrEmpty())
			{
				foreach (var column in columns)
				{
					builder.Append(column.Name).Append(' ').Append(ColumnTypes[column.Type]).Append(',');
				}

				builder.Append("PRIMARY KEY (");

				foreach (var column in columns)
				{
					if (column.Key)
					{
						builder.Append(column.Name).Append(',');
					}
				}

				builder.Remove(builder.Length - 1, 1);

				builder.Append(')');
			}

			return builder.Append(')');
		}

		public static StringBuilder AppendInsertValues(this StringBuilder builder, string[] columns)
		{
			builder.Append(" VALUES (");

			if (columns.IsNotNullOrEmpty())
			{
				for (var i = 0; i < columns.Length; ++i)
				{
					builder.Append("?,");
				}

				builder.Remove(builder.Length - 1, 1);
			}

			return builder.Append(')');
		}

		public static StringBuilder AppendWhere(this StringBuilder builder, KeyFilter[] filters)
		{
			if (filters.IsNotNullOrEmpty())
			{
				builder.Append(" WHERE ");

				foreach (var filter in filters)
				{
					builder.Append(filter.Key).Append(KeyFilterOperators[filter.Operator]).Append("? AND ");
				}

				builder.Remove(builder.Length - 5, 5);
			}

			return builder;
		}

		public static StringBuilder AppendOrder(this StringBuilder builder, KeySort[] sorts)
		{
			if (sorts.IsNotNullOrEmpty())
			{
				builder.Append(" ORDER BY ");

				foreach (var sort in sorts)
				{
					builder.Append(sort.Key).Append(' ').Append(KeySortDirections[sort.Direction]).Append(',');
				}

				builder.Remove(builder.Length - 1, 1);
			}

			return builder;
		}

		public static StringBuilder AppendLimit(this StringBuilder builder, int limit)
		{
			if (limit > 0)
			{
				builder.Append(" LIMIT ").Append(limit);
			}

			return builder;
		}


		public static bool IsNotNullOrEmpty<T>(this T[] array)
		{
			return (array != null && array.Length > 0);
		}


		private static readonly Dictionary<ColumnType, string> ColumnTypes
			= new Dictionary<ColumnType, string>
				  {
					  { ColumnType.Bool, "boolean" },
					  { ColumnType.Int32, "int" },
					  { ColumnType.Int64, "bigint" },
					  { ColumnType.Float, "float" },
					  { ColumnType.Double, "double" },
					  { ColumnType.Decimal, "float" },
					  { ColumnType.Uuid, "uuid" },
					  { ColumnType.DateTime, "timestamp" },
					  { ColumnType.String, "text" },
					  { ColumnType.Blob, "blob" },
				  };

		private static readonly Dictionary<KeyFilterOperator, string> KeyFilterOperators
			= new Dictionary<KeyFilterOperator, string>
				  {
					  { KeyFilterOperator.Equal, "=" },
					  { KeyFilterOperator.Less, "<" },
					  { KeyFilterOperator.LessOrEqual, "<=" },
					  { KeyFilterOperator.Greater, ">" },
					  { KeyFilterOperator.GreaterOrEqual, ">=" }
				  };

		private static readonly Dictionary<KeySortDirection, string> KeySortDirections
			= new Dictionary<KeySortDirection, string>
				  {
					  { KeySortDirection.Asc, "ASC" },
					  { KeySortDirection.Desc, "DESC" }
				  };
	}
}
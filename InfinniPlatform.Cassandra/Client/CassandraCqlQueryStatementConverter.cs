using System;
using System.Collections.Generic;
using System.Text;

using InfinniPlatform.Cassandra.DataAdapter;
using InfinniPlatform.Cassandra.Repository;
using InfinniPlatform.Cassandra.Storage;

namespace InfinniPlatform.Cassandra.Client
{
	/// <summary>
	/// Represents CQL-converter.
	/// </summary>
	sealed class CassandraCqlQueryStatementConverter
	{
		public CassandraCqlQueryStatementConverter()
		{
			RegisterConverter<InsertQueryStatement>(ToCql);
			RegisterConverter<UpdateQueryStatement>(ToCql);
			RegisterConverter<DeleteQueryStatement>(ToCql);

			RegisterConverter<CountQueryStatement>(ToCql);
			RegisterConverter<SelectQueryStatement>(ToCql);

			RegisterConverter<CreateKeyspaceQueryStatement>(ToCql);
			RegisterConverter<DropKeyspaceQueryStatement>(ToCql);

			RegisterConverter<CreateTableQueryStatement>(ToCql);
			RegisterConverter<DropTableQueryStatement>(ToCql);
		}


		/// <summary>
		/// Converts given statement to a CQL-query.
		/// </summary>
		/// <param name="statement">The query statement.</param>
		/// <returns>A CQL-query.</returns>
		public string Convert(IQueryStatement statement)
		{
			Func<IQueryStatement, string> converter;

			if (_converters.TryGetValue(statement.GetType(), out converter))
			{
				return converter(statement);
			}

			throw new NotSupportedException();
		}


		private readonly Dictionary<Type, Func<IQueryStatement, string>> _converters
			= new Dictionary<Type, Func<IQueryStatement, string>>();

		/// <summary>
		/// Registers a function that converts statement of the specified type to a CQL-query.
		/// </summary>
		/// <typeparam name="TStatement">The type of a query statement.</typeparam>
		/// <param name="converter">A function that converts given statement to a CQL-query.</param>
		public void RegisterConverter<TStatement>(Func<TStatement, string> converter) where TStatement : IQueryStatement
		{
			_converters[typeof(TStatement)] = statement => converter((TStatement)statement);
		}


		private static string ToCql(InsertQueryStatement statement)
		{
			return new StringBuilder("INSERT INTO")
				.AppendTableName(statement.Keyspace, statement.Table)
				.AppendInsertColumns(statement.Columns)
				.AppendInsertValues(statement.Columns)
				.Append(';').ToString();
		}

		private static string ToCql(UpdateQueryStatement statement)
		{
			return new StringBuilder("UPDATE")
				.AppendTableName(statement.Keyspace, statement.Table)
				.AppendUpdateColumns(statement.Columns)
				.AppendWhere(statement.Where)
				.Append(';').ToString();
		}

		private static string ToCql(DeleteQueryStatement statement)
		{
			return statement.Where.IsNotNullOrEmpty()
					   ? new StringBuilder("DELETE")
							 .AppendDeleteColumns(statement.Columns).Append(" FROM")
							 .AppendTableName(statement.Keyspace, statement.Table)
							 .AppendWhere(statement.Where)
							 .Append(';').ToString()
					   : new StringBuilder("TRUNCATE")
							 .AppendTableName(statement.Keyspace, statement.Table)
							 .Append(';').ToString();
		}

		private static string ToCql(CountQueryStatement statement)
		{
			return new StringBuilder("SELECT COUNT(*) FROM")
				.AppendTableName(statement.Keyspace, statement.Table)
				.AppendWhere(statement.Where)
				.AppendLimit(statement.Limit)
				.Append(';').ToString();
		}

		private static string ToCql(SelectQueryStatement statement)
		{
			return new StringBuilder("SELECT")
				.AppendSelectColumns(statement.Columns).Append(" FROM")
				.AppendTableName(statement.Keyspace, statement.Table)
				.AppendWhere(statement.Where)
				.AppendLimit(statement.Limit)
				.AppendOrder(statement.Order)
				.Append(';').ToString();
		}

		private static string ToCql(CreateKeyspaceQueryStatement statement)
		{
			return new StringBuilder("CREATE KEYSPACE ").Append(statement.Keyspace)
				.Append(" WITH replication = {'class': 'SimpleStrategy', 'replication_factor' : 3};")
				.ToString();
		}

		private static string ToCql(DropKeyspaceQueryStatement statement)
		{
			return new StringBuilder("DROP KEYSPACE ").Append(statement.Keyspace)
				.Append(';').ToString();
		}

		private static string ToCql(CreateTableQueryStatement statement)
		{
			return new StringBuilder("CREATE TABLE")
				.AppendTableName(statement.Keyspace, statement.Table)
				.AppendDeclarationColumns(statement.Columns)
				.Append(';').ToString();
		}

		private static string ToCql(DropTableQueryStatement statement)
		{
			return new StringBuilder("DROP TABLE")
				.AppendTableName(statement.Keyspace, statement.Table)
				.Append(';').ToString();
		}
	}
}
using InfinniPlatform.Cassandra.Client;
using InfinniPlatform.Cassandra.Storage;

using NUnit.Framework;

namespace InfinniPlatform.Cassandra.Tests.Client
{
	[TestFixture]
	[Category(TestCategories.IntegrationTest)]
	public sealed class CassandraStorageIntegrationTest
	{
		private ISession _session;
		private const string KeyspaceName = "TestStorage";


		[SetUp]
		public void SetUp()
		{
			var sessionFactory = new CassandraSessionFactory();

			_session = sessionFactory.OpenSession();
		}


		[Test]
		public void ShouldCreateDeleteKeyspace()
		{
			// When

			TestDelegate createDeleteKeyspace
				= () =>
					  {
						  _session.Execute(new CreateKeyspaceQueryStatement
											   {
												   Keyspace = KeyspaceName
											   });

						  _session.Execute(new DropKeyspaceQueryStatement
											   {
												   Keyspace = KeyspaceName
											   });
					  };

			// Then

			Assert.DoesNotThrow(createDeleteKeyspace);
		}

		[Test]
		public void ShouldCreateDeleteTable()
		{
			// When

			TestDelegate createDeleteTable
				= () =>
					  {
						  _session.Execute(new CreateKeyspaceQueryStatement
											   {
												   Keyspace = KeyspaceName
											   });

						  _session.Execute(new CreateTableQueryStatement
											   {
												   Keyspace = KeyspaceName,
												   Table = "CreateDeleteTable",
												   Columns = new[]
									                             {
										                             new Column("uuidkey", ColumnType.Uuid, true),

										                             new Column("boolcolumn", ColumnType.Bool),
										                             new Column("int32column", ColumnType.Int32),
										                             new Column("int64column", ColumnType.Int64),
										                             new Column("floatcolumn", ColumnType.Float),
										                             new Column("doublecolumn", ColumnType.Double),
										                             new Column("decimalcolumn", ColumnType.Decimal),
										                             new Column("uuidcolumn", ColumnType.Uuid),
										                             new Column("datetimecolumn", ColumnType.DateTime),
										                             new Column("stringcolumn", ColumnType.String),
										                             new Column("blobcolumn", ColumnType.Blob)
									                             }
											   });

						  _session.Execute(new DropTableQueryStatement
											   {
												   Keyspace = KeyspaceName,
												   Table = "CreateDeleteTable"
											   });

						  _session.Execute(new DropKeyspaceQueryStatement
											   {
												   Keyspace = KeyspaceName
											   });
					  };


			// Then

			Assert.DoesNotThrow(createDeleteTable);
		}

		[Test]
		public void ShouldCreateDeleteCompositeKeyTable()
		{
			// When

			TestDelegate createDeleteCompositeKeyTable
				= () =>
					  {
						  _session.Execute(new CreateKeyspaceQueryStatement
											   {
												   Keyspace = KeyspaceName
											   });

						  _session.Execute(new CreateTableQueryStatement
											   {
												   Keyspace = KeyspaceName,
												   Table = "CreateDeleteCompositeKeyTable",
												   Columns = new[]
									                             {
										                             new Column("boolkey", ColumnType.Bool, true),
										                             new Column("int32key", ColumnType.Int32, true),
										                             new Column("int64key", ColumnType.Int64, true),
										                             new Column("floatkey", ColumnType.Float, true),
										                             new Column("doublekey", ColumnType.Double, true),
										                             new Column("decimalkey", ColumnType.Decimal, true),
										                             new Column("uuidkey", ColumnType.Uuid, true),
										                             new Column("datetimekey", ColumnType.DateTime, true),
										                             new Column("stringkey", ColumnType.String, true),
										                             new Column("blobkey", ColumnType.Blob, true),

										                             new Column("boolcolumn", ColumnType.Bool),
										                             new Column("int32column", ColumnType.Int32),
										                             new Column("int64column", ColumnType.Int64),
										                             new Column("floatcolumn", ColumnType.Float),
										                             new Column("doublecolumn", ColumnType.Double),
										                             new Column("decimalcolumn", ColumnType.Decimal),
										                             new Column("uuidcolumn", ColumnType.Uuid),
										                             new Column("datetimecolumn", ColumnType.DateTime),
										                             new Column("stringcolumn", ColumnType.String),
										                             new Column("blobcolumn", ColumnType.Blob)
									                             }
											   });

						  _session.Execute(new DropTableQueryStatement
											   {
												   Keyspace = KeyspaceName,
												   Table = "CreateDeleteCompositeKeyTable"
											   });

						  _session.Execute(new DropKeyspaceQueryStatement
											   {
												   Keyspace = KeyspaceName
											   });
					  };


			// Then

			Assert.DoesNotThrow(createDeleteCompositeKeyTable);
		}
	}
}
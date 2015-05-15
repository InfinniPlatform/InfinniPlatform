using System;
using System.Linq;

using InfinniPlatform.Cassandra.Client;
using InfinniPlatform.Cassandra.DataAdapter;
using InfinniPlatform.Cassandra.Repository;
using InfinniPlatform.Cassandra.Storage;

using NUnit.Framework;

namespace InfinniPlatform.Cassandra.Tests.Client
{
	[TestFixture]
	[Category(TestCategories.IntegrationTest)]
	public sealed class CassandraDataManipulationIntegrationTest
	{
		private ISession _session;
		private const string KeyspaceName = "TestDataManipulation";
		private const string SimpleTableName = "SimpleTable";
		private const string CompositeTableName = "CompositeTable";


		[SetUp]
		public void SetUp()
		{
			var sessionFactory = new CassandraSessionFactory();

			_session = sessionFactory.OpenSession();

			_session.Execute(new CreateKeyspaceQueryStatement { Keyspace = KeyspaceName });
			_session.Execute(new CreateTableQueryStatement { Keyspace = KeyspaceName, Table = SimpleTableName, Columns = new[] { new Column("key", ColumnType.Uuid, true), new Column("value", ColumnType.String), new Column("value1", ColumnType.String) } });
			_session.Execute(new CreateTableQueryStatement { Keyspace = KeyspaceName, Table = CompositeTableName, Columns = new[] { new Column("key1", ColumnType.Uuid, true), new Column("key2", ColumnType.Int32, true), new Column("value", ColumnType.String), new Column("value1", ColumnType.String) } });
		}

		[TearDown]
		public void TearDown()
		{
			_session.Execute(new DropKeyspaceQueryStatement { Keyspace = KeyspaceName });
		}


		[Test]
		public void ShouldCreateReadUpdateDeleteByKey()
		{
			// Given

			var key = Guid.NewGuid();
			var value1 = Guid.NewGuid().ToString();
			var value2 = Guid.NewGuid().ToString();

			var insertStatement = new InsertQueryStatement { Keyspace = KeyspaceName, Table = SimpleTableName, Columns = new[] { "key", "value" } };
			var updateStatement = new UpdateQueryStatement { Keyspace = KeyspaceName, Table = SimpleTableName, Columns = new[] { "value" }, Where = new[] { new KeyFilter("key") } };
			var deleteStatement = new DeleteQueryStatement { Keyspace = KeyspaceName, Table = SimpleTableName, Where = new[] { new KeyFilter("key") } };
			var selectSattement = new SelectQueryStatement { Keyspace = KeyspaceName, Table = SimpleTableName, Where = new[] { new KeyFilter("key") } };

			// When

			_session.Execute(insertStatement, new object[] { key, value1 });
			var afterInsert = _session.Execute(selectSattement, new object[] { key });

			_session.Execute(updateStatement, new object[] { value2, key });
			var afterUpdate = _session.Execute(selectSattement, new object[] { key });

			_session.Execute(deleteStatement, new object[] { key });
			var afterDelete = _session.Execute(selectSattement, new object[] { key });

			// Then

			Assert.IsNotNull(afterInsert);
			var afterInsertRows = afterInsert.ToArray();
			Assert.AreEqual(1, afterInsertRows.Length);
			Assert.AreEqual(key, afterInsertRows[0].GetValue("key"));
			Assert.AreEqual(value1, afterInsertRows[0].GetValue("value"));

			Assert.IsNotNull(afterUpdate);
			var afterUpdateRows = afterUpdate.ToArray();
			Assert.AreEqual(1, afterUpdateRows.Length);
			Assert.AreEqual(key, afterUpdateRows[0].GetValue("key"));
			Assert.AreEqual(value2, afterUpdateRows[0].GetValue("value"));

			Assert.IsTrue(afterDelete == null || afterDelete.Any() == false);
		}

		[Test]
		public void ShouldCreateReadUpdateDeleteByCompositeKey()
		{
			// Given

			var key1 = Guid.NewGuid();
			var key2 = new Random().Next();
			var value1 = Guid.NewGuid().ToString();
			var value2 = Guid.NewGuid().ToString();

			var insertStatement = new InsertQueryStatement { Keyspace = KeyspaceName, Table = CompositeTableName, Columns = new[] { "key1", "key2", "value" } };
			var updateStatement = new UpdateQueryStatement { Keyspace = KeyspaceName, Table = CompositeTableName, Columns = new[] { "value" }, Where = new[] { new KeyFilter("key1"), new KeyFilter("key2") } };
			var deleteStatement = new DeleteQueryStatement { Keyspace = KeyspaceName, Table = CompositeTableName, Where = new[] { new KeyFilter("key1"), new KeyFilter("key2") } };
			var selectSattement = new SelectQueryStatement { Keyspace = KeyspaceName, Table = CompositeTableName, Where = new[] { new KeyFilter("key1"), new KeyFilter("key2") } };

			// When

			_session.Execute(insertStatement, new object[] { key1, key2, value1 });
			var afterInsert = _session.Execute(selectSattement, new object[] { key1, key2 });

			_session.Execute(updateStatement, new object[] { value2, key1, key2 });
			var afterUpdate = _session.Execute(selectSattement, new object[] { key1, key2 });

			_session.Execute(deleteStatement, new object[] { key1, key2 });
			var afterDelete = _session.Execute(selectSattement, new object[] { key1, key2 });

			// Then

			Assert.IsNotNull(afterInsert);
			var afterInsertRows = afterInsert.ToArray();
			Assert.AreEqual(1, afterInsertRows.Length);
			Assert.AreEqual(key1, afterInsertRows[0].GetValue("key1"));
			Assert.AreEqual(key2, afterInsertRows[0].GetValue("key2"));
			Assert.AreEqual(value1, afterInsertRows[0].GetValue("value"));

			Assert.IsNotNull(afterUpdate);
			var afterUpdateRows = afterUpdate.ToArray();
			Assert.AreEqual(1, afterUpdateRows.Length);
			Assert.AreEqual(key1, afterUpdateRows[0].GetValue("key1"));
			Assert.AreEqual(key2, afterUpdateRows[0].GetValue("key2"));
			Assert.AreEqual(value2, afterUpdateRows[0].GetValue("value"));

			Assert.IsTrue(afterDelete == null || afterDelete.Any() == false);
		}

		[Test]
		public void InsertByExistingKeyShouldWorksAsUpdate()
		{
			// Given

			var key = Guid.NewGuid();
			var value1 = Guid.NewGuid().ToString();
			var value2 = Guid.NewGuid().ToString();

			var insertStatement = new InsertQueryStatement { Keyspace = KeyspaceName, Table = SimpleTableName, Columns = new[] { "key", "value" } };
			var selectSattement = new SelectQueryStatement { Keyspace = KeyspaceName, Table = SimpleTableName, Where = new[] { new KeyFilter("key") } };

			// When

			_session.Execute(insertStatement, new object[] { key, value1 });
			_session.Execute(insertStatement, new object[] { key, value2 });
			var afterInsert = _session.Execute(selectSattement, new object[] { key });

			// Then

			Assert.IsNotNull(afterInsert);
			var afterInsertRows = afterInsert.ToArray();
			Assert.AreEqual(1, afterInsertRows.Length);
			Assert.AreEqual(key, afterInsertRows[0].GetValue("key"));
			Assert.AreEqual(value2, afterInsertRows[0].GetValue("value"));
		}

		[Test]
		public void InsertByExistingKeyShouldAppendColumns()
		{
			// Given

			var key = Guid.NewGuid();
			var value = Guid.NewGuid().ToString();
			var value1 = Guid.NewGuid().ToString();

			var insertStatement1 = new InsertQueryStatement { Keyspace = KeyspaceName, Table = SimpleTableName, Columns = new[] { "key", "value" } };
			var insertStatement2 = new InsertQueryStatement { Keyspace = KeyspaceName, Table = SimpleTableName, Columns = new[] { "key", "value1" } };
			var selectSattement = new SelectQueryStatement { Keyspace = KeyspaceName, Table = SimpleTableName, Where = new[] { new KeyFilter("key") } };

			// When

			_session.Execute(insertStatement1, new object[] { key, value });
			var afterInsert1 = _session.Execute(selectSattement, new object[] { key });

			_session.Execute(insertStatement2, new object[] { key, value1 });
			var afterInsert2 = _session.Execute(selectSattement, new object[] { key });

			// Then

			Assert.IsNotNull(afterInsert1);
			var afterInsert1Rows = afterInsert1.ToArray();
			Assert.AreEqual(1, afterInsert1Rows.Length);
			Assert.AreEqual(key, afterInsert1Rows[0].GetValue("key"));
			Assert.AreEqual(value, afterInsert1Rows[0].GetValue("value"));
			Assert.IsNull(afterInsert1Rows[0].GetValue("value1"));

			Assert.IsNotNull(afterInsert2);
			var afterInsert2Rows = afterInsert2.ToArray();
			Assert.AreEqual(1, afterInsert2Rows.Length);
			Assert.AreEqual(key, afterInsert2Rows[0].GetValue("key"));
			Assert.AreEqual(value, afterInsert2Rows[0].GetValue("value"));
			Assert.AreEqual(value1, afterInsert2Rows[0].GetValue("value1"));
		}

		[Test]
		public void UpdateByNonexistingKeyShouldWorksAsInsert()
		{
			// Given

			var key = Guid.NewGuid();
			var value = Guid.NewGuid().ToString();

			var updateStatement = new UpdateQueryStatement { Keyspace = KeyspaceName, Table = SimpleTableName, Columns = new[] { "value" }, Where = new[] { new KeyFilter("key") } };
			var selectSattement = new SelectQueryStatement { Keyspace = KeyspaceName, Table = SimpleTableName, Where = new[] { new KeyFilter("key") } };

			// When

			_session.Execute(updateStatement, new object[] { value, key });
			var afterUpdate = _session.Execute(selectSattement, new object[] { key });

			// Then

			Assert.IsNotNull(afterUpdate);
			var afterUpdateRows = afterUpdate.ToArray();
			Assert.AreEqual(1, afterUpdateRows.Length);
			Assert.AreEqual(key, afterUpdateRows[0].GetValue("key"));
			Assert.AreEqual(value, afterUpdateRows[0].GetValue("value"));
		}

		[Test]
		public void UpdateByExistingKeyShouldAppendColumns()
		{
			// Given

			var key = Guid.NewGuid();
			var value = Guid.NewGuid().ToString();
			var value1 = Guid.NewGuid().ToString();

			var insertStatement = new InsertQueryStatement { Keyspace = KeyspaceName, Table = SimpleTableName, Columns = new[] { "key", "value" } };
			var updateStatement = new UpdateQueryStatement { Keyspace = KeyspaceName, Table = SimpleTableName, Columns = new[] { "value1" }, Where = new[] { new KeyFilter("key") } };
			var selectSattement = new SelectQueryStatement { Keyspace = KeyspaceName, Table = SimpleTableName, Where = new[] { new KeyFilter("key") } };

			// When

			_session.Execute(insertStatement, new object[] { key, value });
			_session.Execute(updateStatement, new object[] { value1, key });
			var afterUpdate = _session.Execute(selectSattement, new object[] { key });

			// Then

			Assert.IsNotNull(afterUpdate);
			var afterUpdateRows = afterUpdate.ToArray();
			Assert.AreEqual(1, afterUpdateRows.Length);
			Assert.AreEqual(key, afterUpdateRows[0].GetValue("key"));
			Assert.AreEqual(value, afterUpdateRows[0].GetValue("value"));
			Assert.AreEqual(value1, afterUpdateRows[0].GetValue("value1"));
		}

		[Test]
		public void ShouldDeleteSpecificColumns()
		{
			// Given

			var key = Guid.NewGuid();
			var value = Guid.NewGuid().ToString();
			var value1 = Guid.NewGuid().ToString();

			var insertStatement = new InsertQueryStatement { Keyspace = KeyspaceName, Table = SimpleTableName, Columns = new[] { "key", "value", "value1" } };
			var deleteStatement = new DeleteQueryStatement { Keyspace = KeyspaceName, Table = SimpleTableName, Columns = new[] { "value1" }, Where = new[] { new KeyFilter("key") } };
			var selectSattement = new SelectQueryStatement { Keyspace = KeyspaceName, Table = SimpleTableName, Where = new[] { new KeyFilter("key") } };

			// When

			_session.Execute(insertStatement, new object[] { key, value, value1 });
			_session.Execute(deleteStatement, new object[] { key });
			var afterDelete = _session.Execute(selectSattement, new object[] { key });

			// Then

			Assert.IsNotNull(afterDelete);
			var afterInsertRows = afterDelete.ToArray();
			Assert.AreEqual(1, afterInsertRows.Length);
			Assert.AreEqual(key, afterInsertRows[0].GetValue("key"));
			Assert.AreEqual(value, afterInsertRows[0].GetValue("value"));
			Assert.IsNull(afterInsertRows[0].GetValue("value1"));
		}

		[Test]
		public void ShouldTruncateTable()
		{
			// Given

			var insertStatement = new InsertQueryStatement { Keyspace = KeyspaceName, Table = SimpleTableName, Columns = new[] { "key", "value" } };
			var deleteStatement = new DeleteQueryStatement { Keyspace = KeyspaceName, Table = SimpleTableName };
			var selectSattement = new SelectQueryStatement { Keyspace = KeyspaceName, Table = SimpleTableName };

			// When

			_session.Execute(insertStatement, new object[] { Guid.NewGuid(), Guid.NewGuid().ToString() });
			_session.Execute(insertStatement, new object[] { Guid.NewGuid(), Guid.NewGuid().ToString() });
			_session.Execute(insertStatement, new object[] { Guid.NewGuid(), Guid.NewGuid().ToString() });
			_session.Execute(deleteStatement);

			var afterDelete = _session.Execute(selectSattement);

			// Then

			Assert.IsTrue(afterDelete == null || afterDelete.Any() == false);
		}

		[Test]
		public void ShouldSelectSpecificColumns()
		{
			// Given

			var insertStatement = new InsertQueryStatement { Keyspace = KeyspaceName, Table = SimpleTableName, Columns = new[] { "key", "value", "value1" } };
			var selectSattement = new SelectQueryStatement { Keyspace = KeyspaceName, Table = SimpleTableName, Columns = new[] { "key", "value" } };

			// When

			_session.Execute(insertStatement, new object[] { Guid.NewGuid(), Guid.NewGuid().ToString(), Guid.NewGuid().ToString() });
			var rows = _session.Execute(selectSattement);

			// Then

			Assert.IsNotNull(rows);
			var arrayRows = rows.ToArray();
			Assert.AreEqual(1, arrayRows.Length);
			Assert.AreEqual(2, arrayRows[0].Count());
			Assert.IsNotNull(arrayRows[0].GetValue("key"));
			Assert.IsNotNull(arrayRows[0].GetValue("value"));
		}

		[Test]
		public void ShouldOrderSelectedRows()
		{
			// Given

			var insertStatement = new InsertQueryStatement { Keyspace = KeyspaceName, Table = CompositeTableName, Columns = new[] { "key1", "key2", "value" } };
			var selectSattement = new SelectQueryStatement { Keyspace = KeyspaceName, Table = CompositeTableName, Order = new[] { new KeySort("key2", KeySortDirection.Desc) }, Where = new[] { new KeyFilter("key1") } };

			// When

			var key1 = Guid.NewGuid();

			_session.Execute(insertStatement, new object[] { key1, 1, "Row1" });
			_session.Execute(insertStatement, new object[] { key1, 2, "Row2" });
			_session.Execute(insertStatement, new object[] { key1, 3, "Row3" });
			var rows = _session.Execute(selectSattement, new object[] { key1 });

			// Then

			Assert.IsNotNull(rows);
			var arrayRows = rows.ToArray();
			Assert.AreEqual(3, arrayRows.Length);

			Assert.AreEqual(key1, arrayRows[2].GetValue("key1"));
			Assert.AreEqual(1, arrayRows[2].GetValue("key2"));
			Assert.AreEqual("Row1", arrayRows[2].GetValue("value"));

			Assert.AreEqual(key1, arrayRows[1].GetValue("key1"));
			Assert.AreEqual(2, arrayRows[1].GetValue("key2"));
			Assert.AreEqual("Row2", arrayRows[1].GetValue("value"));

			Assert.AreEqual(key1, arrayRows[0].GetValue("key1"));
			Assert.AreEqual(3, arrayRows[0].GetValue("key2"));
			Assert.AreEqual("Row3", arrayRows[0].GetValue("value"));
		}

		[Test]
		public void ShouldLimitSelectionResult()
		{
			// Given

			var insertStatement = new InsertQueryStatement { Keyspace = KeyspaceName, Table = SimpleTableName, Columns = new[] { "key", "value" } };
			var selectSattement = new SelectQueryStatement { Keyspace = KeyspaceName, Table = SimpleTableName, Limit = 2 };

			// When

			_session.Execute(insertStatement, new object[] { Guid.NewGuid(), Guid.NewGuid().ToString() });
			_session.Execute(insertStatement, new object[] { Guid.NewGuid(), Guid.NewGuid().ToString() });
			_session.Execute(insertStatement, new object[] { Guid.NewGuid(), Guid.NewGuid().ToString() });
			var rows = _session.Execute(selectSattement);

			// Then

			Assert.IsNotNull(rows);
			var arrayRows = rows.ToArray();
			Assert.AreEqual(2, arrayRows.Length);
		}

		[Test]
		public void ShouldCountTableRows()
		{
			// Given

			var insertStatement = new InsertQueryStatement { Keyspace = KeyspaceName, Table = SimpleTableName, Columns = new[] { "key", "value" } };
			var selectSattement = new CountQueryStatement { Keyspace = KeyspaceName, Table = SimpleTableName };

			// When

			_session.Execute(insertStatement, new object[] { Guid.NewGuid(), Guid.NewGuid().ToString() });
			_session.Execute(insertStatement, new object[] { Guid.NewGuid(), Guid.NewGuid().ToString() });
			_session.Execute(insertStatement, new object[] { Guid.NewGuid(), Guid.NewGuid().ToString() });
			var rows = _session.Execute(selectSattement);

			// Then

			Assert.IsNotNull(rows);
			var arrayRows = rows.ToArray();
			Assert.AreEqual(1, arrayRows.Length);
			Assert.AreEqual(1, arrayRows[0].Count());
			Assert.AreEqual(3, arrayRows[0].GetValue(0));
		}

		[Test]
		public void ShouldCountTableRowsByCondition()
		{
			// Given

			var insertStatement = new InsertQueryStatement { Keyspace = KeyspaceName, Table = CompositeTableName, Columns = new[] { "key1", "key2", "value" } };
			var selectSattement = new CountQueryStatement { Keyspace = KeyspaceName, Table = CompositeTableName, Where = new[] { new KeyFilter("key1") } };

			// When

			var key1 = Guid.NewGuid();
			var key2 = Guid.NewGuid();

			_session.Execute(insertStatement, new object[] { key1, 1, "Row11" });
			_session.Execute(insertStatement, new object[] { key1, 2, "Row12" });
			_session.Execute(insertStatement, new object[] { key1, 3, "Row13" });
			_session.Execute(insertStatement, new object[] { key2, 1, "Row21" });
			_session.Execute(insertStatement, new object[] { key2, 2, "Row22" });

			var rows = _session.Execute(selectSattement, new object[] { key1 });

			// Then

			Assert.IsNotNull(rows);
			var arrayRows = rows.ToArray();
			Assert.AreEqual(1, arrayRows.Length);
			Assert.AreEqual(1, arrayRows[0].Count());
			Assert.AreEqual(3, arrayRows[0].GetValue(0));
		}

		[Test]
		public void ShouldSupportAllColumnTypes()
		{
			// Given

			var random = new Random(DateTime.Now.Millisecond);

			_session.Execute(new CreateTableQueryStatement
								 {
									 Keyspace = KeyspaceName,
									 Table = "ComplexTable",
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

			var parameters = new object[]
				                 {
					                 /* "boolkey" */ true,
					                 /* "int32key" */ random.Next(),
					                 /* "int64key" */ 2L * int.MaxValue,
					                 /* "floatkey" */ (float)random.NextDouble(),
					                 /* "doublekey" */ random.NextDouble(),
					                 /* "decimalkey" */ (float)random.NextDouble(),
					                 /* "uuidkey" */ Guid.NewGuid(),
					                 /* "datetimekey" */ (DateTimeOffset)DateTime.Today.AddSeconds(random.Next(24 * 3600)).ToUniversalTime(),
					                 /* "stringkey" */ Guid.NewGuid().ToString(),
					                 /* "blobkey" */ new[] { (byte) random.Next(0, 255), (byte) random.Next(0, 255), (byte) random.Next(0, 255) },

									 /* "boolcolumn" */ false,
					                 /* "int32column" */ random.Next(),
					                 /* "int64column" */ 3L * int.MaxValue,
					                 /* "floatcolumn" */ (float)random.NextDouble(),
					                 /* "doublecolumn" */ random.NextDouble(),
					                 /* "decimalcolumn" */ (float)random.NextDouble(),
					                 /* "uuidcolumn" */ Guid.NewGuid(),
					                 /* "datetimecolumn" */ (DateTimeOffset)DateTime.Today.AddSeconds(random.Next(24 * 3600)),
					                 /* "stringcolumn" */ Guid.NewGuid().ToString(),
					                 /* "blobcolumn" */ new[] { (byte) random.Next(0, 255), (byte) random.Next(0, 255), (byte) random.Next(0, 255) }
				                 };

			// When

			_session.Execute(new InsertQueryStatement
								 {
									 Keyspace = KeyspaceName,
									 Table = "ComplexTable",
									 Columns = new[]
						                           {
							                           "boolkey",
							                           "int32key",
							                           "int64key",
							                           "floatkey",
							                           "doublekey",
							                           "decimalkey",
							                           "uuidkey",
							                           "datetimekey",
							                           "stringkey",
							                           "blobkey",

							                           "boolcolumn",
							                           "int32column",
							                           "int64column",
							                           "floatcolumn",
							                           "doublecolumn",
							                           "decimalcolumn",
							                           "uuidcolumn",
							                           "datetimecolumn",
							                           "stringcolumn",
							                           "blobcolumn"
						                           },
								 }, parameters);

			var rows = _session.Execute(new SelectQueryStatement
											{
												Keyspace = KeyspaceName,
												Table = "ComplexTable",
												Columns = new[]
						                                      {
							                                      "boolkey",
							                                      "int32key",
							                                      "int64key",
							                                      "floatkey",
							                                      "doublekey",
							                                      "decimalkey",
							                                      "uuidkey",
							                                      "datetimekey",
							                                      "stringkey",
							                                      "blobkey",

							                                      "boolcolumn",
							                                      "int32column",
							                                      "int64column",
							                                      "floatcolumn",
							                                      "doublecolumn",
							                                      "decimalcolumn",
							                                      "uuidcolumn",
							                                      "datetimecolumn",
							                                      "stringcolumn",
							                                      "blobcolumn"
						                                      },
											});

			// Then

			Assert.IsNotNull(rows);
			var array = rows.ToArray();
			Assert.AreEqual(1, array.Length);
			CollectionAssert.AreEquivalent(parameters, array[0].ToArray());
		}
	}
}
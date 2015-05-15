using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;

using InfinniPlatform.Cassandra.Client;
using InfinniPlatform.Cassandra.DataAdapter;
using InfinniPlatform.Cassandra.Repository;
using InfinniPlatform.Cassandra.Storage;

using NUnit.Framework;

namespace InfinniPlatform.Cassandra.Tests.Client
{
	[TestFixture]
	[Category(TestCategories.PerformanceTest)]
	public sealed class CassandraPerformanceIntegrationTest
	{
		private ISession _session;
		private const string KeyspaceName = "TestPerformance";
		private const string TableName = "SomeTable";


		[SetUp]
		public void SetUp()
		{
			var sessionFactory = new CassandraSessionFactory();

			_session = sessionFactory.OpenSession();

			_session.Execute(new CreateKeyspaceQueryStatement { Keyspace = KeyspaceName });
			_session.Execute(new CreateTableQueryStatement { Keyspace = KeyspaceName, Table = TableName, Columns = new[] { new Column("key", ColumnType.Int32, true), new Column("value", ColumnType.String) } });
		}

		[TearDown]
		public void TearDown()
		{
			_session.Execute(new DropKeyspaceQueryStatement { Keyspace = KeyspaceName });
		}


		[Test]
		[TestCase(10000, 1)]
		[TestCase(10000, 4)]
		[TestCase(10000, 8)]
		[TestCase(10000, 16)]
		[TestCase(10000, 32)]
		[TestCase(10000, 64)]
		public void CrudPerformanceMultithreadTest(int rowCount, int threadCount)
		{
			// Given

			var results = new Dictionary<int, TestResult>(threadCount);

			// When

			var processStart = new ManualResetEvent(false);
			var threadStarted = new CountdownEvent(threadCount);
			var threadCompleted = new CountdownEvent(threadCount);

			var partSize = (int)Math.Ceiling((double)rowCount / threadCount);

			for (var t = 0; t < threadCount; ++t)
			{
				var rowFrom = Math.Min(t * partSize, rowCount);
				var rowTo = Math.Min(rowFrom + partSize, rowCount);

				new Thread(state =>
							   {
								   var threadId = (int)state;

								   threadStarted.Signal();
								   processStart.WaitOne();

								   try
								   {
									   results[threadId] = ProcessRangeRows(_session, rowFrom, rowTo);
								   }
								   finally
								   {
									   threadCompleted.Signal();
								   }

							   }) { IsBackground = true }
					.Start(t);
			}

			threadStarted.Wait();
			processStart.Set();
			threadCompleted.Wait();

			// Then

			Console.WriteLine("ROW COUNT: {0}", rowCount);
			Console.WriteLine("THREAD COUNT: {0} ({1} CPUs)", threadCount, Environment.ProcessorCount);
			Console.WriteLine();

			OutResults("TOTAL", 4 * rowCount, TimeSpan.FromMilliseconds(results.Values.Select(r => r.Total.TotalMilliseconds).Sum()));
			OutResults("INSERT", rowCount, TimeSpan.FromMilliseconds(results.Values.Select(r => r.Insert.TotalMilliseconds).Sum()));
			OutResults("UPDATE", rowCount, TimeSpan.FromMilliseconds(results.Values.Select(r => r.Update.TotalMilliseconds).Sum()));
			OutResults("DELETE", rowCount, TimeSpan.FromMilliseconds(results.Values.Select(r => r.Delete.TotalMilliseconds).Sum()));
			OutResults("SELECT", rowCount, TimeSpan.FromMilliseconds(results.Values.Select(r => r.Select.TotalMilliseconds).Sum()));
		}


		private static readonly Random Random = new Random(DateTime.Now.Millisecond);
		private static readonly InsertQueryStatement InsertStatement = new InsertQueryStatement { Keyspace = KeyspaceName, Table = TableName, Columns = new[] { "key", "value" } };
		private static readonly UpdateQueryStatement UpdateStatement = new UpdateQueryStatement { Keyspace = KeyspaceName, Table = TableName, Columns = new[] { "value" }, Where = new[] { new KeyFilter("key") } };
		private static readonly SelectQueryStatement SelectStatement = new SelectQueryStatement { Keyspace = KeyspaceName, Table = TableName, Where = new[] { new KeyFilter("key") } };
		private static readonly DeleteQueryStatement DeleteStatement = new DeleteQueryStatement { Keyspace = KeyspaceName, Table = TableName, Where = new[] { new KeyFilter("key") } };

		private static TestResult ProcessRangeRows(ISession session, int rowFrom, int rowTo)
		{
			var insertStopwatch = new Stopwatch();
			var updateStopwatch = new Stopwatch();
			var deleteStopwatch = new Stopwatch();
			var selectStopwatch = new Stopwatch();

			for (var i = rowFrom; i < rowTo; ++i)
			{
				var parameters = new object[] { i, Guid.NewGuid().ToString() };

				insertStopwatch.Start();

				session.Execute(InsertStatement, parameters);

				insertStopwatch.Stop();
			}

			for (var i = rowFrom; i < rowTo; ++i)
			{
				var parameters = new object[] { Guid.NewGuid().ToString(), Random.Next(0, rowTo - 1) };

				updateStopwatch.Start();

				session.Execute(UpdateStatement, parameters);

				updateStopwatch.Stop();
			}

			for (var i = rowFrom; i < rowTo; ++i)
			{
				var parameters = new object[] { Random.Next(0, rowTo - 1) };

				selectStopwatch.Start();

				// ReSharper disable ReturnValueOfPureMethodIsNotUsed
				session.Execute(SelectStatement, parameters).ToArray();
				// ReSharper restore ReturnValueOfPureMethodIsNotUsed

				selectStopwatch.Stop();
			}

			for (var i = rowFrom; i < rowTo; ++i)
			{
				var parameters = new object[] { Random.Next(0, rowTo - 1) };

				deleteStopwatch.Start();

				session.Execute(DeleteStatement, parameters);

				deleteStopwatch.Stop();
			}

			return new TestResult
					   {
						   Total = insertStopwatch.Elapsed + updateStopwatch.Elapsed + deleteStopwatch.Elapsed + selectStopwatch.Elapsed,
						   Insert = insertStopwatch.Elapsed,
						   Update = updateStopwatch.Elapsed,
						   Delete = deleteStopwatch.Elapsed,
						   Select = selectStopwatch.Elapsed
					   };
		}


		private static void OutResults(string operation, int totalCount, TimeSpan totalTime)
		{
			Console.WriteLine(operation);
			Console.WriteLine("   Total time: {0} ({1:N2} sec)", totalTime, totalTime.TotalSeconds);
			Console.WriteLine("   Average time: {0:N2} ms/operation", totalTime.TotalMilliseconds / totalCount);
			Console.WriteLine("   Average speed: {0:N3} operation/sec", totalCount / totalTime.TotalSeconds);
			Console.WriteLine();
		}


		class TestResult
		{
			public TimeSpan Total;
			public TimeSpan Insert;
			public TimeSpan Update;
			public TimeSpan Delete;
			public TimeSpan Select;
		}
	}
}
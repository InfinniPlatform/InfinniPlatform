using System;

using InfinniPlatform.Cassandra;
using InfinniPlatform.Cassandra.Client;

using NUnit.Framework;

namespace InfinniPlatform.EventStorage.Tests
{
	[TestFixture]
	[Category(TestCategories.IntegrationTest)]
	public sealed class CassandraEventStorageTest
	{
		[Test]
		public void ShouldAddEvents()
		{
			// Given

			var events = new[] { "Event1", "Event2" };

			var cassandraFactory = new CassandraDatabaseFactory(CassandraSettings.Default);
			var eventStorageFactory = new CassandraEventStorageFactory(cassandraFactory);

			var eventStorage = eventStorageFactory.CreateEventStorage();
			var eventStorageManager = eventStorageFactory.CreateEventStorageManager();

			try
			{
				eventStorageManager.DeleteStorage();
			}
			catch
			{
				// table not exists
			}

			eventStorageManager.CreateStorage();

			// When
			TestDelegate test = () => eventStorage.AddEvents(Guid.NewGuid(), events);

			// Then
			Assert.DoesNotThrow(test);
		}
	}
}
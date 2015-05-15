using System.Collections.Generic;

namespace InfinniPlatform.FastReport.Tests.TestEntities
{
	class Order
	{
		public Client Client { get; set; }
		public Manager Manager { get; set; }
		public IEnumerable<OrderItem> Items { get; set; }
	}
}
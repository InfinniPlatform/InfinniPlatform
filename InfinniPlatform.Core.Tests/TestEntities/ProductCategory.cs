using System.Collections.Generic;

namespace InfinniPlatform.Core.Tests.TestEntities
{
    internal class ProductCategory
    {
        public IEnumerable<IProduct> Products { get; set; }
    }
}
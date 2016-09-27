using System.Collections.Generic;

namespace InfinniPlatform.Sdk.Tests.TestEntities
{
    internal class ProductCategory
    {
        public IEnumerable<IProduct> Products { get; set; }
    }
}
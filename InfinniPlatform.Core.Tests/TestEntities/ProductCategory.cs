using System.Collections.Generic;

namespace InfinniPlatform.TestEntities
{
    internal class ProductCategory
    {
        public IEnumerable<IProduct> Products { get; set; }
    }
}
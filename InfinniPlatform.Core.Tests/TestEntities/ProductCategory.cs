using System.Collections.Generic;

namespace InfinniPlatform.Core.TestEntities
{
    internal class ProductCategory
    {
        public IEnumerable<IProduct> Products { get; set; }
    }
}
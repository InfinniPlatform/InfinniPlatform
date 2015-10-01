using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InfinniPlatform.Sdk.ContextComponents
{
    public interface ISharedCacheComponent
    {
        object Get(string key);

        void Set(string key, object item);
        void Lock();
        void Unlock();
    }
}

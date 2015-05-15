using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InfinniPlatform.Api.Transactions
{
    public interface ITransaction
    {
        void CommitTransaction();

        void Attach(AttachedInstance item);

        string GetTransactionMarker();

        List<AttachedInstance> GetTransactionItems();
    }
}

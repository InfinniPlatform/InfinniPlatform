using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InfinniPlatform.Api.Transactions
{
    public interface ITransactionManager
    {
        ITransaction GetTransaction(string transactionMarker);

        void Attach(string transactionMarker, AttachedInstance document);
    }
}

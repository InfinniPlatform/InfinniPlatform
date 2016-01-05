using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using InfinniPlatform.Api.ContextTypes;
using InfinniPlatform.Api.Dynamic;
using InfinniPlatform.Api.RestApi.DataApi;

namespace InfinniPlatform.Api.Tests.RestBehavior.TestActions.Transactions
{
    public sealed class TestSaveDocumentFailAction
    {
        public void Action(IApplyContext target)
        {
            if (target.Item.IsAdditionalDocument != null)
            {
                return;
            }

            dynamic address = new DynamicWrapper();
            address.IsAdditionalDocument = true;
            target.SetDocument(target.Item.Configuration, "Address", address);

            //выкидываем исключение, должен быть откачен основной документ и документ, созданный в данном обработчике
            throw new ArgumentException();
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using InfinniPlatform.Sdk.ApiContracts;
using InfinniPlatform.Sdk.Contracts;
using InfinniPlatform.Sdk.Global;

namespace InfinniPlatform.Sdk.Tests.TestData
{
    public sealed class ActionUnitOnSaveComment
    {
        public void Action(IApplyContext target)
        {
            target.Item.Text = target.Item.Text + "123";
            target.Context.GetComponent<IDocumentApi>()
                .SetDocument("gameshop", "review", new
                {
                    Text = "test"
                });
        }
    }
}

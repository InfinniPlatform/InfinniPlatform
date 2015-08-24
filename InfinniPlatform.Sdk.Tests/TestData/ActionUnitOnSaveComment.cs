using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using InfinniPlatform.Sdk.Contracts;

namespace InfinniPlatform.Sdk.Tests.TestData
{
    public sealed class ActionUnitOnSaveComment
    {
        public void Action(IApplyContext target)
        {
            target.Item.Text = target.Item.Text + "123";
        }
    }
}

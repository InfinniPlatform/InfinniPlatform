﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using InfinniPlatform.Sdk.Contracts;
using InfinniPlatform.Sdk.Dynamic;

namespace InfinniPlatform.Sdk.Tests.TestData
{
    public class ActionUnitHelloWorld2
    {
        public void Action(IActionContext target)
        {
            target.Result = new DynamicWrapper();
            target.Result.Message = string.Format("Hello world 2");
        }
    }
}

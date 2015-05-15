﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using InfinniPlatform.Api.Hosting;

namespace InfinniPlatform.Hosting
{
    /// <summary>
    ///   Интерфейс обработчика web-роутинга
    /// </summary>
    public interface IWebRoutingHandler
    {
        IConfigRequestProvider ConfigRequestProvider { get; set; }
    }
}

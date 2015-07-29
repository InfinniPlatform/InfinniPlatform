﻿using System.Collections.Generic;

namespace InfinniPlatform.FlowDocument.Model.Blocks
{
    public class ListItem : PrintElement
    {

        public ListItem()
        {
            Blocks = new List<PrintElementBlock>();
        }
        public List<PrintElementBlock> Blocks { get; set; }
    }
}

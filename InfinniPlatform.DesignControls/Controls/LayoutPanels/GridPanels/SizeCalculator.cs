using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using InfinniPlatform.DesignControls.ObjectInspector;

namespace InfinniPlatform.DesignControls.Controls.LayoutPanels.GridPanels
{
    public sealed class SizeCalculator
    {
        private readonly int _currentHeight;
        private readonly ObjectInspectorTree _objectInspector;

        public SizeCalculator(ObjectInspectorTree objectInspector, int currentHeight)
        {
            _objectInspector = objectInspector;
            _currentHeight = currentHeight;
        }

        public int FullSize { get; private set; }

        public Dictionary<CompositPanel, int> Calculate(CompositPanel[] panels)
        {
            FullSize = 0;
            var sizes = new Dictionary<CompositPanel, int>();
            for (var i = 0; i < panels.Count(); i++)
            {
                var sizeMax = 0;
                //проходим по каждой ячейке строки, выбираем максимальную из высот
                foreach (Control control in panels[i].Controls[0].Controls)
                {
                    sizeMax = Math.Max(sizeMax, _objectInspector.GetSize(control));
                }

                if (sizeMax > 0)
                {
                    sizeMax += 20;
                }


                FullSize = FullSize + sizeMax;
                sizes.Add(panels[i], sizeMax);
            }


            var restHeight = _currentHeight - FullSize > 0 ? (_currentHeight - FullSize) : 0;
                //sizeToCalcMin.Any() ? sizeToCalcMin.Select(s => s.Value).Min() : 0;
            var minValue = 0;

            var emptyKeys = sizes.Where(s => s.Value == 0).Select(s => s.Key).ToList();

            if (restHeight > 0)
            {
                if (emptyKeys.Count > 0)
                {
                    minValue = restHeight/emptyKeys.Count;
                }
                else
                {
                    minValue = restHeight;
                }
            }


            FullSize = FullSize + emptyKeys.Count*minValue;


            foreach (var compositPanel in emptyKeys)
            {
                sizes[compositPanel] = minValue;
            }


            foreach (var compositPanel in panels)
            {
                //var originalHeight = (Height/Panels.Count);
                decimal originalHeight = (_currentHeight/panels.Count());
                if (FullSize > 0 && _currentHeight > 0)
                {
                    var scale = (decimal) _currentHeight/FullSize;
                    sizes[compositPanel] = Convert.ToInt32(sizes[compositPanel]*scale);
                }
                else
                {
                    sizes[compositPanel] = Convert.ToInt32(originalHeight);
                }
            }
            return sizes;
        }
    }
}
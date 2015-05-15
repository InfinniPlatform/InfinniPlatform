using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using InfinniPlatform.DesignControls.ObjectInspector;

namespace InfinniPlatform.DesignControls.Controls.LayoutPanels.GridPanels
{
    public sealed class SizeCalculator
    {
        private readonly ObjectInspectorTree _objectInspector;
        private readonly int _currentHeight;
        private int _fullSize;

        public SizeCalculator(ObjectInspectorTree objectInspector, int currentHeight)
        {
            _objectInspector = objectInspector;
            _currentHeight = currentHeight;
        }

        public int FullSize
        {
            get { return _fullSize; }
        }

        public Dictionary<CompositPanel,int> Calculate(CompositPanel[] panels )
        {
            _fullSize = 0;
            var sizes = new Dictionary<CompositPanel, int>();
            for (int i = 0; i < panels.Count(); i++)
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


				_fullSize = FullSize + sizeMax;
                sizes.Add(panels[i], sizeMax);
            }

            
	        var restHeight = _currentHeight - _fullSize > 0 ? (_currentHeight - _fullSize) : 0;  //sizeToCalcMin.Any() ? sizeToCalcMin.Select(s => s.Value).Min() : 0;
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
            

            _fullSize = FullSize + emptyKeys.Count * minValue;


            foreach (CompositPanel compositPanel in emptyKeys)
            {
                sizes[compositPanel] = minValue;
            }


            foreach (CompositPanel compositPanel in panels)
            {
                //var originalHeight = (Height/Panels.Count);
                decimal originalHeight = (_currentHeight / panels.Count());
                if (FullSize > 0 && _currentHeight > 0)
                {
                    decimal scale = (decimal) _currentHeight/FullSize;
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

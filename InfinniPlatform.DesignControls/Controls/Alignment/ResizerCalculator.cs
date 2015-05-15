using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using InfinniPlatform.DesignControls.PropertyDesigner;

namespace InfinniPlatform.DesignControls.Controls.Alignment
{
    public class ResizerCalculator
    {
        private readonly PropertiesControl _control;

        public ResizerCalculator( PropertiesControl control)
        {
            _control = control;
        }

        public int GetSize(IEnumerable<ResizerFixed> resizers)
        {
            var listResizers = new List<ResizerFixed>();

            var checkChildResizers = resizers.ToList();
            foreach (ResizerFixed resizer in checkChildResizers)
            {
                var parentList = new List<Control>();
                GetParents(parentList, resizer.GetControl());
                if (parentList.FirstOrDefault(p => p == _control) != null)
                {
                    listResizers.Add(resizer);                    
                }
            }

            listResizers = CheckResizersIntersect(listResizers);

            return listResizers.Sum(r => r.GetSize());           
        }

        private List<ResizerFixed> CheckResizersIntersect(List<ResizerFixed> listResizers)
        {

            var removedControls = new List<ResizerFixed>();
            foreach (var resizerFixed in listResizers)
            {
                var checkList = listResizers.ToList();
                var parentsAll = new List<Control>();
                GetParents(parentsAll, resizerFixed.GetControl());
                foreach (var @fixed in checkList)
                {
                    if(parentsAll.Contains(@fixed.GetControl()))
                    {
                        removedControls.Add(resizerFixed);
                    }
                }
            }

            return listResizers.Except(removedControls).ToList();
        }


        private List<Control> GetParents(List<Control> parents, Control control)
        {
            var parent = control.Parent;
            if (parent != null)
            {
                parents.Add(parent);
                return GetParents(parents, parent);
            }
            return parents;
        } 

        public PropertiesControl GetControl()
        {
            return _control;
        }
    }
}

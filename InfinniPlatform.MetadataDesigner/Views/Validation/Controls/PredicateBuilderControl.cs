using System.Windows.Forms;

namespace InfinniPlatform.MetadataDesigner.Views.Validation.Controls
{
    public partial class PredicateBuilderControl : UserControl, IPredicateBuilderControl
    {
        public PredicateBuilderControl()
        {
            InitializeComponent();
        }

        public PredicateDescriptionNode GetPredicateDescription()
        {
            var methodName = radioGroup1.Properties.Items[radioGroup1.SelectedIndex].Value.ToString();
            
            return new PredicateDescriptionNode(PredicateDescriptionType.Root, methodName, new string[0], new object[0]);
        }

        public PredicateDescriptionType GetNextControlType()
        {
            if (radioGroup1.SelectedIndex == 0)
            {
                return PredicateDescriptionType.ObjectComposite;
            }

            return PredicateDescriptionType.CollectionComposite;
        }
    }
}

using System.Windows.Forms;

namespace InfinniPlatform.MetadataDesigner.Views.Validation.Controls
{
    public partial class ObjectCompositePredicateBuilderControl : UserControl, IPredicateBuilderControl
    {
        public ObjectCompositePredicateBuilderControl()
        {
            InitializeComponent();
        }

        public PredicateDescriptionNode GetPredicateDescription()
        {
            var methodName = radioGroup1.Properties.Items[radioGroup1.SelectedIndex].Value.ToString();

            return new PredicateDescriptionNode(PredicateDescriptionType.ObjectComposite, methodName, new string[0], new object[0]);
        }

        public PredicateDescriptionType GetNextControlType()
        {
            return PredicateDescriptionType.Object;
        }
    }
}

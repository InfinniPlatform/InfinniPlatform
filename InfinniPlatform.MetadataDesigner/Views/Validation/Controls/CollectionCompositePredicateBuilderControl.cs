using System.Windows.Forms;

namespace InfinniPlatform.MetadataDesigner.Views.Validation.Controls
{
    /// <summary>
    /// Контрол для конструирования композитной операции
    /// Набор допустимых операций определен внутри RadioGroup
    /// </summary>
    public partial class CollectionCompositePredicateBuilderControl : UserControl, IPredicateBuilderControl
    {
        public CollectionCompositePredicateBuilderControl()
        {
            InitializeComponent();
        }

        public PredicateDescriptionNode GetPredicateDescription()
        {
            var methodName = radioGroup1.Properties.Items[radioGroup1.SelectedIndex].Value.ToString();

            return new PredicateDescriptionNode(PredicateDescriptionType.CollectionComposite, methodName, new string[0], new object[0]);
        }

        public PredicateDescriptionType GetNextControlType()
        {
            return PredicateDescriptionType.Collection;
        }
    }
}

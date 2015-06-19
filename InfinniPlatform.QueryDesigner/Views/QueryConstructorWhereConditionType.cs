using System.Windows.Forms;
using DevExpress.XtraEditors.Controls;
using InfinniPlatform.Api.SearchOptions;

namespace InfinniPlatform.QueryDesigner.Views
{
    public partial class QueryConstructorWhereConditionType : UserControl
    {
        public QueryConstructorWhereConditionType()
        {
            InitializeComponent();

            ComboBoxConditionType.Properties.Items.Clear();
            ComboBoxConditionType.Properties.Items.Add(new ImageComboBoxItem("IsEquals", CriteriaType.IsEquals));
            ComboBoxConditionType.Properties.Items.Add(new ImageComboBoxItem("IsNotEquals", CriteriaType.IsNotEquals));
            ComboBoxConditionType.Properties.Items.Add(new ImageComboBoxItem("IsEmpty", CriteriaType.IsEmpty));
            ComboBoxConditionType.Properties.Items.Add(new ImageComboBoxItem("IsNotEmpty", CriteriaType.IsNotEmpty));
            ComboBoxConditionType.Properties.Items.Add(new ImageComboBoxItem("IsContains", CriteriaType.IsContains));
            ComboBoxConditionType.Properties.Items.Add(new ImageComboBoxItem("IsNotContains", CriteriaType.IsNotContains));
            ComboBoxConditionType.Properties.Items.Add(new ImageComboBoxItem("IsStartsWith", CriteriaType.IsStartsWith));
            ComboBoxConditionType.Properties.Items.Add(new ImageComboBoxItem("IsNotStartsWith",
                CriteriaType.IsNotStartsWith));
            ComboBoxConditionType.Properties.Items.Add(new ImageComboBoxItem("IsEndsWith", CriteriaType.IsEndsWith));
            ComboBoxConditionType.Properties.Items.Add(new ImageComboBoxItem("IsNotEndsWith", CriteriaType.IsNotEndsWith));
            ComboBoxConditionType.Properties.Items.Add(new ImageComboBoxItem("IsLessThan", CriteriaType.IsLessThan));
            ComboBoxConditionType.Properties.Items.Add(new ImageComboBoxItem("IsLessThanOrEquals",
                CriteriaType.IsLessThanOrEquals));
            ComboBoxConditionType.Properties.Items.Add(new ImageComboBoxItem("IsMoreThan", CriteriaType.IsMoreThan));
            ComboBoxConditionType.Properties.Items.Add(new ImageComboBoxItem("IsMoreThanOrEquals",
                CriteriaType.IsMoreThanOrEquals));
        }

        public CriteriaType Value
        {
            get { return (CriteriaType) ComboBoxConditionType.EditValue; }
            set { ComboBoxConditionType.EditValue = value; }
        }
    }
}
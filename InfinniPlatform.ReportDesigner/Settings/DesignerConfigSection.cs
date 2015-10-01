using System.Configuration;

namespace InfinniPlatform.ReportDesigner.Settings
{
    public sealed class DesignerConfigSection : ConfigurationSection
    {
        private const string ReportServicesKeyName = "reportServices";

        public static DesignerConfigSection Instance
        {
            get { return (DesignerConfigSection) ConfigurationManager.GetSection("designer"); }
        }

        [ConfigurationProperty(ReportServicesKeyName)]
        [ConfigurationCollection(typeof (ValueConfigElementCollection))]
        public ValueConfigElementCollection ReportServices
        {
            get { return (ValueConfigElementCollection) base[ReportServicesKeyName]; }
            set { base[ReportServicesKeyName] = value; }
        }
    }
}
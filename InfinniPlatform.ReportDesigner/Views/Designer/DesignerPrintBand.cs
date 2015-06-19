using FastReport;

namespace InfinniPlatform.ReportDesigner.Views.Designer
{
    internal sealed class DesignerPrintBand
    {
        public DesignerPrintBand(BandBase band)
        {
            Name = band.Name;
        }

        public string Name { get; private set; }
    }
}
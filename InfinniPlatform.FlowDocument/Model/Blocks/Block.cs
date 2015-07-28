namespace InfinniPlatform.FlowDocument.Model.Blocks
{
    public class Block : TextElement
    {
        public Brush BorderBrush { get; set; }

        public Thickness Margin { get; set; }

        public Thickness Padding { get; set; }

        public Thickness BorderThickness { get; set; }

        public bool BreakPageBefore { get; set; }
    }
}

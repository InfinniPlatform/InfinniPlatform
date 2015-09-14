namespace InfinniPlatform.DesignControls.Layout
{
    public interface ILayoutProvider
    {
        dynamic GetLayout();
        void SetLayout(dynamic value);
        string GetPropertyName();
    }
}
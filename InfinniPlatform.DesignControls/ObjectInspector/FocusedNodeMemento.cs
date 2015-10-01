namespace InfinniPlatform.DesignControls.ObjectInspector
{
    public sealed class FocusedNodeMemento
    {
        private PropertiesNode _state;
        private readonly ObjectInspectorTree _inspector;

        public FocusedNodeMemento(ObjectInspectorTree inspector)
        {
            _inspector = inspector;
        }

        public void BeginUpdate()
        {
            _state = _inspector.FocusedPropertiesNode;
        }

        public void EndUpdate()
        {
            _inspector.FocusedPropertiesNode = _state;
        }
    }
}
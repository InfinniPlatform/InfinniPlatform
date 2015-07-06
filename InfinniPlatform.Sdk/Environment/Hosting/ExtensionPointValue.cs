namespace InfinniPlatform.Sdk.Environment.Hosting
{
    public sealed class ExtensionPointValue
    {
        private readonly string _stateMachineReference;
        private readonly string _typeName;

        public ExtensionPointValue(string typeName, string stateMachineReference)
        {
            _typeName = typeName;
            _stateMachineReference = stateMachineReference;
        }

        public string TypeName
        {
            get { return _typeName; }
        }

        public string StateMachineReference
        {
            get { return _stateMachineReference; }
        }
    }
}
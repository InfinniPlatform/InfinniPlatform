namespace InfinniPlatform.Sdk.Environment.Actions
{
    public interface IActionOperator
    {
        string Description { get; }
        void Action(dynamic target);
    }
}
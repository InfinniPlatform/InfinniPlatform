namespace InfinniPlatform.Api.Actions
{
    public interface IActionOperator
    {
        string Description { get; }
        void Action(dynamic target);
    }
}
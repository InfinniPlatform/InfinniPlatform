namespace InfinniPlatform.QueryDesigner.Contracts
{
    public interface IQueryBlockProvider
    {
        ConstructOrder GetConstructOrder();
        void ProcessQuery(dynamic query);
        bool DefinitionCompleted();
        string GetErrorMessage();
    }
}
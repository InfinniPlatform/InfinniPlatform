namespace InfinniPlatform.Api.Factories
{
    public interface IScriptProcessor
    {
        object InvokeScript(string scriptIdentifier, dynamic scriptContext);
    }
}
namespace InfinniPlatform.Sdk.Environment.Scripts
{
    public interface IScriptProcessor
    {
        object InvokeScript(string scriptIdentifier, dynamic scriptContext);
    }
}
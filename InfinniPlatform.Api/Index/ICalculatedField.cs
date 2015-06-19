namespace InfinniPlatform.Api.Index
{
    public interface ICalculatedField
    {
        ICalculatedField Add(ICalculatedField item);
        ICalculatedField Subtract(ICalculatedField item);
        ICalculatedField Divide(ICalculatedField item);
        ICalculatedField Multiply(ICalculatedField item);
        string GetRawScript();
    }
}
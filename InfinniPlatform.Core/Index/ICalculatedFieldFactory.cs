using InfinniPlatform.Api.Index;
using InfinniPlatform.Sdk.Environment.Index;

namespace InfinniPlatform.Index
{
    public interface ICalculatedFieldFactory
    {
        ICalculatedField Field(string fieldName);
        ICalculatedField Const(double item);
        ICalculatedField Const(int item);
        ICalculatedField RawString(string item);
        ICalculatedField DateTrimTime(string fieldName);
    }
}
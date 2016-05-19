using InfinniPlatform.Sdk.Documents;
using InfinniPlatform.Sdk.Documents.Obsolete;

namespace InfinniPlatform.Core.Index
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
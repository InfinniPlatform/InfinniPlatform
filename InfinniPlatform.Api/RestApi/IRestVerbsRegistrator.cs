using InfinniPlatform.Api.RestQuery;
using InfinniPlatform.Sdk.Environment.Hosting;

namespace InfinniPlatform.Api.RestApi
{
    public interface IRestVerbsRegistrator
    {
        /// <summary>
        /// �������� ���������� ������� ������� REST
        /// </summary>
        /// <returns>��������� ������������ REST ��������</returns>
        IRestVerbsContainer AddVerb(IQueryHandler queryHandler);
    }
}
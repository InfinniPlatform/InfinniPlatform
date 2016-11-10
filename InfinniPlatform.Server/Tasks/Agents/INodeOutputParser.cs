using InfinniPlatform.Sdk.Http.Services;

namespace InfinniPlatform.Server.Tasks.Agents
{
    /// <summary>
    /// ���������� ����������� ������ ���������� Infinni.Node.
    /// </summary>
    public interface INodeOutputParser
    {
        /// <summary>
        /// ����������� ����� ������� status � ��������� � ������ �������.
        /// </summary>
        /// <param name="serviceResult">����� �������.</param>
        ServiceResult<AgentTaskStatus> FormatAppsInfoOutput(ServiceResult<AgentTaskStatus> serviceResult);
    }
}
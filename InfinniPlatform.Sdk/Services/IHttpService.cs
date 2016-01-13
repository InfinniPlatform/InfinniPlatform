namespace InfinniPlatform.Sdk.Services
{
    /// <summary>
    /// ������ ����������� ������������ �������� �������.
    /// </summary>
    public interface IHttpService
    {
        /// <summary>
        /// ��������� ������.
        /// </summary>
        /// <param name="builder">����������� ������������ ��������.</param>
        void Load(IHttpServiceBuilder builder);
    }
}
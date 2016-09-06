namespace InfinniPlatform.Sdk.Hosting
{
    /// <summary>
    /// ��������� ��� ������� ������� �����.
    /// </summary>
    public interface IHostAddressParser
    {
        /// <summary>
        /// ����������, �������� �� ����� ���������.
        /// </summary>
        /// <param name="hostNameOrAddress">��� ���� ��� ��� �����.</param>
        /// <returns>���������� <c>true</c>, ���� ����� �������� ���������; ����� ���������� <c>false</c>.</returns>
        bool IsLocalAddress(string hostNameOrAddress);

        /// <summary>
        /// ����������, �������� �� ����� ���������.
        /// </summary>
        /// <param name="hostNameOrAddress">��� ���� ��� ��� �����.</param>
        /// <param name="normalizedAddress">��������������� ����� ����.</param>
        /// <returns>���������� <c>true</c>, ���� ����� �������� ���������; ����� ���������� <c>false</c>.</returns>
        bool IsLocalAddress(string hostNameOrAddress, out string normalizedAddress);
    }
}
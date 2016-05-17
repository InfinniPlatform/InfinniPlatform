namespace InfinniPlatform.Sdk.Queues
{
    /// <summary>
    /// ��������� � �������.
    /// </summary>
    /// <typeparam name="T">��� ���� ���������.</typeparam>
    public interface IMessage<out T> : IMessage where T : class
    {
        /// <summary>
        /// ���� ���������.
        /// </summary>
        T Body { get; }
    }
}
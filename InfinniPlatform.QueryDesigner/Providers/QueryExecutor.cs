namespace InfinniPlatform.QueryDesigner.Providers
{
    internal sealed class QueryExecutor
    {
        private string _host;
        private string _port;

        public void SetConnectionSettings(string host, string port)
        {
            _host = host;
            _port = port;
        }
    }
}
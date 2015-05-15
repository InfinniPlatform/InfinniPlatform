namespace InfinniPlatform.Index.QueryLanguage.Implementation
{
    public class ResultLimits
    {
        public int StartPage { get; private set; }
        public int PageSize { get; private set; }
        public int Skip { get; private set; }

        public ResultLimits(int startPage, int pageSize, int skip)
        {
            Skip = skip;
            PageSize = pageSize;
            StartPage = startPage;
        }
    }
}

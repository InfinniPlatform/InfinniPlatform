namespace InfinniPlatform.ElasticSearch.QueryLanguage
{
    public class ResultLimits
    {
        public ResultLimits(int startPage, int pageSize, int skip)
        {
            Skip = skip;
            PageSize = pageSize;
            StartPage = startPage;
        }

        public int StartPage { get; private set; }
        public int PageSize { get; private set; }
        public int Skip { get; private set; }
    }
}
namespace InfinniPlatform.DocumentStorage.Tests.TestEntities
{
    internal class Article
    {
        public object _id { get; set; }

        public string subject { get; set; }

        public string author { get; set; }

        public int views { get; set; }

        public object score { get; set; }
    }
}
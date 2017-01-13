namespace InfinniPlatform.Core.Settings
{
    internal sealed class AppEnvironment
    {
        public App App { get; set; }
    }

    public class App
    {
        public string Name { get; set; }
        public string InstanceId { get; set; }
        public bool IsInCluster { get; set; }
    }

//    internal sealed class AppEnvironment : IAppEnvironment
//    {
//        public const string SectionName = "app";
//
//        public AppEnvironment()
//        {
//            Name = "InfinniPlatform";
//            IsInCluster = false;
//            _instanceId = $"{Environment.MachineName}_{Directory.GetCurrentDirectory() .Split(Path.DirectorySeparatorChar) .Last()}";
//        }
//
//        public App App { get; set; }
//
//        private string _instanceId;
//
//        public string Name { get; set; }
//
//        public string InstanceId
//        {
//            get { return _instanceId; }
//            set
//            {
//                if (!string.IsNullOrWhiteSpace(value))
//                {
//                    _instanceId = value;
//                }
//            }
//        }
//
//        public bool IsInCluster { get; set; }
//    }
}
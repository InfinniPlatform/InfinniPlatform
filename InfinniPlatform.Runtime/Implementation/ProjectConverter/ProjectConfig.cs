namespace InfinniPlatform.Runtime.Implementation.ProjectConverter
{
    public sealed class ProjectConfig
    {
        /// <summary>
        /// версия .NET Framework, на которую ориентирован проект
        /// </summary>
        public string TargetFrameworkVersion { set; get; }
        public string DebugOutputPath { set; get; }
        public string ReleaseOutputPath { set; get; }
    }
}

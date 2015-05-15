namespace InfinniPlatform.Runtime.Implementation.ProjectConverter
{
    /// <summary>
    /// Хранит информацию о том, как связаны между собой названия свойств классов IProjectComponent
    /// и названия аттрибутов и тэгов xml файла
    /// </summary>
    public class ReflectionPair
    {
        public string PropertyName { get; private set; }
        public string XmlElementName { get; private set; }

        public ReflectionPair(string propertyName, string xmlElementName)
        {
            PropertyName = propertyName;
            XmlElementName = xmlElementName;
        }
    }
}

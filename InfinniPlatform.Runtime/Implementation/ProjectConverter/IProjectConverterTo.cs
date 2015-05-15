using System.IO;

namespace InfinniPlatform.Runtime.Implementation.ProjectConverter
{
    public interface IProjectConverterTo
    {      
        /// <summary>
        /// Создает на основе объекта класса Project текст файла proj
        /// </summary>
        /// <param name="initialProject">объект, в котором содержиться информация о проекте</param>
        /// <returns>поток, содержащий текст proj файла</returns>
        Stream Convert(Project initialProject);
    }
}

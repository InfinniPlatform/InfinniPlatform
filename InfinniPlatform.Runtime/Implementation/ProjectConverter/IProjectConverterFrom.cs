using System.IO;

namespace InfinniPlatform.Runtime.Implementation.ProjectConverter
{
    public interface IProjectConverterFrom
    {
        /// <summary>
        /// Сохраняет важную информацию из proj файла в объект Project
        /// </summary>
        /// <param name="projectText">поток, содержащий текст proj файла</param>
        /// <returns>объект Project, в котором содержиться существенная информация о проекте</returns>
        Project Convert(Stream projectText);
    }
}

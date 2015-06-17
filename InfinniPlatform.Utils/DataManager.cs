using System;
using System.IO;

using InfinniPlatform.Api.Dynamic;
using InfinniPlatform.Api.Hosting;
using InfinniPlatform.Api.RestApi.CommonApi;
using InfinniPlatform.Api.TestEnvironment;

namespace InfinniPlatform.Utils
{
	public class DataManager
	{
		public void Import(string pathToFolder)
		{
			TestApi.InitClientRouting(new HostingConfig());

			if (!Directory.Exists(pathToFolder))
			{
				Console.WriteLine("Specified path {0} doesn't exist", pathToFolder);
				return;
			}

			foreach (var folderFullFile in Directory.EnumerateFiles(pathToFolder, "*.zip", SearchOption.TopDirectoryOnly))
			{
				var fileName = Path.GetFileNameWithoutExtension(folderFullFile);

				var dividerIndex = fileName.IndexOf('_');

				if (dividerIndex == -1)
				{
					Console.WriteLine("File {0} skipped due to incorrect name", folderFullFile);
					continue;
				}

				dynamic item = new DynamicWrapper();

				item.Configuration = fileName.Substring(0, dividerIndex);
				item.Metadata = fileName.Substring(dividerIndex + 1);

				var classifierData = Convert.ToBase64String(File.ReadAllBytes(folderFullFile));

				item.FileContent = classifierData;

				try
				{
					RestQueryApi.QueryPostJsonRaw("SystemConfig", "metadata", "ImportDataFromJson", null, item, null);

					Console.WriteLine("Data from file {0} imported", folderFullFile);
				}
				catch (Exception e)
				{
					Console.WriteLine("Fail to import data from file {0}: ", e);
				}
			}
		}
	}
}
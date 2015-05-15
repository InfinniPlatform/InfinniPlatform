using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Resources;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace InfinniPlatform.UserInterface.ViewBuilders.Images
{
	/// <summary>
	/// Хранилище изображений.
	/// </summary>
	sealed class ImageRepository
	{
		static ImageRepository()
		{
			Resources = GetResourceNames();
			ImageCache = new Dictionary<string, ImageSource>(StringComparer.OrdinalIgnoreCase);
		}


		private static readonly string[] Resources;
		private static readonly Dictionary<string, ImageSource> ImageCache;


		public static string[] GetResourceNames()
		{
			var assembly = Assembly.GetCallingAssembly();
			var assemblyName = assembly.GetName().Name;
			var assemblyResources = assemblyName + ".g.resources";

			using (var stream = assembly.GetManifestResourceStream(assemblyResources))
			{
				if (stream != null)
				{
					using (var reader = new ResourceReader(stream))
					{
						return reader.Cast<DictionaryEntry>()
							.Select(entry => string.Format("pack://application:,,,/{0};component/{1}", assemblyName, entry.Key))
							.ToArray();
					}
				}
			}

			return new string[] { };
		}


		/// <summary>
		/// Получить изображение по имени.
		/// </summary>
		public static ImageSource GetImage(string name)
		{
			ImageSource image = null;

			if (!string.IsNullOrEmpty(name))
			{
				if (ImageCache.TryGetValue(name, out image) == false)
				{
					try
					{
						var imagePath = name;

						if (name.IndexOf('/') < 0)
						{
							imagePath = "System/" + name;
						}

						if (!name.EndsWith("_16x16", StringComparison.OrdinalIgnoreCase))
						{
							imagePath += "_16x16";
						}

						if (!name.EndsWith(".png", StringComparison.OrdinalIgnoreCase))
						{
							imagePath += ".png";
						}

						var imagResource = Resources.FirstOrDefault(r => r.EndsWith(imagePath, StringComparison.OrdinalIgnoreCase));

						if (imagResource != null)
						{
							image = new BitmapImage(new Uri(imagResource));
						}
					}
					catch (Exception)
					{
						image = null;
					}

					ImageCache[name] = image;
				}
			}

			return image;
		}
	}
}
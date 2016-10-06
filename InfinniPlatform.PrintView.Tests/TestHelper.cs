using System.Drawing;
using System.Drawing.Imaging;
using System.IO;

using InfinniPlatform.PrintView.Model.Inline;

using NUnit.Framework;

namespace InfinniPlatform.PrintView.Tests
{
    internal static class TestHelper
    {
        public static byte[] BitmapToBytes(Bitmap bitmap)
        {
            if (bitmap != null)
            {
                using (var stream = new MemoryStream())
                {
                    bitmap.Save(stream, ImageFormat.Png);

                    return stream.ToArray();
                }
            }

            return null;
        }

        public static void AssertImagesAreEqual(Bitmap expected, PrintImage actual)
        {
            using (var imageStream = new MemoryStream(actual.Data))
            using (var imageBitmap = new Bitmap(imageStream))
            {
                Assert.AreEqual(expected.Width, imageBitmap.Width);
                Assert.AreEqual(expected.Height, imageBitmap.Height);

                for (var x = 0; x < expected.Width; x++)
                {
                    for (var y = 0; y < expected.Height; y++)
                    {
                        Assert.AreEqual(expected.GetPixel(x, y), imageBitmap.GetPixel(x, y), $"X={x}, Y={y}");
                    }
                }
            }
        }

        public static string GetEmbeddedResource(string resourceName)
        {
            var assembly = typeof(TestHelper).Assembly;
            var resource = assembly.GetManifestResourceStream($"{assembly.GetName().Name}.{resourceName}");

            if (resource != null)
            {
                using (resource)
                using (var reader = new StreamReader(resource))
                {
                    return reader.ReadToEnd();
                }
            }

            return null;
        }
    }
}
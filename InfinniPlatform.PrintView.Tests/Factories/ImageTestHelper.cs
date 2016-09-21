using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;

using InfinniPlatform.PrintView.Model.Inlines;

using NUnit.Framework;

namespace InfinniPlatform.PrintView.Tests.Factories
{
    internal static class ImageTestHelper
    {
        public static string BitmapToBase64(Bitmap bitmap)
        {
            if (bitmap != null)
            {
                using (var stream = new MemoryStream())
                {
                    bitmap.Save(stream, ImageFormat.Bmp);

                    return Convert.ToBase64String(stream.ToArray());
                }
            }

            return null;
        }

        public static void AssertImagesAreEqual(Bitmap expected, PrintElementImage image)
        {
            var actual = new Bitmap(image.Source);

            Assert.AreEqual(expected.Width, actual.Width);
            Assert.AreEqual(expected.Height, actual.Height);

            for (var x = 0; x < expected.Width; x++)
            {
                for (var y = 0; y < expected.Height; y++)
                {
                    Assert.AreEqual(expected.GetPixel(x, y), actual.GetPixel(x, y), "X={0}, Y={1}", x, y);
                }
            }
        }
    }
}
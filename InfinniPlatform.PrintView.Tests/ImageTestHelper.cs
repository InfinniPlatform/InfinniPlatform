using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Runtime.InteropServices;

using InfinniPlatform.PrintView.Model.Inlines;

using NUnit.Framework;

namespace InfinniPlatform.PrintView.Tests
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
            CollectionAssert.AreEqual(BitmapToPixels(expected), BitmapToPixels(image.Source));
        }

        private static IEnumerable<byte> BitmapToPixels(Bitmap bitmap)
        {
            var bitmapRect = new Rectangle(0, 0, bitmap.Width, bitmap.Height);
            var bitmapData = bitmap.LockBits(bitmapRect, ImageLockMode.ReadOnly, PixelFormat.Format32bppArgb);

            var stride = bitmapData.Stride;
            var size = bitmap.Height*stride;
            var pixels = new byte[size];
            Marshal.Copy(bitmapData.Scan0, pixels, 0, size);

            bitmap.UnlockBits(bitmapData);

            return pixels;
        }
    }
}
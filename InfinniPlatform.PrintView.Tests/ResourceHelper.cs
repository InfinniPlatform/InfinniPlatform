using System.IO;
using System.Linq;
using System.Reflection;
using SixLabors.ImageSharp;

namespace InfinniPlatform.PrintView
{
    internal class ResourceHelper
    {
        public static ImageInfo Image => GetEmbeddedResourceImage("Images.Image.png");
        public static ImageInfo ImageRotate0 => GetEmbeddedResourceImage("Images.ImageRotate0.png");
        public static ImageInfo ImageRotate180 => GetEmbeddedResourceImage("Images.ImageRotate180.png");
        public static ImageInfo ImageRotate270 => GetEmbeddedResourceImage("Images.ImageRotate270.png");
        public static ImageInfo ImageRotate90 => GetEmbeddedResourceImage("Images.ImageRotate90.png");

        public static ImageInfo BarcodeEan13Rotate0 => GetEmbeddedResourceImage("Images.BarcodeEan13Rotate0.png");
        public static ImageInfo BarcodeEan13Rotate180 => GetEmbeddedResourceImage("Images.BarcodeEan13Rotate180.png");
        public static ImageInfo BarcodeEan13Rotate270 => GetEmbeddedResourceImage("Images.BarcodeEan13Rotate270.png");
        public static ImageInfo BarcodeEan13Rotate90 => GetEmbeddedResourceImage("Images.BarcodeEan13Rotate90.png");

        public static ImageInfo BarcodeQrRotate0 => GetEmbeddedResourceImage("Images.BarcodeQrRotate0.png");
        public static ImageInfo BarcodeQrRotate180 => GetEmbeddedResourceImage("Images.BarcodeQrRotate180.png");
        public static ImageInfo BarcodeQrRotate270 => GetEmbeddedResourceImage("Images.BarcodeQrRotate270.png");
        public static ImageInfo BarcodeQrRotate90 => GetEmbeddedResourceImage("Images.BarcodeQrRotate90.png");

        public static ImageInfo BarcodeQrErrorCorrectionHigh => GetEmbeddedResourceImage("Images.BarcodeQrErrorCorrectionHigh.png");
        public static ImageInfo BarcodeQrErrorCorrectionLow => GetEmbeddedResourceImage("Images.BarcodeQrErrorCorrectionLow.png");
        public static ImageInfo BarcodeQrErrorCorrectionMedium => GetEmbeddedResourceImage("Images.BarcodeQrErrorCorrectionMedium.png");
        public static ImageInfo BarcodeQrErrorCorrectionQuartile => GetEmbeddedResourceImage("Images.BarcodeQrErrorCorrectionQuartile.png");


        public static ImageInfo GetEmbeddedResourceImage(string resourceName)
        {
            var imageInfo = new ImageInfo();

            var resource = GetEmbeddedResource(resourceName);

            if (resource != null)
            {
                using (var image = SixLabors.ImageSharp.Image.Load<Rgba32>(resource))
                {
                    imageInfo.Width = image.Width;
                    imageInfo.Height = image.Height;

                    using (var imageStream = new MemoryStream())
                    {
                        image.SaveAsPng(imageStream);
                        imageStream.Seek(0, SeekOrigin.Begin);
                        imageInfo.Data = imageStream.ToArray();
                    }
                }
            }

            return imageInfo;
        }


        public static string GetEmbeddedResourceText(string resourceName)
        {
            var resource = GetEmbeddedResource(resourceName);

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


        public static Stream GetEmbeddedResource(string resourceName)
        {
            var assembly = typeof(ResourceHelper).GetTypeInfo().Assembly;

            var fullResourceName = assembly.GetManifestResourceNames().FirstOrDefault(i => i.EndsWith(resourceName));

            var resourceStream = assembly.GetManifestResourceStream(fullResourceName);

            return resourceStream;
        }
    }
}
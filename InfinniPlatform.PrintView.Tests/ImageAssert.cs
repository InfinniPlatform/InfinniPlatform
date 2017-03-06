using InfinniPlatform.PrintView.Model.Inline;

using NUnit.Framework;

namespace InfinniPlatform.PrintView.Tests
{
    internal static class ImageAssert
    {
        public static void AreEqual(ImageInfo expected, PrintImage actual)
        {
            CollectionAssert.AreEqual(expected.Data, actual.Data);
        }
    }
}
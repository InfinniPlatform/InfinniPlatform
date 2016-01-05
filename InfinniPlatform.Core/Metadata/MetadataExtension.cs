using System;

namespace InfinniPlatform.Core.Metadata
{
    public static class MetadataExtension
    {
        public static string GetMetadataIndex(string metadataTypeFrom)
        {
            return metadataTypeFrom.ToLowerInvariant() != "metadata" &&
                   metadataTypeFrom.ToLowerInvariant() != "configuration"
                ? String.Format("{0}{1}", metadataTypeFrom, "metadata")
                : "metadata";
        }
    }
}
using System.Collections.Generic;
using System.Linq;

namespace InfinniPlatform.Api.Schema
{
    public sealed class LinkEntry
    {
        public string ConfigId { get; set; }
        public string DocumentId { get; set; }
        public dynamic Schema { get; set; }
    }

    public static class LinkEntryExtensions
    {
        public static bool HasEntry(this IEnumerable<LinkEntry> entries, LinkEntry linkEntry)
        {
            return entries.Any(e => e.ConfigId == linkEntry.ConfigId && e.DocumentId == linkEntry.DocumentId);
        }
    }
}
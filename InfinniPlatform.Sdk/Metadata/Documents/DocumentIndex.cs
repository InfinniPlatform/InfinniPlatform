using System;
using System.Collections.Generic;

namespace InfinniPlatform.Sdk.Metadata.Documents
{
    /// <summary>
    /// Индекс документа.
    /// </summary>
    public sealed class DocumentIndex : IEquatable<DocumentIndex>
    {
        /// <summary>
        /// Имя индекса.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Уникальный индекс.
        /// </summary>
        public bool Unique { get; set; }

        /// <summary>
        /// Ключ индекса документа.
        /// </summary>
        public IDictionary<string, DocumentIndexKeyType> Key { get; set; }

        public bool Equals(DocumentIndex other)
        {
            if (ReferenceEquals(this, other))
            {
                return true;
            }

            if (other != null && Unique == other.Unique)
            {
                if (ReferenceEquals(Key, other.Key) || ((Key == null || Key.Count == 0) && (other.Key == null || other.Key.Count == 0)))
                {
                    return true;
                }

                if (Key != null && other.Key != null && Key.Count == other.Key.Count)
                {
                    var keysAreEqual = true;

                    foreach (var item in Key)
                    {
                        DocumentIndexKeyType otherKeyType;

                        if (!other.Key.TryGetValue(item.Key, out otherKeyType) || item.Value != otherKeyType)
                        {
                            keysAreEqual = false;
                            break;
                        }
                    }

                    return keysAreEqual;
                }
            }

            return false;
        }
    }
}
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
        /// Время существования документа.
        /// </summary>
        public TimeSpan? ExpireAfter { get; set; }

        /// <summary>
        /// Ключ индекса документа.
        /// </summary>
        public IDictionary<string, DocumentIndexKeyType> Key { get; set; }


        /// <summary>
        /// Возвращает имя индекса по умолчанию.
        /// </summary>
        public string GetDefaultName()
        {
            if (Key != null && Key.Count > 0)
            {
                var indexName = string.Empty;

                foreach (var item in Key)
                {
                    var indexSuffix = string.Empty;

                    switch (item.Value)
                    {
                        case DocumentIndexKeyType.Asc:
                            indexSuffix = "1";
                            break;
                        case DocumentIndexKeyType.Desc:
                            indexSuffix = "-1";
                            break;
                        case DocumentIndexKeyType.Text:
                            indexSuffix = "text";
                            break;
                        case DocumentIndexKeyType.Ttl:
                            indexSuffix = "1";
                            break;
                    }

                    indexName += "_" + item.Key + "_" + indexSuffix;
                }

                return indexName.Substring(1);
            }

            return null;
        }


        public bool Equals(DocumentIndex other)
        {
            if (ReferenceEquals(this, other))
            {
                return true;
            }

            if (other != null && Unique == other.Unique)
            {
                var name = string.IsNullOrEmpty(Name) ? GetDefaultName() : Name;
                var otherName = string.IsNullOrEmpty(other.Name) ? other.GetDefaultName() : other.Name;

                return string.Equals(name, otherName);
            }

            return false;
        }


        public override int GetHashCode()
        {
            return 0;
        }
    }
}
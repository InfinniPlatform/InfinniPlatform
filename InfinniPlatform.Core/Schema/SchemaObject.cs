namespace InfinniPlatform.Core.Schema
{
    public enum PathResolveType
    {
        Select,
        Where
    }


    public sealed class SchemaObject
    {
        public SchemaObject(SchemaObject parentInfo, string name, string caption, dynamic value, dynamic objectSchema)
        {
            Parent = parentInfo;
            Name = name;
            Value = value;
            Caption = caption;
            ObjectSchema = objectSchema;
        }

        public string Caption { get; set; }

        public string ParentPath
        {
            get { return Parent != null ? Parent.GetFullPath() : ""; }
        }

        public string Name { get; set; }
        public dynamic Value { get; set; }

        /// <summary>
        ///     Признак того, что метаданные объекта являются встроенными (Inline)
        /// </summary>
        public bool Inline
        {
            get
            {
                return Value.TypeInfo != null && Value.TypeInfo.DocumentLink != null &&
                       Value.TypeInfo.DocumentLink.Inline != null &&
                       Value.TypeInfo.DocumentLink.Inline == true;
            }
        }

        /// <summary>
        ///     Признак метаданных ссылки на документ
        /// </summary>
        public bool IsDocumentLink
        {
            get { return Value != null && Value.TypeInfo != null && Value.TypeInfo.DocumentLink != null; }
        }

        public bool IsArray
        {
            get { return Value != null && Value.Type == "Array"; }
        }

        public bool IsPrimitive
        {
            get { return !IsArray && !IsDocumentLink; }
        }

        public bool IsResolve
        {
            get { return IsDocumentLink && Value.TypeInfo.DocumentLink.Resolve == true; }
        }

        public bool IsDocumentArray
        {
            get
            {
                return Value != null && Value.Items != null && Value.Items.TypeInfo != null &&
                       Value.Items.TypeInfo.DocumentLink != null;
            }
        }

        public string ConfigId
        {
            get
            {
                return Value != null && Value.TypeInfo != null && Value.TypeInfo.DocumentLink != null
                    ? Value.TypeInfo.DocumentLink.ConfigId
                    : null;
            }
        }

        public string DocumentId
        {
            get
            {
                return Value != null && Value.TypeInfo != null && Value.TypeInfo.DocumentLink != null
                    ? Value.TypeInfo.DocumentLink.DocumentId
                    : null;
            }
        }

        public string Type
        {
            get { return Value != null ? Value.Type : string.Empty; }
        }

        public SchemaObject Parent { get; private set; }
        public dynamic ObjectSchema { get; private set; }

        public string ArrayItemType
        {
            get { return IsArray ? Value.Items.Type : string.Empty; }
        }

        public string GetFullPath()
        {
            if (Value != null && Value.Type == "Array")
            {
                if (string.IsNullOrEmpty(ParentPath))
                {
                    return Name + ".$";
                }
                return ParentPath + "." + Name + ".$";
            }

            if (string.IsNullOrEmpty(ParentPath))
            {
                return Name;
            }
            return ParentPath + "." + Name;
        }
    }
}
namespace Digizuite.Models.Metadata.Fields
{
    public abstract record Field
    {
        public int FieldItemId { get; set; }
        public int MetafieldId { get; set; }
        internal int LabelId { get; set; }
        public bool ReadOnly { get; set; }
        public bool Required { get; set; }
        public int SortIndex { get; set; }
        public AutotranslateSetting AutoTranslate { get; set; }
        public int VisibilityMetaFieldId { get; set; }
        public string VisibilityRegex { get; set; } = "";
        public bool System { get; set; }
        public int TargetItemId { get; set; }
        public AssetType RestrictToAssetType { get; set; } = AssetType.All;
        public string UploadTagName { get; set; } = "";
        public abstract string? ToSingleString(string separator);
    }

    public abstract record Field<T> : Field
    {
        public T Value { get; set; } = default!;
    }
}
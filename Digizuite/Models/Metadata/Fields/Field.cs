namespace Digizuite.Models.Metadata.Fields
{
    public abstract record Field
    {
        public int FieldItemId { get; set; }
        public int MetafieldId { get; set; }
        public int LabelId { get; set; }
        public string Label { get; set; } = null!;
        public int LanguageId { get; set; }
        public bool ReadOnly { get; set; }
        public bool Required { get; set; }
        public int SortIndex { get; set; }
        public bool AutoTranslated { get; set; }
        public bool AutoTranslateOverwriteExisting { get; set; }
        public bool Audited { get; set; }
        public int VisibilityMetaFieldId { get; set; }
        public string VisibilityRegex { get; set; } = "";

        public abstract string? ToSingleString(string separator);
    }

    public abstract record Field<T> : Field
    {
        public T Value { get; set; } = default!;
    }
}
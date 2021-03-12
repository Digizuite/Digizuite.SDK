namespace Digizuite.Models.Metadata
{
    public class MetaField
    {
        public int MetaFieldId { get; set; } = default!;
        public int MetaFieldItemId { get; set; } = default!;
        public string MetaFieldGuid { get; set; } = default!;
        public int MetaFieldLabelId { get; set; } = default!;

        public string Name { get; set; } = default!;
        public MetaFieldDataType Type { get; set; } = default!;

        public int GroupId { get; set; } = default!;
    }
}
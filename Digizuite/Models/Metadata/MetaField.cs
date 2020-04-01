namespace Digizuite.Models.Metadata
{
    public class MetaField
    {
        public int MetaFieldId { get; set; }
        public int MetaFieldItemId { get; set; }
        public string MetaFieldGuid { get; set; }
        public int MetaFieldLabelId { get; set; }

        public string Name { get; set; }
        public MetaFieldDataType Type { get; set; }

        public int GroupId { get; set; }
    }
}
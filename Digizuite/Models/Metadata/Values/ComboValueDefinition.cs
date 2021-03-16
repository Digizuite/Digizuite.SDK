namespace Digizuite.Models.Metadata.Values
{
    public record ComboValueDefinition
    {
        public int Id { get; set; }
        public int ItemId { get; set; }
        public string Text { get; set; } = default!;
        public string Value { get; set; } = default!;
        public int SortIndex { get; set; }
        public bool IsPublic { get; set; } = true;
        public bool Visible { get; set; } = true;
    }
}
namespace Digizuite.Models.Metadata.Values
{
    public class ComboValueDefinition
    {
        public int Id { get; set; }
        public int ItemId { get; set; }
        public string Text { get; set; }
        public string Value { get; set; }
        public int SortIndex { get; set; }
        public bool IsPublic { get; set; } = true;
        public bool Visible { get; set; } = true;
    }
}
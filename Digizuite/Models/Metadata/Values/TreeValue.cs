namespace Digizuite.Models.Metadata.Values
{
    public record TreeValue : TreeValueReference
    {
        public int Id { get; set; } = default!;
        public string Label { get; set; } = default!;
    }
}
namespace Digizuite.Models.Metadata.Values
{
    public record ItemReferenceOption : ItemReferenceOptionReference
    {
        public int BaseId { get; set; } = default!;
        public string Label { get; set; } = default!;
    }
}
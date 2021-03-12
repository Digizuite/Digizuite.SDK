namespace Digizuite.BatchUpdate.Models
{
    public class BatchValueJsonValue
    {
        public string FieldId { get; set; } = null!;
        public ValueType Type { get; set; }
        public object Values { get; set; } = null!;
    }
}
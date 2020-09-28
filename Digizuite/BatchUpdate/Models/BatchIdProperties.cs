namespace Digizuite.BatchUpdate.Models
{
    public class BatchIdProperties : IBatchProperties
    {
        public int Id { get; }

        public BatchIdProperties(int id)
        {
            Id = id;
        }

        public string ToUpdateKey()
        {
            return $"id=\"{Id}\"";
        }
    }
}
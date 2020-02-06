namespace Digizuite.BatchUpdate.Models
{
    public class BatchNoProperties : IBatchProperties
    {
        public static readonly BatchNoProperties Value = new BatchNoProperties();

        private BatchNoProperties()
        {
        }
    }
}
namespace Digizuite.BatchUpdate.Models
{
    public class BatchLabelIdProperties : IBatchProperties
    {
        public int LabelId { get; }

        public BatchLabelIdProperties(int labelId)
        {
            LabelId = labelId;
        }
    }
}
namespace Digizuite.BatchUpdate.Models
{
    public class BatchLabelIdProperties : IBatchProperties
    {
        public int LabelId;

        public BatchLabelIdProperties(int labelId)
        {
            LabelId = labelId;
        }
    }
}
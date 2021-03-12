namespace Digizuite.BatchUpdate.Models
{
    public class DeleteBatchValue : BatchValue
    {
        public DeleteBatchValue(string fieldName, IBatchProperties? properties) : base(fieldName, null,
            properties)
        {
        }

        public override ValueType ValueType => ValueType.Delete;
    }
}
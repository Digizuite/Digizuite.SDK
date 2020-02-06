namespace Digizuite.BatchUpdate.Models
{
    public class DeleteBatchValue : BatchValue
    {
        public DeleteBatchValue(FieldType fieldName, IBatchProperties properties) : base(fieldName, null,
            properties)
        {
        }

        public override ValueType ValueType => ValueType.Delete;
    }
}
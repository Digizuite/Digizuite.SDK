namespace Digizuite.BatchUpdate.Models
{
    public class IntBatchValue : BatchValue
    {
        public IntBatchValue(FieldType fieldName, int value, IBatchProperties properties) : base(fieldName, value,
            properties)
        {
        }

        public override ValueType ValueType => ValueType.Int;
    }
}
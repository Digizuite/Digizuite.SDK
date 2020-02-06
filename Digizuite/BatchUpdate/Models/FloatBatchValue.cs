namespace Digizuite.BatchUpdate.Models
{
    public class FloatBatchValue : BatchValue
    {
        public FloatBatchValue(FieldType fieldName, double value, IBatchProperties properties) : base(fieldName,
            value, properties)
        {
        }

        public override ValueType ValueType => ValueType.Float;
    }
}
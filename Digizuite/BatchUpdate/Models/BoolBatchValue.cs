namespace Digizuite.BatchUpdate.Models
{
    public class BoolBatchValue : BatchValue
    {
        public BoolBatchValue(string fieldName, bool value, IBatchProperties properties) : base(fieldName, value,
            properties)
        {
        }

        public override ValueType ValueType => ValueType.Bool;
    }
}
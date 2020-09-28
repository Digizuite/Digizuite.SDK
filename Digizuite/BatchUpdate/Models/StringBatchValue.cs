namespace Digizuite.BatchUpdate.Models
{
    public class StringBatchValue : BatchValue
    {
        public StringBatchValue(string fieldName, string value, IBatchProperties properties) : base(fieldName,
            value, properties)
        {
        }

        public override ValueType ValueType => ValueType.String;
    }
}
namespace Digizuite.BatchUpdate.Models
{
    public abstract class BatchValue
    {
        public string FieldName { get; set; }

        public IBatchProperties? Properties { get; set; }
        public object? Value { get; set; }

        protected BatchValue(string fieldName, object? value, IBatchProperties? properties)
        {
            FieldName = fieldName;
            Value = value;
            Properties = properties;
        }

        public abstract ValueType ValueType { get; }
    }
}
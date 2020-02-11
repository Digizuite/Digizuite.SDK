using Newtonsoft.Json;

namespace Digizuite.BatchUpdate.Models
{
    public abstract class BatchValue
    {
        [JsonConverter(typeof(FieldTypeJsonConverter))]
        public FieldType FieldName { get; set; }

        public IBatchProperties Properties { get; set; }
        public object Value { get; set; }

        protected BatchValue(FieldType fieldName, object value, IBatchProperties properties)
        {
            FieldName = fieldName;
            Value = value;
            Properties = properties;
        }

        public abstract ValueType ValueType { get; }
    }
}
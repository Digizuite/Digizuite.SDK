using System;

namespace Digizuite.BatchUpdate.Models
{
    public class ValueExtraValueBatchValue : BatchValue
    {
        public ValueExtraValueBatchValue(FieldType fieldName, Tuple<string, string> value,
            IBatchProperties properties) : base(fieldName, value, properties)
        {
        }

        public override ValueType ValueType => ValueType.ValueExtraValue;
    }
}
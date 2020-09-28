using System;

namespace Digizuite.BatchUpdate.Models
{
    public class DateTimeBatchValue : BatchValue
    {
        public DateTimeBatchValue(string fieldName, DateTime value, IBatchProperties properties) : base(
            fieldName, value, properties)
        {
        }

        public override ValueType ValueType => ValueType.DateTime;
    }
}
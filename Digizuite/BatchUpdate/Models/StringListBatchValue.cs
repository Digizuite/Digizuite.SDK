using System.Collections.Generic;

namespace Digizuite.BatchUpdate.Models
{
    public class StringListBatchValue : BatchValue
    {
        public StringListBatchValue(string fieldName, List<string> value, IBatchProperties? properties) : base(
            fieldName, value, properties)
        {
        }

        public override ValueType ValueType => ValueType.StringList;
    }
}
using System.Collections.Generic;

namespace Digizuite.BatchUpdate.Models
{
    public class IntListBatchValue : BatchValue
    {
        public IntListBatchValue(string fieldName, List<int> value, IBatchProperties? properties) : base(
            fieldName, value, properties)
        {
        }

        public override ValueType ValueType => ValueType.IntList;
    }
}
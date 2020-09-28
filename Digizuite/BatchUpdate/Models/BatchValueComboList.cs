using System.Collections.Generic;
using Digizuite.Metadata;

namespace Digizuite.BatchUpdate.Models
{
    public class BatchValueComboList : BatchValue
    {
        public bool AllowCreate { get; set; }
        public bool AddOnly { get; set; }
        
        public BatchValueComboList(FieldType fieldName, List<ComboCreateValue> value, IBatchProperties properties) : base(fieldName, value, properties)
        {
        }

        public override ValueType ValueType => ValueType.StringList;

        public override object ToJsonValue(string fieldId, object actualValues)
        {
            return new ValueBatchComboListJsonValue
            {
                AllowCreate = AllowCreate,
                AddOnly = AddOnly,
                FieldId = fieldId,
                Values = actualValues,
                Type = ValueType
            };
        }

        private class ValueBatchComboListJsonValue : BatchValueJsonValue
        {
            public bool AllowCreate { get; set; }
            public bool AddOnly { get; set; }
        }
    }

    public class ComboCreateValue
    {
        public string ComboValue { get; set; }
        public string OptionValue { get; set; }
        public ExistsCheck ExistsCheck { get; set; }
        public bool IsPublic { get; set; }
        public bool Visible { get; set; }
    }
}
using System.Collections.Generic;
using System.Linq;
using Digizuite.Models.Metadata.Values;

namespace Digizuite.Models.Metadata.Fields
{
    public record EditMultiComboMetafield : Field<List<ComboValue>>
    {
        public MetaFieldDataType Type => MetaFieldDataType.EditMultiComboValue;

        public EditMultiComboMetafield()
        {
            Value = new List<ComboValue>();
        }
        
        public override string ToSingleString(string separator)
        {
            if (Value == null) return "";

            return string.Join(separator, Value.Select(v => v.Label));
        }
    }
}
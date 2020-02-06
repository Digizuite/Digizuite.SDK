using System.Collections.Generic;
using System.Linq;
using Digizuite.Models.Metadata.Values;

namespace Digizuite.Models.Metadata.Fields
{
    public class MasterItemReferenceMetafield : ItemReferenceMetaField<ItemReferenceOption>
    {
        public MetaFieldDataType Type => MetaFieldDataType.MasterItemReference;
        public List<int> RefByMetafieldId { get; set; }
        public List<int> RefByMetafieldLabelId { get; set; }

        public override string ToSingleString(string separator)
        {
            if (Value == null)
            {
                return "";
            }

            return string.Join(separator, Value.Select(v => v.Label));
        }
    }
}
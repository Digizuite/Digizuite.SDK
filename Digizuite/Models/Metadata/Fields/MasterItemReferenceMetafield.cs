using System.Linq;
using Digizuite.Models.Metadata.Values;

namespace Digizuite.Models.Metadata.Fields
{
    public record MasterItemReferenceMetafield : ItemReferenceMetaField<ItemReferenceOption>
    {
        public MetaFieldDataType Type => MetaFieldDataType.MasterItemReference;

        public override string ToSingleString(string separator)
        {
            if (Value == null) return "";

            return string.Join(separator, Value.Select(v => v.Label));
        }
    }
}
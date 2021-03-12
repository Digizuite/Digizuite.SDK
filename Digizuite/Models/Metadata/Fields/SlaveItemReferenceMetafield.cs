using System.Linq;
using Digizuite.Models.Metadata.Values;

namespace Digizuite.Models.Metadata.Fields
{
    public record SlaveItemReferenceMetafield : ItemReferenceMetaField<ItemReferenceOption>
    {
        public MetaFieldDataType Type => MetaFieldDataType.SlaveItemReference;
        public int RefLabelId { get; set; }

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
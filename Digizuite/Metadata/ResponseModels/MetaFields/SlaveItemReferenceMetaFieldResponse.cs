using Digizuite.Models;
using Digizuite.Models.Metadata;

namespace Digizuite.Metadata.ResponseModels.MetaFields
{
    public record SlaveItemReferenceMetaFieldResponse : MetaFieldResponse
    {
        public override MetaFieldDataType Type => MetaFieldDataType.SlaveItemReference;
        public ItemType ItemType { get; set; }
        public int RelatedMetaFieldLabelId { get; set; }
    }
}
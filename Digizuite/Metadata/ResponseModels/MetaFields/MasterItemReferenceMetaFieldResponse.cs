using Digizuite.Models;
using Digizuite.Models.Metadata;

namespace Digizuite.Metadata.ResponseModels.MetaFields
{
    public record MasterItemReferenceMetaFieldResponse : MetaFieldResponse
    {
        public override MetaFieldDataType Type => MetaFieldDataType.MasterItemReference;
        public ItemType ItemType { get; set; }
        public int MaxCount { get; set; }
        public int? RelatedMetaFieldId { get; set; }
    }
}
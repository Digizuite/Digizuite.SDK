using Digizuite.Collections;
using Digizuite.Metadata.ResponseModels.Properties;
using Digizuite.Models.Metadata;

namespace Digizuite.Metadata.ResponseModels.Metadata
{
    public record MasterItemReferenceMetadataResponse(ValueList<ItemReferenceResponseItem> Items) : MetadataResponse
    {
        public override MetaFieldDataType Type => MetaFieldDataType.MasterItemReference;
    }
}
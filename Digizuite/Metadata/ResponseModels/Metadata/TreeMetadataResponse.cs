using Digizuite.Collections;
using Digizuite.Metadata.ResponseModels.Properties;
using Digizuite.Models.Metadata;

namespace Digizuite.Metadata.ResponseModels.Metadata
{
    public record TreeMetadataResponse(ValueList<TreeValueResponse> Values) : MetadataResponse
    {
        public override MetaFieldDataType Type => MetaFieldDataType.Tree;
    }
}
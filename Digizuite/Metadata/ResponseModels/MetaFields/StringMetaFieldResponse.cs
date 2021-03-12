using Digizuite.Models.Metadata;

namespace Digizuite.Metadata.ResponseModels.MetaFields
{
    public record StringMetaFieldResponse : MetaFieldResponse
    {
        public int MaxLength { get; set; }
        public override MetaFieldDataType Type => MetaFieldDataType.String;
    }
}
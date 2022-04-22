using Digizuite.Models.Metadata;

namespace Digizuite.Metadata.ResponseModels.MetaFields
{
    public record TreeMetaFieldResponse : MetaFieldResponse
    {
        public override MetaFieldDataType Type => MetaFieldDataType.Tree;
        public bool SelectToRoot { get; set; }
    }
}
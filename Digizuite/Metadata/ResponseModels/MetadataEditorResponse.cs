using System.Collections.Generic;

namespace Digizuite.Metadata.ResponseModels
{
    public record MetadataEditorResponse
    {
        public List<MetaGroupResponse> Groups { get; set; } = new();
        public List<MetaFieldResponse> Fields { get; set; } = new();
        public List<MetadataResponse> Values { get; set; } = new();
    }

    public record MetaGroupResponse(
        string Name,
        int GroupId,
        int SortIndex,
        bool Iterative
    );
}
namespace Digizuite.Metadata.ResponseModels
{
    public record MetaGroupResponse(
        string Name,
        int GroupId,
        int SortIndex,
        bool Iterative
    );
}
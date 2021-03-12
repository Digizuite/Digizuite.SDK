using Digizuite.Models;

namespace Digizuite.Metadata.ResponseModels.Properties
{
    public record ItemReferenceResponseItem(int ItemId, string Title, int BaseId, ItemType ItemType);
}
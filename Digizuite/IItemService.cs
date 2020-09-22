using System;
using System.Threading.Tasks;
using Digizuite.Models;

namespace Digizuite
{
    public interface IItemService
    {
        Task<ItemInfo> GetItemInfo(Guid itemGuid);
        Task<ItemInfo> GetItemInfo(int itemId);
        Task<ItemInfo> GetItemInfoFromBaseId(ItemType itemType, int baseId);
    }
}

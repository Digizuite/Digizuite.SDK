using System;

namespace Digizuite.Models
{
    public record ItemInfo
    {
        public int BaseId { get; set; }
        public int ItemId { get; set; }
        public ItemType ItemTypeId { get; set; }
        public Guid ItemGuid { get; set; }
    }
}
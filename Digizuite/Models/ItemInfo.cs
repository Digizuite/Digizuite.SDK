using System;
using System.Collections.Generic;
using System.Text;

namespace Digizuite.Models
{
    public class ItemInfo
    {
        public int BaseId { get; set; }
        public int ItemId { get; set; }
        public ItemType ItemTypeId { get; set; }
        public Guid ItemGuid { get; set; }
    }
}

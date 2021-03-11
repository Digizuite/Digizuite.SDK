using System.Collections.Generic;

namespace Digizuite.Models.Metadata.Fields
{
    public abstract class ItemReferenceMetaField<T> : Field<List<T>>
    {
        public int MaxItems { get; set; }
        public ItemType RefType { get; set; }
    }
}
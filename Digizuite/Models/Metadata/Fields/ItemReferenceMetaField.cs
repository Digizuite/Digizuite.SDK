using System.Collections.Generic;

namespace Digizuite.Models.Metadata.Fields
{
    public abstract class ItemReferenceMetaField<T> : Field<List<T>>
    {
        public int MaxItems { get; set; }
        public int RefItemId { get; set; }
        public int RefItemBaseId { get; set; }
        public string RefItemTitle { get; set; }
        public ReferenceItemType RefType { get; set; }
    }
}
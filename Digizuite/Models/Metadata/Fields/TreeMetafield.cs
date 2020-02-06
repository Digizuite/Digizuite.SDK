using System.Collections.Generic;
using System.Linq;
using Digizuite.Models.Metadata.Values;

namespace Digizuite.Models.Metadata.Fields
{
    public class TreeMetafield : Field<List<TreeValue>>
    {
        public MetaFieldDataType Type => MetaFieldDataType.Tree;
        public TreeViewType ViewType { get; set; }
        public bool RecursiveToRoot { get; set; }

        public override string ToSingleString(string separator)
        {
            if (Value == null)
            {
                return "";
            }

            return string.Join(separator, Value.Select(v => v.Label));
        }
    }
}
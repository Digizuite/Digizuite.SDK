using System.Collections.Generic;
using Digizuite.Models.Metadata;

namespace Digizuite.Models
{
    public class RootSystemToolsResponse
    {
        public int ChildCount { get; set; }
        public int FolderId { get; set; }
        public int ItemId { get; set; }
        public CodedFolder SubRepositoryType { get; set; }
        public int UniqueId { get; set; }
        public List<SystemToolsNodeItem> Items { get; set; }
    }
}
using System.Collections.Generic;
using Digizuite.Models.Metadata;

namespace Digizuite.Models
{
    public class RootSystemToolsResponse
    {
        public int ChildCount { get; set; } = default!;
        public int FolderId { get; set; } = default!;
        public int ItemId { get; set; } = default!;
        public CodedFolder SubRepositoryType { get; set; } = default!;
        public int UniqueId { get; set; } = default!;
        public List<SystemToolsNodeItem> Items { get; set; } = default!;
    }
}
using System.Collections.Generic;
using Digizuite.BatchUpdate.Models;
using Digizuite.Models.Metadata;

namespace Digizuite.Models
{
    public class SystemToolsNodeItem
    {
        public int ChildCount { get; set; } = default!;
        public int FolderId { get; set; } = default!;
        public string Hid { get; set; } = default!;
        public int ItemId { get; set; } = default!;
        public List<SystemToolsNodeItem> Items { get; set; } = default!;
        public string Name { get; set; } = default!;
        public RepositoryType RepositoryType { get; set; } = default!;
        public CodedFolder SubRepositoryType { get; set; } = default!;
        public string UniqueId { get; set; } = default!;
        public int MetafieldGroupId { get; set; } = default!;
    }
}
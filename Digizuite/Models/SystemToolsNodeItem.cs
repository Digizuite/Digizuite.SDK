using System.Collections.Generic;
using Digizuite.BatchUpdate.Models;
using Digizuite.Models.Metadata;

namespace Digizuite.Models
{
    public class SystemToolsNodeItem
    {
        public int ChildCount { get; set; }
        public int FolderId { get; set; }
        public string Hid { get; set; }
        public int ItemId { get; set; }
        public List<SystemToolsNodeItem> Items { get; set; }
        public string Name { get; set; }
        public RepositoryType RepositoryType { get; set; }
        public CodedFolder SubRepositoryType { get; set; }
        public string UniqueId { get; set; }
        public int MetafieldGroupId { get; set; }
    }
}
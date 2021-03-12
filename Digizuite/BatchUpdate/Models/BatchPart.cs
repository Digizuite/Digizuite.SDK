using System.Collections.Generic;

namespace Digizuite.BatchUpdate.Models
{
    public class BatchPart
    {
        public List<int> BaseIds { get; set; } = new List<int>();
        public BatchType BatchType { get; set; }
        public string? FieldName { get; set; } = "";
        public List<int> ItemIds { get; set; } = new List<int>();

        public int RowId { get; set; } = 1;

        public RepositoryType RepositoryType { get; set; } = RepositoryType.Default;

        public string Target { get; set; } = null!;

        public List<BatchValue> Values { get; set; } = new List<BatchValue>();

        public bool ForceDelete { get; set; }
    }
}
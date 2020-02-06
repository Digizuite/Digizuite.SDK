using System.Collections.Generic;
using Newtonsoft.Json;

namespace Digizuite.BatchUpdate.Models
{
    public class BatchPart
    {
        public List<int> BaseIds = new List<int>();
        public BatchType BatchType;
        public string FieldName = "";
        public List<int> ItemIds = new List<int>();

        public int RowId = 1;

        [JsonConverter(typeof(FieldTypeJsonConverter))]
        public FieldType Target;

        public List<BatchValue> Values = new List<BatchValue>();

        public bool ForceDelete;

        public int ItemId
        {
            set => ItemIds = new List<int> {value};
        }
    }
}
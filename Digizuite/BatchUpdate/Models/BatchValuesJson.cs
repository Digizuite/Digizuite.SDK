using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Digizuite.BatchUpdate.Models
{
    public class BatchValuesJson
    {
        public string Id { get; set; } = null!;
        public string FieldId { get; set; } = null!;
        public BatchType ContainerType { get; set; }
        public int RowId { get; set; }
        public List<int>? ItemIds { get; set; }
        public RepositoryType RepositoryType { get; set; }
        public bool? ForceDelete { get; set; }

        public int? ItemId
        {
            get
            {
                if (ItemIds != null && ItemIds.Count == 1)
                {
                    return ItemIds[0];
                }
                else
                {
                    return null;
                }
            }
        }

        public List<int>? BaseIds { get; set; }

        public int? BaseId
        {
            get
            {
                if (BaseIds != null && BaseIds.Count == 1)
                {
                    return BaseIds[0];
                }
                else
                {
                    return null;
                }
            }
        }

        public List<BatchValueJsonValue> Values { get; set; } = null!;
        [JsonPropertyName("fieldName")] public string FieldName { get; set; } = null!;
    }
}
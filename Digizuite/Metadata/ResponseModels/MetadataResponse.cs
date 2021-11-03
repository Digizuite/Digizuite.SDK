using System.Text.Json.Serialization;
using Digizuite.Metadata.ResponseModels.Helpers;
using Digizuite.Models.Metadata;

namespace Digizuite.Metadata.ResponseModels
{
    [JsonConverter(typeof(MetadataResponseConverter))]
    public abstract record MetadataResponse
    {
        public int ItemId { get; set; }

        public int LabelId { get; set; }

        // public int RowId { get; set; }
        public abstract MetaFieldDataType Type { get; }
    }
}
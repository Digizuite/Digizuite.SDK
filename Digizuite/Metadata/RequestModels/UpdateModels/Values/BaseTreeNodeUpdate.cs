using System.Text.Json.Serialization;
using Digizuite.Metadata.RequestModels.UpdateModels.Helpers;

namespace Digizuite.Metadata.RequestModels.UpdateModels.Values
{
    [JsonConverter(typeof(InputTreeNodeConverter))]
    public abstract record BaseTreeNodeUpdate
    {
        /// <summary>
        /// If this value should actually be selected, or if it just for reference as a parent
        /// </summary>
        public bool Select { get; set; } = true;
    }
}

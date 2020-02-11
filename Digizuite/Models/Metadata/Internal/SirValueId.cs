using Newtonsoft.Json;

namespace Digizuite.Models.Metadata.Internal
{
    internal class SirValueId
    {
        [JsonProperty("sir_valueid")]
        public int ValueId { get; set; }
        [JsonProperty("sirItemItemId")]
        public int ItemId { get; set; }
        [JsonProperty("sirItemBaseId")]
        public int BaseId { get; set; }
        [JsonProperty("sirItemTitle")]
        public string Title { get; set; }
    }
}
using System.Collections.Generic;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
// ReSharper disable InconsistentNaming

namespace Digizuite.Models.Metadata.Internal
{
    internal class MetaFieldResponse
    {
        public string ItemId { get; set; }
        public int MetafieldLabelId { get; set; }
        public string MetafieldLabellabel { get; set; }
        public int MetafieldLabellanguageid { get; set; }
        public int MetafieldLabelSortindex { get; set; }
        public MetaFieldLabelResponse MetafieldLabel { get; set; }
        public MetaFieldIdResponse MetafieldId { get; set; }
        public string MetafieldReferenceSelectMode { get; set; }
        public string MetafieldReferenceMaxItems { get; set; }
        public string MetafieldReferenceMetafieldLabelId { get; set; }
        public string ReferenceTypeId { get; set; }
        public string[] MetafieldReferencedMetafieldId { get; set; }
        public string[] MetafieldReferencedMetafieldLabelId { get; set; }
        
        [JsonProperty("sir_valueid")]
        public object SirValueId { get; set; }

        public object Note { get; set; }
        public object Item_metafield_valueid { get; set; }

        public NoteValue GetNoteValue()
        {
            if (Note == null || string.IsNullOrWhiteSpace(Note.ToString())) return null;

            var list = ((JArray)Note).ToObject<List<NoteValue>>();

            if (list.Count == 1) return list[0];

            return null;
        }

        public bool HasNoteValue()
        {
            var noteValue = GetNoteValue();

            return noteValue != null;
        }

        public ItemMetafieldValueIdResponse GetSingleMetaFieldValueId()
        {
            var value = GetMetaFieldValueId();

            return value.Count == 1 ? value[0] : null;
        }

        public List<ItemMetafieldValueIdResponse> GetMetaFieldValueId()
        {
            if (Item_metafield_valueid == null) return new List<ItemMetafieldValueIdResponse>();

            if (string.IsNullOrWhiteSpace(Item_metafield_valueid.ToString()))
                return new List<ItemMetafieldValueIdResponse>();

            return ((JArray)Item_metafield_valueid).ToObject<List<ItemMetafieldValueIdResponse>>();
        }

        public List<SirValueId> GetSirValueId()
        {
            if(SirValueId == null) return new List<SirValueId>();
            
            if(string.IsNullOrWhiteSpace(SirValueId.ToString())) return new List<SirValueId>();

            return ((JArray) SirValueId).ToObject<List<SirValueId>>();
        }

        public bool HasItemMetaFieldValueId()
        {
            return GetMetaFieldValueId().Count != 0;
        }
    }
}

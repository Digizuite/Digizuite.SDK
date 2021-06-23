using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;
using Digizuite.Metadata.RequestModels.UpdateModels.Helpers;

namespace Digizuite.Metadata.RequestModels.UpdateModels
{
    [JsonConverter(typeof(MetadataUpdateConverter))]
    public abstract record MetadataUpdate
    {
        /// <summary>
        /// The items to update with this update
        /// </summary>
        public HashSet<int> TargetItemIds { get; set; } = new();

        /// <summary>
        /// The labelId of the metafield to update
        /// </summary>
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        public int MetaFieldLabelId { get; set; }
        
        /// <summary>
        /// The ItemGuid of the metafield to update
        /// </summary>
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        public Guid MetaFieldItemGuid { get; set; }

        /// <summary>
        /// If true, normal auto translate behavior is skipped. You probably don't want to set this
        /// field.
        /// </summary>
        public bool SkipAutoTranslateBehavior { get; set; }

        public virtual bool Equals(MetadataUpdate? other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return  MetaFieldLabelId == other.MetaFieldLabelId
                   && SkipAutoTranslateBehavior == other.SkipAutoTranslateBehavior
                   && TargetItemIds.SetEquals(other.TargetItemIds);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                var hashCode = TargetItemIds.GetHashCode();
                hashCode = (hashCode * 397) ^ MetaFieldLabelId;
                hashCode = (hashCode * 397) ^ SkipAutoTranslateBehavior.GetHashCode();
                return hashCode;
            }
        }
    }
}
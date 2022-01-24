using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;
using Digizuite.Metadata.ResponseModels.Helpers;
using Digizuite.Models.Metadata;

namespace Digizuite.Metadata.ResponseModels
{
    [JsonConverter(typeof(MetaFieldResponseConverter))]
    public abstract record MetaFieldResponse
    {
        public int MetafieldId { get; set; } = default;
        public int SortIndex { get; set; } = default;
        public bool Required { get; set; } = default;
        public bool Readonly { get; set; } = default;
        public bool AutoTranslated { get; set; } = default;
        public bool AutoTranslatedOverwriteExisting { get; set; } = default;
        public bool AiTranslate { get; set; } = default;
        public int VisibilityMetaFieldId { get; set; } = default;
        public string VisibilityRegex { get; set; } = "";
        public int GroupId { get; set; } = default;
        public Guid Guid { get; set; } = default;
        public int ItemId { get; set; } = default;
        public int RestrictToItemType { get; set; } = default;
        public bool Audited { get; set; }
        public abstract MetaFieldDataType Type { get; }

        [Obsolete]
        public int LabelId { get; set; } = default;
        
        [Obsolete]
        public int LanguageId { get; set; } = default;
        
        [Obsolete]
        public string Label { get; set; } = "";

        public Dictionary<int, MetaFieldLabelResponse> Labels { get; set; } = new();
    }
}

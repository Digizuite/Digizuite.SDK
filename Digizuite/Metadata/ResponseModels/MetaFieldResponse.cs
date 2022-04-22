using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;
using Digizuite.Metadata.ResponseModels.Helpers;
using Digizuite.Models;
using Digizuite.Models.Metadata;

namespace Digizuite.Metadata.ResponseModels
{
    [JsonConverter(typeof(MetaFieldResponseConverter))]
    public abstract record MetaFieldResponse
    {
        public int ItemId { get; set; } = default;
        public int MetafieldId { get; set; } = default;
        public Guid Guid { get; set; } = default;
        public abstract MetaFieldDataType Type { get; }
        public string Name { get; set; } = "";
        public string Description { get; set; } = "";
        public int SortIndex { get; set; } = default;
        public int VisibilityMetaFieldId { get; set; } = default;
        public string VisibilityRegex { get; set; } = "";
        public bool Required { get; set; } = default;
        public bool Readonly { get; set; } = default;
        public bool ShowInList { get; set; } = default;
        public bool System { get; set; } = default;
        public AutotranslateSetting AutoTranslate { get; set; }
        public int GroupId { get; set; } = default;
        public AssetType RestrictToAssetType { get; set; } = AssetType.All;
        public string UploadTagName { get; set; } = "";

        [Obsolete]
        public int LabelId { get; set; } = default;
        
        [Obsolete]
        public int LanguageId { get; set; } = default;
        
        [Obsolete]
        public string Label { get; set; } = "";

        public Dictionary<int, MetaFieldLabelResponse> Labels { get; set; } = new();
    }
}

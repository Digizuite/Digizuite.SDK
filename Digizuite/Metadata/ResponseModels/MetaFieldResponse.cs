using System;
using System.Text.Json.Serialization;
using Digizuite.Metadata.ResponseModels.Helpers;
using Digizuite.Models;
using Digizuite.Models.Metadata;

namespace Digizuite.Metadata.ResponseModels
{
    [JsonConverter(typeof(MetaFieldResponseConverter))]
    public abstract record MetaFieldResponse
    {
        public int MetafieldId { get; set; } = default;
        public int LabelId { get; set; } = default;
        public int LanguageId { get; set; } = default;
        public int SortIndex { get; set; } = default;
        public string Label { get; set; } = "";
        public bool Required { get; set; } = default;
        public bool Readonly { get; set; } = default;
        public bool AutoTranslated { get; set; } = default;
        public bool AutoTranslatedOverwriteExisting { get; set; } = default;
        public int VisibilityMetaFieldId { get; set; } = default;
        public string VisibilityRegex { get; set; } = "";
        public int GroupId { get; set; } = default;
        public Guid Guid { get; set; } = default;
        public int ItemId { get; set; } = default;
        public int RestrictToItemType { get; set; } = default;
        public bool Audited { get; set; }
        public abstract MetaFieldDataType Type { get; }
    }

    public record IntMetaFieldResponse : MetaFieldResponse
    {
        public override MetaFieldDataType Type => MetaFieldDataType.Int;
    }

    public record StringMetaFieldResponse : MetaFieldResponse
    {
        public int MaxLength { get; set; }
        public override MetaFieldDataType Type => MetaFieldDataType.String;
    }

    public record BitMetaFieldResponse : MetaFieldResponse
    {
        public override MetaFieldDataType Type => MetaFieldDataType.Bit;
    }

    public enum DateTimeViewType
    {
        Date,
        DateTime
    }

    public record DateTimeMetaFieldResponse : MetaFieldResponse
    {
        public override MetaFieldDataType Type => MetaFieldDataType.DateTime;
        public DateTimeViewType ViewType { get; set; }
    }


    public enum ComboValueViewType
    {
        Combo,
        Radio
    }

    public record ComboValueMetaFieldResponse : MetaFieldResponse
    {
        public ComboValueViewType ViewType { get; set; }
        public override MetaFieldDataType Type => MetaFieldDataType.ComboValue;
    }

    public record EditComboValueMetaFieldResponse : MetaFieldResponse
    {
        public override MetaFieldDataType Type => MetaFieldDataType.EditComboValue;
    }

    public record NoteMetaFieldResponse : MetaFieldResponse
    {
        public override MetaFieldDataType Type => MetaFieldDataType.Note;
        public bool ShowRichTextEditor { get; set; }
    }


    public record MultiComboValueMetaFieldResponse : MetaFieldResponse
    {
        public override MetaFieldDataType Type => MetaFieldDataType.MultiComboValue;
    }

    public record MasterItemReferenceMetaFieldResponse : MetaFieldResponse
    {
        public override MetaFieldDataType Type => MetaFieldDataType.MasterItemReference;
        public ItemType ItemType { get; set; }
        public int MaxCount { get; set; }
        public int? RelatedMetaFieldLabelId { get; set; }
    }

    public record SlaveItemReferenceMetaFieldResponse : MetaFieldResponse
    {
        public override MetaFieldDataType Type => MetaFieldDataType.SlaveItemReference;
        public ItemType ItemType { get; set; }
        public int RelatedMetaFieldLabelId { get; set; }
    }

    public record FloatMetaFieldResponse : MetaFieldResponse
    {
        public override MetaFieldDataType Type => MetaFieldDataType.Float;
    }

    public record EditMultiComboValueMetaFieldResponse : MetaFieldResponse
    {
        public override MetaFieldDataType Type => MetaFieldDataType.EditMultiComboValue;
    }

    public record TreeMetaFieldResponse : MetaFieldResponse
    {
        public override MetaFieldDataType Type => MetaFieldDataType.Tree;
        public bool RecursiveToRoot { get; set; }
    }

    public record LinkMetaFieldResponse : MetaFieldResponse
    {
        public override MetaFieldDataType Type => MetaFieldDataType.Link;
    }
}

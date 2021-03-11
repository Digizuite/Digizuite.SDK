using System;
using System.Text.Json.Serialization;
using Digizuite.Collections;
using Digizuite.Metadata.ResponseModels.Helpers;
using Digizuite.Models;
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

    public record IntMetadataResponse(int? Value) : MetadataResponse
    {
        public override MetaFieldDataType Type => MetaFieldDataType.Int;

        public IntMetadataResponse() : this(0)
        {
        }
    }

    public record StringMetadataResponse(string Value) : MetadataResponse
    {
        public override MetaFieldDataType Type => MetaFieldDataType.String;
    }

    public record BitMetadataResponse(bool Value) : MetadataResponse
    {
        public override MetaFieldDataType Type => MetaFieldDataType.Bit;
    }

    public record DateTimeMetadataResponse(DateTime? Value) : MetadataResponse
    {
        public override MetaFieldDataType Type => MetaFieldDataType.DateTime;
    }

    public record ComboValue(int Id, string Label, string OptionValue);

    public record MultiComboValueMetadataResponse(ValueList<ComboValue> Values) : MetadataResponse
    {
        public override MetaFieldDataType Type => MetaFieldDataType.MultiComboValue;
    }

    public record ComboValueMetadataResponse(ComboValue? Value) : MetadataResponse
    {
        public override MetaFieldDataType Type => MetaFieldDataType.ComboValue;
    }

    public record EditComboValueMetadataResponse(ComboValue? Value) : MetadataResponse
    {
        public override MetaFieldDataType Type => MetaFieldDataType.EditComboValue;
    }

    public record EditMultiComboValueMetadataResponse(ValueList<ComboValue> Values) : MetadataResponse
    {
        public override MetaFieldDataType Type => MetaFieldDataType.EditMultiComboValue;
    }

    public record NoteMetadataResponse(string Value) : MetadataResponse
    {
        public override MetaFieldDataType Type => MetaFieldDataType.Note;
    }

    public record ItemReferenceResponseItem(int ItemId, string Title, int BaseId, ItemType ItemType);

    public record MasterItemReferenceMetadataResponse(ValueList<ItemReferenceResponseItem> Items) : MetadataResponse
    {
        public override MetaFieldDataType Type => MetaFieldDataType.MasterItemReference;
    }

    public record SlaveItemReferenceMetadataResponse(ValueList<ItemReferenceResponseItem> Items) : MetadataResponse
    {
        public override MetaFieldDataType Type => MetaFieldDataType.SlaveItemReference;
    }

    public record TreeValue(int Id, string Label, string OptionValue, int? ParentId);

    public record TreeMetadataResponse(ValueList<TreeValue> Values) : MetadataResponse
    {
        public override MetaFieldDataType Type => MetaFieldDataType.Tree;
    }

    public record LinkMetadataResponse(string? Value) : MetadataResponse
    {
        public override MetaFieldDataType Type => MetaFieldDataType.Link;
    }

    public record FloatMetadataResponse(double? Value) : MetadataResponse
    {
        public override MetaFieldDataType Type => MetaFieldDataType.Float;
    }
}
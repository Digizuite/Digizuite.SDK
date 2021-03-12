using System;
using System.Text.Json;
using System.Text.Json.Serialization;
using Digizuite.Extensions;
using Digizuite.Models.Metadata;

namespace Digizuite.Metadata.ResponseModels.Helpers
{
    internal class MetaFieldResponseConverter : JsonConverter<MetaFieldResponse>
    {
        public override bool CanConvert(Type typeToConvert)
        {
            return typeof(MetaFieldResponse).IsAssignableFrom(typeToConvert);
        }

        private static T? Deserialize<T>(JsonDocument doc, JsonSerializerOptions options)
        {
            return JsonSerializer.Deserialize<T>(doc.RootElement.GetRawText(), options);
        }

        public override MetaFieldResponse? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            using var doc = JsonDocument.ParseValue(ref reader);

            var typeElement = doc.RootElement.GetPropertyCaseInsensitive(nameof(MetaFieldResponse.Type));

            var type = (MetaFieldDataType) typeElement.GetInt32();

            return type switch
            {
                MetaFieldDataType.Int => Deserialize<IntMetaFieldResponse>(doc, options),
                MetaFieldDataType.String => Deserialize<StringMetaFieldResponse>(doc, options),
                MetaFieldDataType.Bit => Deserialize<BitMetaFieldResponse>(doc, options),
                MetaFieldDataType.DateTime => Deserialize<DateTimeMetaFieldResponse>(doc, options),
                MetaFieldDataType.MultiComboValue => Deserialize<MultiComboValueMetaFieldResponse>(doc, options),
                MetaFieldDataType.ComboValue => Deserialize<ComboValueMetaFieldResponse>(doc, options),
                MetaFieldDataType.EditComboValue => Deserialize<EditComboValueMetaFieldResponse>(doc, options),
                MetaFieldDataType.Note => Deserialize<NoteMetaFieldResponse>(doc, options),
                MetaFieldDataType.MasterItemReference => Deserialize<MasterItemReferenceMetaFieldResponse>(doc, options),
                MetaFieldDataType.SlaveItemReference => Deserialize<SlaveItemReferenceMetaFieldResponse>(doc, options),
                MetaFieldDataType.Float => Deserialize<FloatMetaFieldResponse>(doc, options),
                MetaFieldDataType.EditMultiComboValue => Deserialize<EditMultiComboValueMetaFieldResponse>(doc, options),
                MetaFieldDataType.Tree => Deserialize<TreeMetaFieldResponse>(doc, options),
                MetaFieldDataType.Link => Deserialize<LinkMetaFieldResponse>(doc, options),
                _ => throw new ArgumentOutOfRangeException(nameof(type), type, "Unknown metafield data type")
            };
        }

        public override void Write(Utf8JsonWriter writer, MetaFieldResponse value, JsonSerializerOptions options)
        {
            JsonSerializer.Serialize(writer, value, value.GetType(), options);
        }
    }
}
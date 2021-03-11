using System;
using System.Text.Json;
using System.Text.Json.Serialization;
using Digizuite.Extensions;
using Digizuite.Models.Metadata;

namespace Digizuite.Metadata.ResponseModels.Helpers
{
    public class MetadataResponseConverter : JsonConverter<MetadataResponse>
    {
        public override bool CanConvert(Type typeToConvert)
        {
            return typeof(MetadataResponse).IsAssignableFrom(typeToConvert);
        }

        private static T? Deserialize<T>(JsonDocument doc, JsonSerializerOptions options)
        {
            return JsonSerializer.Deserialize<T>(doc.RootElement.GetRawText(), options);
        }

        public override MetadataResponse? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            
            using var doc = JsonDocument.ParseValue(ref reader);

            var typeElement = doc.RootElement.GetPropertyCaseInsensitive(nameof(MetadataResponse.Type));

            var type = (MetaFieldDataType) typeElement.GetInt32();

            return type switch
            {
                MetaFieldDataType.Int => Deserialize<IntMetadataResponse>(doc, options),
                MetaFieldDataType.String => Deserialize<StringMetadataResponse>(doc, options),
                MetaFieldDataType.Bit => Deserialize<BitMetadataResponse>(doc, options),
                MetaFieldDataType.DateTime => Deserialize<DateTimeMetadataResponse>(doc, options),
                MetaFieldDataType.MultiComboValue => Deserialize<MultiComboValueMetadataResponse>(doc, options),
                MetaFieldDataType.ComboValue => Deserialize<ComboValueMetadataResponse>(doc, options),
                MetaFieldDataType.EditComboValue => Deserialize<EditComboValueMetadataResponse>(doc, options),
                MetaFieldDataType.Note => Deserialize<NoteMetadataResponse>(doc, options),
                MetaFieldDataType.MasterItemReference => Deserialize<MasterItemReferenceMetadataResponse>(doc, options),
                MetaFieldDataType.SlaveItemReference => Deserialize<SlaveItemReferenceMetadataResponse>(doc, options),
                MetaFieldDataType.Float => Deserialize<FloatMetadataResponse>(doc, options),
                MetaFieldDataType.EditMultiComboValue => Deserialize<EditMultiComboValueMetadataResponse>(doc, options),
                MetaFieldDataType.Tree => Deserialize<TreeMetadataResponse>(doc, options),
                MetaFieldDataType.Link => Deserialize<LinkMetadataResponse>(doc, options),
                _ => throw new ArgumentOutOfRangeException()
            };
        }

        public override void Write(Utf8JsonWriter writer, MetadataResponse value, JsonSerializerOptions options)
        {
            JsonSerializer.Serialize(writer, value, value.GetType(), options);
        }
    }
}
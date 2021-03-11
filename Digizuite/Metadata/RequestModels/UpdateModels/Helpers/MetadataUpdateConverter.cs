using System;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Digizuite.Metadata.RequestModels.UpdateModels.Helpers
{
    internal class MetadataUpdateConverter : JsonConverter<MetadataUpdate>
    {
        public override bool CanConvert(Type typeToConvert)
        {
            return typeof(MetadataUpdate).IsAssignableFrom(typeToConvert);
        }

        public override MetadataUpdate? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            return JsonSerializer.Deserialize(ref reader, typeToConvert, options) as MetadataUpdate;
        }

        public override void Write(Utf8JsonWriter writer, MetadataUpdate value, JsonSerializerOptions options)
        {
            JsonSerializer.Serialize(writer, value, value.GetType(), options);
        }
    }
}
using System;
using Newtonsoft.Json;

namespace Digizuite.BatchUpdate.Models
{
    public class FieldTypeJsonConverter : JsonConverter
    {
        public override bool CanRead => false;

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            var fieldType = value as FieldType;
            writer.WriteValue(fieldType?.Value);
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue,
            JsonSerializer serializer)
        {
            throw new NotImplementedException();
        }

        public override bool CanConvert(Type objectType)
        {
            return objectType == typeof(FieldType);
        }
    }
}
using System;
using System.Text.Json;
using System.Text.Json.Serialization;
using Digizuite.Metadata.RequestModels.UpdateModels.Values;

namespace Digizuite.Metadata.RequestModels.UpdateModels.Helpers
{
    internal class InputComboConverter : JsonConverter<BaseInputCombo>
    {
        public override bool CanConvert(Type typeToConvert)
        {
            return typeof(BaseInputCombo).IsAssignableFrom(typeToConvert);
        }

        public override BaseInputCombo? Read(ref Utf8JsonReader reader, Type typeToConvert,
            JsonSerializerOptions options)
        {
            if (reader.TokenType == JsonTokenType.Number && reader.TryGetInt32(out var valueId))
            {
                return new ExistingCombo(valueId);
            }

            return JsonSerializer.Deserialize<DynamicCombo>(ref reader, options);
        }

        public override void Write(Utf8JsonWriter writer, BaseInputCombo value, JsonSerializerOptions options)
        {
            switch (value)
            {
                case DynamicCombo dynamicCombo:
                    JsonSerializer.Serialize(writer, dynamicCombo, options);
                    break;
                case ExistingCombo existingCombo:
                    writer.WriteNumberValue(existingCombo.ComboValueId);
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(value), value.GetType(), "Unknown combo type");
            }
        }
    }
}

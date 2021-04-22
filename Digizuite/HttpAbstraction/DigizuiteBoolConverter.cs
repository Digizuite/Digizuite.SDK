using System;
using System.Diagnostics.Contracts;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Digizuite.HttpAbstraction
{
    /// <summary>
    /// Converts between the strange ways the Digizuite can represent a bool, to a proper bool
    /// </summary>
    public class DigizuiteBoolConverter : JsonConverter<bool>
    {
        public override bool Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            return reader.TokenType switch
            {
                JsonTokenType.True => true,
                JsonTokenType.False => false,
                JsonTokenType.Number when reader.TryGetInt32(out var i) => i == 1,
                JsonTokenType.String => reader.GetString() switch
                {
                    "true" or "1" => true,
                    _ => false
                },
                _ => false
            };
        }

        public override void Write(Utf8JsonWriter writer, bool value, JsonSerializerOptions options)
        {
            writer.WriteBooleanValue(value);
        }
    }
}
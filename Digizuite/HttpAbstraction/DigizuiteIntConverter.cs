using System;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Digizuite.HttpAbstraction
{
    public class DigizuiteIntConverter : JsonConverter<int>
    {
        public override int Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            return reader.TokenType switch
            {
                JsonTokenType.String when int.TryParse(reader.GetString(), out var si) => si,
                JsonTokenType.Number when reader.TryGetInt32(out var ni) => ni,
                JsonTokenType.String when string.IsNullOrWhiteSpace(reader.GetString()) => 0,
                _ => throw new Exception($"Value is not a valid int {reader.ValueSequence.ToString()}")
            };
        }

        public override void Write(Utf8JsonWriter writer, int value, JsonSerializerOptions options)
        {
            writer.WriteNumberValue(value);
        }
    }
    
    public class DigizuiteLongConverter : JsonConverter<long>
    {
        public override long Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            return reader.TokenType switch
            {
                JsonTokenType.String when long.TryParse(reader.GetString(), out var si) => si,
                JsonTokenType.Number when reader.TryGetInt64(out var ni) => ni,
                JsonTokenType.String when string.IsNullOrWhiteSpace(reader.GetString()) => 0,
                _ => throw new Exception($"Value is not a valid long {reader.ValueSequence.ToString()}")
            };
        }

        public override void Write(Utf8JsonWriter writer, long value, JsonSerializerOptions options)
        {
            writer.WriteNumberValue(value);
        }
    }
}

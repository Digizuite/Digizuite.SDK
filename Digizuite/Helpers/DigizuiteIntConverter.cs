using System;
using System.Diagnostics.Contracts;
using System.Globalization;
using Newtonsoft.Json;

namespace Digizuite.Helpers
{
    public class DigizuiteIntConverter : JsonConverter
    {
        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            Contract.Requires(writer != null);
            writer.WriteValue(value);
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            Contract.Requires(reader != null);
            switch (reader.Value)
            {
                case string s:
                {
                    if (string.IsNullOrWhiteSpace(s)) return default(int);
                    if (int.TryParse(s, out int iVal))
                    {
                        return iVal;
                    }
                    throw new Exception($"Cannot create int from the given reader value {reader.Value}");
                }
                case long l:
                    return (int)Convert.ChangeType(l, TypeCode.Int32, CultureInfo.InvariantCulture.NumberFormat);
                case int i:
                    return i;
                default:
                    throw new Exception($"Cannot create bool from the given reader value {reader.Value}");
            }
        }

        public override bool CanConvert(Type objectType)
        {
            return objectType == typeof(int);
        }
    }
}

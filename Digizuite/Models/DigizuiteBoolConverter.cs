using System;
using System.Diagnostics.Contracts;
using Newtonsoft.Json;

namespace Digizuite.Models
{
    /// <summary>
    /// Converts between the strange ways the Digizuite can represent a bool, to a proper bool
    /// </summary>
    public class DigizuiteBoolConverter : JsonConverter
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
                    return s == "1" || s == "true";
                case int i:
                    return i == 1;
                case bool b:
                    return b;
                default:
                    throw new Exception("Cannot create bool from the given reader value");
            }
        }

        public override bool CanConvert(Type objectType)
        {
            return objectType == typeof(bool);
        }
    }
}
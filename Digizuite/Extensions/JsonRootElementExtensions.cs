using System;
using System.Text.Json;

namespace Digizuite.Extensions
{
    internal static class JsonRootElementExtensions
    {
        public static JsonElement GetPropertyCaseInsensitive(this JsonElement element, string propertyName)
        {
            foreach (var property in element.EnumerateObject())
            {
                if (property.Name.Equals(propertyName, StringComparison.OrdinalIgnoreCase))
                {
                    return property.Value;
                }
            }

            throw new JsonException($"Could not find property {propertyName}");
        }
    }
}
using System;
using System.Text.Json;
using System.Text.Json.Serialization;
using Digizuite.Metadata.RequestModels.UpdateModels.Values;

namespace Digizuite.Metadata.RequestModels.UpdateModels.Helpers
{
    internal class InputTreeNodeConverter : JsonConverter<BaseTreeNodeUpdate>
    {
        public override bool CanConvert(Type typeToConvert)
        {
            return typeof(BaseTreeNodeUpdate).IsAssignableFrom(typeToConvert);
        }

        public override BaseTreeNodeUpdate? Read(ref Utf8JsonReader reader, Type typeToConvert,
            JsonSerializerOptions options)
        {
            if (reader.TokenType == JsonTokenType.Number && reader.TryGetInt32(out var treeValueId))
            {
                return new ExistingTreeNode(treeValueId);
            }

            using var doc = JsonDocument.ParseValue(ref reader);

            if (HasProperty(doc.RootElement, nameof(DynamicTopDownTreeNode.Children)))
            {
                return JsonSerializer.Deserialize<DynamicTopDownTreeNode>(doc.RootElement.GetRawText(), options);
            }

            return JsonSerializer.Deserialize<DynamicBottomUpTreeNode>(doc.RootElement.GetRawText(), options);
        }

        private bool HasProperty(JsonElement doc, string propertyName)
        {
            foreach (var property in doc.EnumerateObject())
            {
                if (property.Name.Equals(propertyName, StringComparison.OrdinalIgnoreCase))
                {
                    return true;
                }
            }

            return false;
        }

        public override void Write(Utf8JsonWriter writer, BaseTreeNodeUpdate value, JsonSerializerOptions options)
        {
            switch (value)
            {
                case DynamicTopDownTreeNode dynamicTopDownTreeNode:
                    JsonSerializer.Serialize(writer, dynamicTopDownTreeNode, options);
                    break;
                case DynamicBottomUpTreeNode dynamicBottomUpTreeNode:
                    JsonSerializer.Serialize(writer, dynamicBottomUpTreeNode, options);
                    break;
                case ExistingTreeNode existingTreeNode:
                    writer.WriteNumberValue(existingTreeNode.TreeValueId);
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(value), value.GetType(),
                        "Unknown tree node update type");
            }
        }
    }
}

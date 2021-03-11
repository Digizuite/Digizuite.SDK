using System.Text.Json.Serialization;
using Digizuite.Metadata.RequestModels.UpdateModels.Helpers;

namespace Digizuite.Metadata.RequestModels.UpdateModels.Values
{
    [JsonConverter(typeof(InputComboConverter))]
    public abstract record BaseInputCombo
    {
    }
}

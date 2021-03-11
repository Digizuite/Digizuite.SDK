namespace Digizuite.Metadata.RequestModels.UpdateModels.Values
{
    public record DynamicCombo : BaseInputCombo
    {
        public string Label { get; set; } = null!;

        public string OptionValue { get; set; } = null!;
    }
}
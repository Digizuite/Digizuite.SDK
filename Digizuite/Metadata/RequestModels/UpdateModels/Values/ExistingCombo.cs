namespace Digizuite.Metadata.RequestModels.UpdateModels.Values
{
    public record ExistingCombo : BaseInputCombo
    {
        public int ComboValueId { get; set; }

        public ExistingCombo(int comboValueId)
        {
            ComboValueId = comboValueId;
        }
    }
}

namespace Digizuite.Models.Metadata
{
#pragma warning disable CA1720
    public enum MetaFieldDataType
    {
        Int = 51,
        String = 60,
        Bit = 61,
        DateTime = 64,
        MultiComboValue = 67,
        ComboValue = 68,
        EditComboValue = 69,
        Note = 70,
        MasterItemReference = 80,
        SlaveItemReference = 81,
        Float = 82,
        EditMultiComboValue = 169,
        Tree = 300,
        Link = 350,
        
        MetaFieldGroup = 65,
        MetagroupRef = 100
    }
#pragma warning restore CA1720
}
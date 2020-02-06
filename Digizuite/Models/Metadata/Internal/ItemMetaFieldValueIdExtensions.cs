using Digizuite.Models.Metadata.Values;

namespace Digizuite.Models.Metadata.Internal
{
    internal static class ItemMetaFieldValueIdExtensions
    {
        internal static ComboValue ToComboValue(this ItemMetafieldValueIdResponse value)
        {
            return new ComboValue
            {
                Label = value.metaValue,
                InternalValue = value.combooptionvalue,
                Value = value.item_combo_valueid
            };
        }

        internal static ComboValue ToEditComboValue(this ItemMetafieldValueIdResponse value)
        {
            return new ComboValue
            {
                Label = value.metaValue,
                InternalValue = value.combooptionvalue,
                Value = value.metaValue
            };
        }

        internal static TreeValue ToTreeValue(this ItemMetafieldValueIdResponse value)
        {
            return new TreeValue
            {
                Id = int.Parse(value.item_tree_valueid),
                Value = value.optionvalue,
                Label = value.metaValue
            };
        }

        internal static ItemReferenceOption ToItemReferenceOption(this ItemMetafieldValueIdResponse value)
        {
            return new ItemReferenceOption
            {
                Type = MasterSlaveReferenceType.Other,
                Label = value.refItemTitle,
                ItemId = int.Parse(value.ref_itemid),
                BaseId = int.Parse(value.refItemBaseId)
            };
        }
    }
}

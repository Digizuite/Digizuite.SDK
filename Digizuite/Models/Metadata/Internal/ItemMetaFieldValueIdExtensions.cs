using System.Globalization;
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
                Id = int.Parse(value.item_tree_valueid, NumberStyles.Integer, CultureInfo.InvariantCulture),
                Value = value.optionvalue,
                Label = value.metaValue
            };
        }

        internal static ItemReferenceOption ToItemReferenceOption(this ItemMetafieldValueIdResponse value)
        {
            return new ItemReferenceOption
            {
                Label = value.refItemTitle,
                ItemId = int.Parse(value.ref_itemid, NumberStyles.Integer, CultureInfo.InvariantCulture),
                BaseId = int.Parse(value.refItemBaseId, NumberStyles.Integer, CultureInfo.InvariantCulture)
            };
        }
        
        internal static ItemReferenceOption ToItemReferenceOption(this SirValueId value)
        {
            return new ItemReferenceOption
            {
                Label = value.Title,
                ItemId = value.ItemId,
                BaseId = value.BaseId
            };
        }
    }
}

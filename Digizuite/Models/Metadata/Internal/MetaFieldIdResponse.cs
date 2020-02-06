using System.Diagnostics.CodeAnalysis;

namespace Digizuite.Models.Metadata.Internal
{
    [SuppressMessage("ReSharper", "InconsistentNaming")]
    internal class MetaFieldIdResponse
    {
        public int Metafieldid { get; set; }
        public string MetafieldName { get; set; }
        public string MetafieldAudit { get; set; }
        public string MetafieldAutoTranslate { get; set; }
        public string MetafieldOverwriteExisting { get; set; }
        public string MetafieldIsRequired { get; set; }
        public string MetafieldRecursiveToRoot { get; set; }
        public string MetafieldIsRequiredUpload { get; set; }
        public string MetafieldReadonly { get; set; }
        public string MetafieldFormfieldsize { get; set; }
        public int MetafieldItemId { get; set; }
        public string MetafieldSecWriteaccess { get; set; }
        public string Iterated { get; set; }
        public string Is_html { get; set; }
        public string Treeview_format { get; set; }
        public string Show_extra_field { get; set; }
        public MetaFieldDataType Item_datatypeid { get; set; }
        public string MetafieldSortindex { get; set; }
        public string MetafieldMaxlength { get; set; }
        public string MetafieldComboViewType { get; set; }
        public string MetafieldVisibilityMetafieldId { get; set; }
        public string MetafieldVisibilityRegex { get; set; }
        public string IsCaseSensitive { get; set; }
        public string Ref_itemid { get; set; }
        public string RefItemBaseId { get; set; }
        public string RefItemTitle { get; set; }
        public string MetafieldReferenceSelectMode { get; set; }
        public string MetafieldReferenceMaxItems { get; set; }
        public string MetafieldRestrictToItemtypeid { get; set; }
    }
}

namespace Digizuite.BatchUpdate.Models
{
    public class FieldType
    {
        public static FieldType Asset = new FieldType("asset");
        public static FieldType Item = new FieldType("item");
        public static FieldType Metafield = new FieldType("metafield");
        public static FieldType State = new FieldType("state");
        public static FieldType Empty = new FieldType("empty");
        public static FieldType LayoutIsPublic = new FieldType("layoutIsPublic");
        public static FieldType DefaultMetafieldLanguageId = new FieldType("defaultmetafieldlanguageid");
        public static FieldType Member = new FieldType("member");
        public static FieldType MemberGroupIds = new FieldType("membergroupids");
        public static FieldType Folder = new FieldType("folder");
        public static FieldType ItemSecurity = new FieldType("item_security");
        public static FieldType AccessorItemId = new FieldType("accessor_itemid");
        public static FieldType WriteAccess = new FieldType("writeaccess");
        public static FieldType ReadAccess = new FieldType("readaccess");
        public static FieldType ItemSecurityId = new FieldType("item_securityid");
        public static FieldType MetaComboDefinition = new FieldType("metacombo_definition");
        public static FieldType ComboValue = new FieldType("combovalue");
        public static FieldType SortIndex = new FieldType("sortindex");
        public static FieldType IsPublic = new FieldType("ispublic");
        public static FieldType Visible = new FieldType("visible");
        public static FieldType OptionValue = new FieldType("optionvalue");
        public static FieldType ItemMetaFieldLabelId = new FieldType("item_metafield_labelid");
        public static FieldType Password = new FieldType("password");


        public readonly string Value;

        private FieldType(string value)
        {
            Value = value;
        }

        public override string ToString()
        {
            return Value;
        }
    }
}
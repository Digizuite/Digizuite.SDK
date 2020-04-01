namespace Digizuite.BatchUpdate.Models
{
    public class FieldType
    {
        public static readonly FieldType Asset = new FieldType("asset");
        public static readonly FieldType AssetType = new FieldType("AssetType");
        public static readonly FieldType Item = new FieldType("item");
        public static readonly FieldType Metafield = new FieldType("metafield");
        public static readonly FieldType MetafieldGroup = new FieldType("metafield_group");
        public static readonly FieldType State = new FieldType("state");
        public static readonly FieldType Empty = new FieldType("empty");
        public static readonly FieldType LayoutIsPublic = new FieldType("layoutIsPublic");
        public static readonly FieldType DefaultMetafieldLanguageId = new FieldType("defaultmetafieldlanguageid");
        public static readonly FieldType Member = new FieldType("member");
        public static readonly FieldType MemberGroupIds = new FieldType("membergroupids");
        public static readonly FieldType Folder = new FieldType("folder");
        public static readonly FieldType ItemSecurity = new FieldType("item_security");
        public static readonly FieldType AccessorItemId = new FieldType("accessor_itemid");
        public static readonly FieldType WriteAccess = new FieldType("writeaccess");
        public static readonly FieldType ReadAccess = new FieldType("readaccess");
        public static readonly FieldType ItemSecurityId = new FieldType("item_securityid");
        public static readonly FieldType MetaComboDefinition = new FieldType("metacombo_definition");
        public static readonly FieldType ComboValue = new FieldType("combovalue");
        public static readonly FieldType SortIndex = new FieldType("sortindex");
        public static readonly FieldType IsPublic = new FieldType("ispublic");
        public static readonly FieldType Visible = new FieldType("visible");
        public static readonly FieldType OptionValue = new FieldType("optionvalue");
        public static readonly FieldType ItemMetaFieldLabelId = new FieldType("item_metafield_labelid");
        public static readonly FieldType Password = new FieldType("password");
        public static readonly FieldType Name = new FieldType("name");


        public string Value { get; }

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
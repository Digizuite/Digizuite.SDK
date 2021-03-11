using Digizuite.Models.Metadata.Values;

namespace Digizuite.Models.Metadata.Fields
{
    public class ComboMetafield : Field<ComboValue?>
    {
        private string _viewType;
        public MetaFieldDataType Type => MetaFieldDataType.ComboValue;

        /// <summary>
        /// Can only be the strings "combo" or "radio"
        /// </summary>
        public string ViewType
        {
            get => _viewType;
            set
            {
                if (value == "combo" || value == "radio")
                {
                    _viewType = value;
                }
            }
        }

        public override string ToSingleString(string separator)
        {
            return Value?.Label ?? "";
        }
    }
}
using System;
using System.Globalization;

namespace Digizuite.Models.Metadata.Fields
{
    public class DateTimeMetafield : Field<DateTime?>
    {
        private string _subType;
        public MetaFieldDataType Type => MetaFieldDataType.DateTime;

        /// <summary>
        /// must be either "date" or "datetime" 
        /// </summary>
        public string SubType
        {
            get => _subType;
            set
            {
                if (value == "date" || value == "datetime")
                {
                    _subType = value;
                }
            }
        }

        public override string ToSingleString(string separator)
        {
            if (Value == default)
            {
                return "";
            }

            return Value?.ToString(CultureInfo.InvariantCulture) ?? "";
        }
    }
}
using System;
using System.Globalization;
using Digizuite.Metadata.ResponseModels.Properties;

namespace Digizuite.Models.Metadata.Fields
{
    public record DateTimeMetafield : Field<DateTime?>
    {
        public DateTimeViewType SubType { get; set; } = default!;
        public MetaFieldDataType Type => MetaFieldDataType.DateTime;

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
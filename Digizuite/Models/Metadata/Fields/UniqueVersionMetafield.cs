using Digizuite.Models.Metadata.Values;

namespace Digizuite.Models.Metadata.Fields
{
    public class UniqueVersionMetafield : Field<UniqueVersionValue>
    {
        public MetaFieldDataType Type => MetaFieldDataType.EditComboValue;

        public override string ToSingleString(string separator)
        {
            if (Value == null)
            {
                return "";
            }

            return Value.Value + ":" + Value.Version;
        }
    }
}
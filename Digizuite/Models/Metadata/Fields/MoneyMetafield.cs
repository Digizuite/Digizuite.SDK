namespace Digizuite.Models.Metadata.Fields
{
    public class MoneyMetafield : Field<string>
    {
        public MetaFieldDataType Type => MetaFieldDataType.Money;

        public override string ToSingleString(string separator)
        {
            return Value;
        }
    }
}
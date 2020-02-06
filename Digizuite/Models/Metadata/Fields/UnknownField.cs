namespace Digizuite.Models.Metadata.Fields
{
    public class UnknownField<T> : Field<T>
    {
        public override string ToSingleString(string separator)
        {
            return Value.ToString();
        }
    }
}

using System;
using System.Diagnostics.CodeAnalysis;

namespace Digizuite.Models.Metadata.Values
{
    public class TreeValueReference
    {
        public string Value { get; set; }

        public override string ToString()
        {
            return $"{nameof(Value)}: {Value}";
        }

        protected bool Equals(TreeValueReference other)
        {
            if (other == null)
            {
                throw new ArgumentNullException(nameof(other));
            }

            return string.Equals(Value, other.Value, StringComparison.InvariantCultureIgnoreCase);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != GetType()) return false;
            return Equals((TreeValueReference)obj);
        }

        [SuppressMessage("ReSharper", "NonReadonlyMemberInGetHashCode")]
        public override int GetHashCode()
        {
            return Value != null ? Value.GetHashCode() : 0;
        }
    }
}

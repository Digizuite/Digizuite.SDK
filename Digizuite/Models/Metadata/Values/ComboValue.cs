using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace Digizuite.Models.Metadata.Values
{
    public class ComboValue
    {
        public string Label { get; set; }
        public string InternalValue { get; set; }
        public string Value { get; set; }

        public static IEqualityComparer<ComboValue> InternalValueComparer { get; } =
            new InternalValueEqualityComparer();

        public override string ToString()
        {
            return $"{nameof(Label)}: {Label}, {nameof(InternalValue)}: {InternalValue}, {nameof(Value)}: {Value}";
        }

        protected bool Equals(ComboValue other)
        {
            if (other == null)
            {
                throw new ArgumentNullException(nameof(other));
            }
            return string.Equals(InternalValue, other.InternalValue, StringComparison.InvariantCultureIgnoreCase);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((ComboValue)obj);
        }

        [SuppressMessage("ReSharper", "NonReadonlyMemberInGetHashCode")]
        public override int GetHashCode()
        {
            unchecked
            {
                var hashCode = (Label != null ? Label.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ (InternalValue != null ? InternalValue.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ (Value != null ? Value.GetHashCode() : 0);
                return hashCode;
            }
        }

        private sealed class InternalValueEqualityComparer : IEqualityComparer<ComboValue>
        {
            public bool Equals(ComboValue x, ComboValue y)
            {
                if (ReferenceEquals(x, y)) return true;
                if (ReferenceEquals(x, null)) return false;
                if (ReferenceEquals(y, null)) return false;
                if (x.GetType() != y.GetType()) return false;
                return string.Equals(x.InternalValue, y.InternalValue, StringComparison.InvariantCultureIgnoreCase);
            }

            public int GetHashCode(ComboValue obj)
            {
                return (obj.InternalValue != null ? obj.InternalValue.GetHashCode() : 0);
            }
        }
    }
}

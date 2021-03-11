using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace Digizuite.Models.Metadata.Values
{
    public record ComboValue
    {
        public string Label { get; set; }
        public string OptionValue { get; set; }
        public int Id { get; set; }

        public virtual bool Equals(ComboValue? other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return OptionValue == other.OptionValue;
        }

        public override int GetHashCode()
        {
            return OptionValue.GetHashCode();
        }
    }
}

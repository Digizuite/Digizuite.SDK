using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace Digizuite.Models.Metadata.Fields
{
    public abstract class Field
    {
        public int ItemId { get; set; }
        public int MetafieldId { get; set; }
        public int LabelId { get; set; }
        public string Label { get; set; } = null!;
        public int LanguageId { get; set; }
        public bool ReadOnly { get; set; }
        public bool Required { get; set; }
        public int SortIndex { get; set; }
        public bool AutoTranslated { get; set; }
        public bool AutoTranslateOverwriteExisting { get; set; }
        public bool Audited { get; set; }
        public int VisibilityMetaFieldId { get; set; }
        public string VisibilityRegex { get; set; } = "";

        public abstract string ToSingleString(string separator);
    }
    public abstract class Field<T> : Field
    {
        public T Value { get; set; }

        public override string ToString()
        {
            return
                $"{nameof(ItemId)}: {ItemId}, {nameof(MetafieldId)}: {MetafieldId}, {nameof(LabelId)}: {LabelId}, {nameof(Label)}: {Label}, {nameof(LanguageId)}: {LanguageId}, {nameof(ReadOnly)}: {ReadOnly}, {nameof(Required)}: {Required}, {nameof(SortIndex)}: {SortIndex}, {nameof(Value)}: {Value}, {nameof(AutoTranslated)}: {AutoTranslated}, {nameof(Audited)}: {Audited}, {nameof(VisibilityMetaFieldId)}: {VisibilityMetaFieldId}";
        }

        protected bool Equals(Field<T> other)
        {
            if (other == null)
            {
                throw new ArgumentNullException(nameof(other));
            }
            
            return ItemId == other.ItemId && MetafieldId == other.MetafieldId && LabelId == other.LabelId &&
                   string.Equals(Label, other.Label, StringComparison.InvariantCulture) && LanguageId == other.LanguageId && ReadOnly == other.ReadOnly &&
                   Required == other.Required && SortIndex == other.SortIndex &&
                   EqualityComparer<T>.Default.Equals(Value, other.Value) && AutoTranslated == other.AutoTranslated &&
                   Audited == other.Audited && VisibilityMetaFieldId == other.VisibilityMetaFieldId;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != GetType()) return false;
            return Equals((Field<T>)obj);
        }

        [SuppressMessage("ReSharper", "NonReadonlyMemberInGetHashCode")]
        public override int GetHashCode()
        {
            unchecked
            {
                var hashCode = ItemId;
                hashCode = (hashCode * 397) ^ MetafieldId;
                hashCode = (hashCode * 397) ^ LabelId;
                hashCode = (hashCode * 397) ^ (Label != null ? Label.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ LanguageId;
                hashCode = (hashCode * 397) ^ ReadOnly.GetHashCode();
                hashCode = (hashCode * 397) ^ Required.GetHashCode();
                hashCode = (hashCode * 397) ^ SortIndex;
                hashCode = (hashCode * 397) ^ EqualityComparer<T>.Default.GetHashCode(Value);
                hashCode = (hashCode * 397) ^ AutoTranslated.GetHashCode();
                hashCode = (hashCode * 397) ^ Audited.GetHashCode();
                hashCode = (hashCode * 397) ^ VisibilityMetaFieldId;
                return hashCode;
            }
        }
    }
}

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Digizuite.Extensions;

#nullable enable
namespace Digizuite.Collections
{
    public class ValueList<T> : ICollection<T>
        where T : notnull
    {
        private readonly List<T> _innerCollection;

        private readonly IEqualityComparer<T> _comparer;

        public bool Remove(T item)
        {
            return _innerCollection.Remove(item);
        }

        public int Count => _innerCollection.Count;
        public bool IsReadOnly => false;

        public ValueList(IEqualityComparer<T> comparer)
        {
            _comparer = comparer;
            _innerCollection = new List<T>();
        }

        public ValueList() : this(EqualityComparer<T>.Default)
        {
        }

        public ValueList(IEnumerable<T> enumerable) : this()
        {
            _innerCollection = new List<T>(enumerable);
        }

        public IEnumerator<T> GetEnumerator()
        {
            return _innerCollection.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return ((IEnumerable) _innerCollection).GetEnumerator();
        }

        public void Add(T item)
        {
            _innerCollection.Add(item);
        }

        public void RemoveAll(Predicate<T> match)
        {
            _innerCollection.RemoveAll(match);
        }

        protected virtual bool Equals(ValueList<T> other)
        {
            if (other.Count != Count)
            {
                return false;
            }

            var t = _innerCollection.ToHashSetNetstandard(_comparer);
            var o = other.ToHashSetNetstandard(_comparer);

            return t.ComparerSetEquals(o);
        }

        public override bool Equals(object? obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != GetType()) return false;
            return Equals((ValueList<T>) obj);
        }

        public override int GetHashCode()
        {
            return _innerCollection.Aggregate(0, (sum, item) => unchecked(sum + item.GetHashCode() * 397));
        }

        public static bool operator ==(ValueList<T>? left, ValueList<T>? right)
        {
            return Equals(left, right);
        }

        public static bool operator !=(ValueList<T>? left, ValueList<T>? right)
        {
            return !Equals(left, right);
        }

        public override string ToString()
        {
            return "[ " + string.Join(", ", _innerCollection) + " ]";
        }

        public void AddRange(IEnumerable<T> items)
        {
            _innerCollection.AddRange(items);
        }

        public void Clear()
        {
            _innerCollection.Clear();
        }

        public bool Contains(T item)
        {
            return _innerCollection.Contains(item);
        }

        public void CopyTo(T[] array, int arrayIndex)
        {
            _innerCollection.CopyTo(array, arrayIndex);
        }
        
        public T this[int index] => _innerCollection[index];
    }

    public static class ValueListExtensions
    {
        public static ValueList<T> ToValueList<T>(this IEnumerable<T> enumerable)
            where T : notnull
        {
            return new ValueList<T>(enumerable);
        }
    }
}

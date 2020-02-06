using System.Collections.Generic;

namespace Digizuite.BatchUpdate.Models
{
    public class Batch
    {
        public readonly List<BatchPart> Values = new List<BatchPart>();

        public Batch()
        {
        }

        public Batch(BatchPart values) : this()
        {
            AddValues(values);
        }

        public Batch(IEnumerable<BatchPart> values) : this()
        {
            AddValues(values);
        }

        public Batch AddValues(BatchPart values)
        {
            Values.Add(values);
            return this;
        }

        public Batch AddValues(IEnumerable<BatchPart> values)
        {
            Values.AddRange(values);
            return this;
        }
    }
}
using System.Collections.Generic;

namespace Digizuite.Models
{
    public class DigiResponse<T>
    {
        public bool Success { get; set; }
        public List<T> Items { get; set; } = default!;

        public object Error { get; set; } = default!;
        public object Errors { get; set; } = default!;
        public object Warning { get; set; } = default!;
        public object Warnings { get; set; } = default!;
        
        public int Total { get; set; }
    }
}
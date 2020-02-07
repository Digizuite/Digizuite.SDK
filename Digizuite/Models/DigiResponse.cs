using System.Collections.Generic;

namespace Digizuite.Models
{
    public class DigiResponse<T>
    {
        public bool Success { get; set; }
        public List<T> Items { get; set; }

        public object Error { get; set; }
        public object Errors { get; set; }
        public object Warning { get; set; }
        public object Warnings { get; set; }
        
        public int Total { get; set; }
    }
}
using System;
using System.Collections.Generic;

namespace Digizuite.Models
{
    public interface IConfiguration
    {
        /// <summary>
        /// The base url for all DC requests
        /// </summary>
        Uri BaseUrl { get; set; }

        string SystemUsername { get; set; }
        string SystemPassword { get; set; }
        
        string? ConfigVersionId { get; set; }
        
        HashSet<ServiceType> DevelopmentServices { get; set; }
    }
}
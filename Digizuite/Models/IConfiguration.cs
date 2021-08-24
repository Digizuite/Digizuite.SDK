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
        
        HashSet<ServiceType>? DevelopmentServices { get; set; }
        
        /// <summary>
        /// True if this service is running in Docker. Should only be used internally by Digizuite, since
        /// those services has direct communication paths with each other. 
        /// </summary>
        bool RunInDocker { get; }
    }
}
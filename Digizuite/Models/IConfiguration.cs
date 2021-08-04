using System;
using System.Collections.Generic;

namespace Digizuite.Models
{
    public interface IConfiguration
    {
        /// <summary>
        /// The base url for all DC requests
        /// </summary>
        Uri BaseUrl { get; }

        string SystemUsername { get; }
        string SystemPassword { get; }
        
        string? ConfigVersionId { get; }
        
        HashSet<ServiceType>? DevelopmentServices { get; }
        
        long UploadChunkSize { get; }
        
        /// <summary>
        /// True if this service is running in Docker. Should only be used internally by Digizuite, since
        /// those services has direct communication paths with each other. 
        /// </summary>
        bool RunInDocker { get; }
    }
}
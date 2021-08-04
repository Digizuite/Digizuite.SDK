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
    }
}
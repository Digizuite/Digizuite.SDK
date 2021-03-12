using System;
using System.Collections.Generic;

namespace Digizuite.Models
{
    public class DigizuiteConfiguration : IConfiguration
    {
        /// <summary>
        /// The base url for all DC requests
        /// </summary>
        public Uri BaseUrl { get; set; }

        public string SystemUsername { get; set; }
        public string SystemPassword { get; set; }

        /// <summary>
        /// The idPath "!/1/2/3/" of the configuration to use
        /// </summary>
        public string ConfigVersionId { get; set; }

        /// <summary>
        /// Optional, if not specified, get set to the same as config version id
        /// </summary>
        public string DataVersionId { get; set; }

        /// <summary>
        /// If localhost version of the core services should be called instead. 
        /// </summary>
        public HashSet<ServiceType> DevelopmentServices { get; set; } = new();

        public override string ToString()
        {
            return
                $"{nameof(BaseUrl)}: {BaseUrl}, {nameof(SystemUsername)}: {SystemUsername}, {nameof(SystemPassword)}: {SystemPassword.Length}, {nameof(ConfigVersionId)}: {ConfigVersionId}, {nameof(DataVersionId)}: {DataVersionId}";
        }

    }
}

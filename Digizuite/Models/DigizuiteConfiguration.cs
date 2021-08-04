using System;
using System.Collections.Generic;

namespace Digizuite.Models
{
    public class DigizuiteConfiguration : IConfiguration
    {
        private Uri _baseUrl = null!;

        /// <summary>
        /// The base url for all DC requests
        /// </summary>
        public Uri BaseUrl
        {
            get => _baseUrl;
            set => _baseUrl = new Uri(value.Scheme + Uri.SchemeDelimiter + value.Host);
        }

        public string SystemUsername { get; set; } = null!;
        public string SystemPassword { get; set; } = null!;

        /// <summary>
        /// The idPath "!/1/2/3/" of the configuration to use
        /// </summary>
        public string? ConfigVersionId { get; set; }

        /// <summary>
        /// If localhost version of the core services should be called instead. 
        /// </summary>
        public HashSet<ServiceType>? DevelopmentServices { get; set; } = new();

        public bool RunInDocker { get; set; }

        public long UploadChunkSize { get; set; } = 1000 * 1000 * 10;

        public override string ToString()
        {
            return
                $"{nameof(BaseUrl)}: {BaseUrl}, {nameof(SystemUsername)}: {SystemUsername}, {nameof(SystemPassword)}: {SystemPassword.Length}, {nameof(ConfigVersionId)}: {ConfigVersionId}, {nameof(UploadChunkSize)}: {UploadChunkSize} bytes";
        }
    }
}
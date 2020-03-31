using System;

namespace Digizuite.Models
{
    public class DigizuiteConfiguration : IConfiguration
    {
        /// <summary>
        /// The base url for all DC requests
        /// </summary>
        public Uri BaseUrl { get; set; }

        /// <summary>
        /// How long the access key lasts before it expires and should be renewed.
        /// We will renew the access key 10% before the duration is up, so we have time to fetch a new access key
        /// </summary>
        public TimeSpan AccessKeyDuration { get; set; } = TimeSpan.FromDays(1);

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

        public override string ToString()
        {
            return
                $"{nameof(BaseUrl)}: {BaseUrl}, {nameof(AccessKeyDuration)}: {AccessKeyDuration}, {nameof(SystemUsername)}: {SystemUsername}, {nameof(SystemPassword)}: {SystemPassword.Length}, {nameof(ConfigVersionId)}: {ConfigVersionId}, {nameof(DataVersionId)}: {DataVersionId}";
        }

    }
}

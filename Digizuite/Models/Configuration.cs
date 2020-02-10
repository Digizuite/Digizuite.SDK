using System;

namespace Digizuite.Models
{
    public class Configuration : IConfiguration
    {
        /// <summary>
        /// The base url for all DC requests
        /// </summary>
        public string BaseUrl { get; set; }

        /// <summary>
        /// How long the access key lasts before it expires and should be renewed.
        /// We will renew the access key 10% before the duration is up, so we have time to fetch a new access key
        /// </summary>
        public TimeSpan AccessKeyDuration { get; set; }

        public string SystemUsername { get; set; }
        public string SystemPassword { get; set; }

        public override string ToString()
        {
            return
                $"{nameof(BaseUrl)}: {BaseUrl}, {nameof(AccessKeyDuration)}: {AccessKeyDuration}, {nameof(SystemUsername)}: {SystemUsername}, {nameof(SystemPassword)}: {SystemPassword.Length}";
        }

    }
}

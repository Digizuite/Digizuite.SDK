using System;

namespace Digizuite.Models
{
    public interface IConfiguration
    {
        /// <summary>
        /// The base url for all DC requests
        /// </summary>
        Uri BaseUrl { get; set; }

        /// <summary>
        /// How long the access key lasts before it expires and should be renewed.
        /// We will renew the access key 10% before the duration is up, so we have time to fetch a new access key
        /// </summary>
        TimeSpan AccessKeyDuration { get; set; }

        string SystemUsername { get; set; }
        string SystemPassword { get; set; }
    }
}
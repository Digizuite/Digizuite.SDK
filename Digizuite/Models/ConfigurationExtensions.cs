using System;

namespace Digizuite.Models
{
    public static class ConfigurationExtensions
    {
        public static string GetDmm3Bwsv3Url(this IConfiguration config)
        {
            if (config == null) throw new ArgumentNullException(nameof(config));
            var baseUrl = config.BaseUrl;

            if (!baseUrl.EndsWith("/", StringComparison.OrdinalIgnoreCase))
            {
                baseUrl += "/";
            }
            if (!baseUrl.EndsWith("/dmm3bwsv3/", StringComparison.OrdinalIgnoreCase))
            {
                baseUrl += "dmm3bwsv3/";
            }
            return baseUrl;

        }
    }
}

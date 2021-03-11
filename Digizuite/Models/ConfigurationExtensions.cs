using System;

namespace Digizuite.Models
{
    public static class ConfigurationExtensions
    {
        public static Uri GetDmm3Bwsv3Url(this IConfiguration config)
        {
            if (config == null) throw new ArgumentNullException(nameof(config));
            var baseUrl = config.BaseUrl.ToString();

            if (!baseUrl.EndsWith("/", StringComparison.OrdinalIgnoreCase))
            {
                baseUrl += "/";
            }
            if (!baseUrl.EndsWith("/dmm3bwsv3/", StringComparison.OrdinalIgnoreCase))
            {
                baseUrl += "dmm3bwsv3/";
            }
            return new Uri(baseUrl);
        }

        public static Uri GetDigizuiteCoreUrl(this IConfiguration config)
        {
            if (config == null) throw new ArgumentNullException(nameof(config));
            var baseUrl = config.BaseUrl.ToString();

            if (!baseUrl.EndsWith("/", StringComparison.OrdinalIgnoreCase))
            {
                baseUrl += "/";
            }
            if (!baseUrl.EndsWith("/DigizuiteCore/", StringComparison.OrdinalIgnoreCase))
            {
                baseUrl += "DigizuiteCore/";
            }
            return new Uri(baseUrl);
        }
    }
}

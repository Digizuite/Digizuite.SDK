using System;
using System.Threading.Tasks;
using Digizuite.Models;

namespace Digizuite
{
    
    /// <inheritdoc cref="IAssetStreamerService"/>
    public class AssetStreamerService : IAssetStreamerService
    {
        private readonly IConfiguration _configuration;
        private readonly IDamAuthenticationService _damAuthenticationService;

        public AssetStreamerService(IConfiguration configuration, IDamAuthenticationService damAuthenticationService)
        {
            _configuration = configuration;
            _damAuthenticationService = damAuthenticationService;
        }

        public async Task<Uri> GetAssetDownloadUrl(int assetId,
            string accessKey = null, int mediaFormatId = -1, int destinationId = -1)
        {
            if (string.IsNullOrWhiteSpace(accessKey))
            {
                accessKey = await _damAuthenticationService.GetAccessKey().ConfigureAwait(false);
            }
            
            var baseUrl = _configuration.GetDmm3Bwsv3Url();
            
            var downloadUrl = new Uri(baseUrl, $"AssetStream.aspx?assetid={assetId}&download=true&accesskey={accessKey}");

            if (mediaFormatId == -1)
            {
                downloadUrl = new Uri(downloadUrl, downloadUrl.Query + "&AssetOutputIdent=Download");
            }
            else
            {
                downloadUrl = new Uri(downloadUrl,  downloadUrl.Query + $"&downloadname=&mediaformatid={mediaFormatId}&destinationid={destinationId}");
            }

            return downloadUrl;
        }
    }
}
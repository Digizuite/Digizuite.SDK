using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Digizuite.Exceptions;
using Digizuite.Extensions;
using Digizuite.HttpAbstraction;
using Digizuite.Logging;
using Digizuite.Models;

namespace Digizuite
{
    public class NewUploadService : IUploadService
    {
        private readonly ServiceHttpWrapper _serviceHttpWrapper;
        private readonly ILogger<NewUploadService> _logger;
        private readonly IDamAuthenticationService _damAuthenticationService;
        private readonly IConfiguration _configuration;

        public NewUploadService(ServiceHttpWrapper serviceHttpWrapper, ILogger<NewUploadService> logger, IDamAuthenticationService damAuthenticationService, IConfiguration configuration)
        {
            _serviceHttpWrapper = serviceHttpWrapper;
            _logger = logger;
            _damAuthenticationService = damAuthenticationService;
            _configuration = configuration;
        }

        public async Task<UploadResponse> Upload(Stream stream, string filename, string computerName, IUploadProgressListener? listener = null,
            CancellationToken cancellationToken = default)
        {
            _logger.LogDebug("Starting upload", nameof(filename), filename, nameof(computerName), computerName);

            var response = await UploadChunks(stream, null, KeepMetadata.Keep, Overwrite.ReplaceHistoryEntry,
                computerName, filename, listener, cancellationToken);

            return new UploadResponse(response.ItemId, response.AssetId);
        }

        public async Task<UploadResponse> Replace(Stream stream, string filename, string computerName, int targetAssetId, KeepMetadata keepMetadata,
            Overwrite overwrite, IUploadProgressListener? listener = null, CancellationToken cancellationToken = default)
        {
            var response = await UploadChunks(stream, targetAssetId, keepMetadata, overwrite,
                computerName, filename, listener, cancellationToken);

            return new UploadResponse(response.ItemId, response.AssetId);
        }

        private async Task<InternalUploadResponse> UploadChunks(
            Stream stream, 
            int? assetId,
            KeepMetadata? keepMetadata, 
            Overwrite? overwrite, 
            string? computerName, 
            string? filename,
            IUploadProgressListener? listener, 
            CancellationToken cancellationToken
        )
        {
            var firstRequest = true;
            var response = new InternalUploadResponse(-1, -1, false);
            while (!response.Finished)
            {
                if (firstRequest)
                {
                    _logger.LogDebug("Uploading first chunk");
                    response = await UploadChunk(stream, assetId, keepMetadata, overwrite, computerName, filename,
                        true, listener, cancellationToken);
                }
                else
                {
                    _logger.LogDebug("Uploading following chunks");
                    response = await UploadChunk(stream, assetId, null, null, null, null,
                        false, listener, cancellationToken);
                }

                firstRequest = false;
                assetId = response.AssetId;
            }
            _logger.LogDebug("Uploaded all chunks");

            return response;
        }

        private async Task<InternalUploadResponse> UploadChunk(
            Stream stream, 
            int? assetId,
            KeepMetadata? keepMetadata, 
            Overwrite? overwrite, 
            string? computerName, 
            string? filename,
            bool firstChunk,
            IUploadProgressListener? listener, 
            CancellationToken cancellationToken
        )
        {
            var (client, request) = _serviceHttpWrapper.GetClientAndRequest(ServiceType.TranscodeService, "api/upload/upload-chunks");
            
            if (assetId != null)
            {
                request.AddQueryParameter("AssetId", assetId.Value);
            }

            if (keepMetadata != null)
            {
                request.AddQueryParameter("MetadataHandling", keepMetadata.Value.ToString());
            }

            if (overwrite != null)
            {
                request.AddQueryParameter("OverwriteHandling", overwrite.Value.ToString());
            }

            if (computerName != null)
            {
                request.AddQueryParameter("ComputerName", computerName);
            }

            if (filename != null)
            {
                request.AddQueryParameter("Filename", filename);
            }

            request.AddQueryParameter("LastChunkIfSizeLessThan", _configuration.UploadChunkSize);
            var ak = await _damAuthenticationService.GetAccessKey();
            request.AddAccessKey(ak);

            using var limitedStream = new LimitedReaderStream(stream, _configuration.UploadChunkSize);
            request.Body = new StreamBody(limitedStream);

            _logger.LogDebug("Sending chunk");
            var response = await client.PostAsync<InternalUploadResponse>(request, cancellationToken);
            _logger.LogDebug("Chunk transferred");

            if (!response.IsSuccessful)
            {
                throw new UploadException($"Failed to upload chunk: {response.StatusCode}", response.Exception);
            }

            var res = response.Data!;
            
            if (listener != null)
            {
                if (firstChunk)
                {
                    await listener.UploadInitiated(res.ItemId, cancellationToken);
                }
                await listener.ChunkUploaded(res.ItemId, stream.Position, cancellationToken);
                if (res.Finished)
                {
                    await listener.FinishedUpload(res.ItemId, cancellationToken);
                }
            }

            return res;
        }
        
        private record InternalUploadResponse(int AssetId, int ItemId, bool Finished);
    }
}
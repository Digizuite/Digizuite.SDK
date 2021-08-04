using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Digizuite.Exceptions;
using Digizuite.Extensions;
using Digizuite.HttpAbstraction;
using Digizuite.Logging;

namespace Digizuite
{
    public class NewUploadService : IUploadService
    {
        private const long SliceSize = 1000 * 1000 * 10;
        private readonly ServiceHttpWrapper _serviceHttpWrapper;
        private readonly ILogger<NewUploadService> _logger;
        private readonly IDamAuthenticationService _damAuthenticationService;

        public NewUploadService(ServiceHttpWrapper serviceHttpWrapper, ILogger<NewUploadService> logger, IDamAuthenticationService damAuthenticationService)
        {
            _serviceHttpWrapper = serviceHttpWrapper;
            _logger = logger;
            _damAuthenticationService = damAuthenticationService;
        }

        public async Task<int> Upload(Stream stream, string filename, string computerName, IUploadProgressListener? listener = null,
            CancellationToken cancellationToken = default)
        {
            _logger.LogDebug("Starting upload", nameof(filename), filename, nameof(computerName), computerName);

            var response = await UploadChunks(stream, null, KeepMetadata.Keep, Overwrite.ReplaceHistoryEntry,
                computerName, filename, listener, cancellationToken);

            return response.ItemId;
        }

        public async Task<int> Replace(Stream stream, string filename, string computerName, int targetAssetId, KeepMetadata keepMetadata,
            Overwrite overwrite, IUploadProgressListener? listener = null, CancellationToken cancellationToken = default)
        {
            var response = await UploadChunks(stream, targetAssetId, keepMetadata, overwrite,
                computerName, filename, listener, cancellationToken);

            return response.ItemId;
        }

        private async Task<UploadResponse> UploadChunks(
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
            var response = new UploadResponse(-1, -1, false);
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

        private async Task<UploadResponse> UploadChunk(
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

            request.AddQueryParameter("LastChunkIfSizeLessThan", SliceSize);
            var ak = await _damAuthenticationService.GetAccessKey();
            request.AddAccessKey(ak);

            using var limitedStream = new LimitedReaderStream(stream, SliceSize);
            request.Body = new StreamBody(limitedStream);

            _logger.LogDebug("Sending chunk");
            var response = await client.PostAsync<UploadResponse>(request, cancellationToken);
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
        
        private record UploadResponse(int AssetId, int ItemId, bool Finished);
    }
}
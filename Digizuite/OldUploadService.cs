using System;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Digizuite.Exceptions;
using Digizuite.Extensions;
using Digizuite.HttpAbstraction;
using Digizuite.Logging;
using Digizuite.Models;
using Newtonsoft.Json;

namespace Digizuite
{
    public class OldUploadService : IUploadService
    {
        private const string UploadEndpoint = "UploadService.js";
        private const string UploadFileChunkEndpoint = "UploadFileChunk.js";
        private readonly IDamAuthenticationService _damAuthenticationService;
        private readonly ServiceHttpWrapper _serviceHttpWrapper;
        private readonly ILogger<OldUploadService> _logger;
        private readonly IConfiguration _configuration;

        public OldUploadService(IDamAuthenticationService damAuthenticationService, ServiceHttpWrapper serviceHttpWrapper,
            ILogger<OldUploadService> logger, IConfiguration configuration)
        {
            _damAuthenticationService = damAuthenticationService;
            _serviceHttpWrapper = serviceHttpWrapper;
            _logger = logger;
            _configuration = configuration;
        }

        public Task<int> Upload(Stream stream, string filename, string computerName,
            IUploadProgressListener? listener = null, CancellationToken cancellationToken = default)
        {
            return InternalUpload(stream, filename, computerName, listener, cancellationToken,
                uploadInfo => FinishUpload(uploadInfo.UploadId, uploadInfo.ItemId, cancellationToken));
        }


        public Task<int> Replace(Stream stream, string filename, string computerName, int targetAssetId,
            KeepMetadata keepMetadata, Overwrite overwrite, IUploadProgressListener? listener = null, CancellationToken cancellationToken = default)
        {
            return InternalUpload(stream, filename, computerName, listener, cancellationToken, 
                uploadInfo =>
                    FinishReplace(uploadInfo.UploadId, uploadInfo.ItemId, targetAssetId, keepMetadata, overwrite, cancellationToken));
        }

        private async Task<int> InternalUpload(Stream stream, string filename, string computerName,
            IUploadProgressListener? listener, CancellationToken cancellationToken,
            Func<InitiateUploadResponse, Task> completeUpload)
        {
            var uploadInfo = await InitiateUpload(filename, computerName, cancellationToken).ConfigureAwait(false);
            if (listener != null)
            {
                await listener.UploadInitiated(uploadInfo.ItemId, cancellationToken).ConfigureAwait(false);
            }

            await UploadFileChunks(stream, uploadInfo.ItemId, listener, cancellationToken).ConfigureAwait(false);

            await completeUpload(uploadInfo).ConfigureAwait(false);

            if (listener != null)
            {
                await listener.FinishedUpload(uploadInfo.ItemId, cancellationToken).ConfigureAwait(false);
            }

            return uploadInfo.ItemId;
        }

        private async Task<InitiateUploadResponse> InitiateUpload(string filename, string computerName,
            CancellationToken cancellationToken = default)
        {
            var ak = await _damAuthenticationService.GetAccessKey().ConfigureAwait(false);

            var (client, request) = _serviceHttpWrapper.GetClientAndRequest(ServiceType.Dmm3bwsv3, UploadEndpoint);
            
            request.AddQueryParameter("method", "InitiateUpload")
                .AddQueryParameter("uploadername", computerName)
                .AddQueryParameter("filename", filename)
                .AddAccessKey(ak);

            _logger.LogTrace("Sending initiate upload", nameof(filename), filename, nameof(computerName), computerName);
            var res = await client.PostAsync<DigiResponse<InitiateUploadResponse>>(request, cancellationToken).ConfigureAwait(false);

            var response = res.Data;

            if (!response!.Success)
            {
                _logger.LogError("Initiate upload failed", nameof(response), response);
                throw new UploadException("Initiate upload failed. Check your logs for more information");
            }

            var item = response.Items[0];

            if (item.ItemId == default)
            {
                _logger.LogError("Initiate upload succeeded, but no itemId was returned");
                throw new UploadException("Request succeeded, but no itemId was returned???");
            }

            return item;
        }

        private async Task UploadFileChunks(Stream stream, int itemId, IUploadProgressListener? listener, CancellationToken cancellationToken)
        {
            var buffer = new byte[_configuration.UploadChunkSize];

            using var webClient = new WebClient();
            long totalUploaded = 0;

            var endOfStream = false;
            while (!endOfStream && !cancellationToken.IsCancellationRequested)
            {
                var read = await AttemptToFillBuffer(stream, buffer).ConfigureAwait(false);
                _logger.LogTrace("Read bytes into buffer", nameof(read), read, nameof(buffer.Length),
                    buffer.Length);
                if (read < buffer.Length)
                {
                    var tempBuffer = new byte[read];
                    Buffer.BlockCopy(buffer, 0, tempBuffer, 0, read);
                    buffer = tempBuffer;
                    endOfStream = true;
                }

                var ak = await _damAuthenticationService.GetAccessKey().ConfigureAwait(false);


                _logger.LogDebug("Sending file chunk", nameof(itemId), itemId, nameof(endOfStream), endOfStream);

                // Restsharp doesn't work for this, so we have to do things to old way
                var uri = new UriBuilder(
                    new Uri(_configuration.GetDmm3Bwsv3Url(), UploadFileChunkEndpoint));
                var finished = endOfStream ? 1 : 0;
                uri.Query =
                    $"accessKey={ak}&itemid={itemId}&jsonresponse=1&finished={finished}";
                var response = await webClient.UploadDataTaskAsync(uri.Uri, "POST", buffer).ConfigureAwait(false);
                var actualResponse = Encoding.UTF8.GetString(response);

                var resp = JsonConvert.DeserializeObject<DigiResponse<object>>(actualResponse);

                if (!resp!.Success)
                {
                    _logger.LogError("Failed to upload file chunk", nameof(resp), resp);
                    throw new UploadException("Failed to upload file chunk");
                }

                _logger.LogDebug("Uploaded file chunk", nameof(itemId), itemId, nameof(actualResponse),
                    actualResponse);

                totalUploaded += read;
                if (listener != null)
                {
                    await listener.ChunkUploaded(itemId, totalUploaded, cancellationToken).ConfigureAwait(false);
                }
            }
            
            cancellationToken.ThrowIfCancellationRequested();
        }

        private async Task FinishUpload(int uploadId, int itemId, CancellationToken cancellationToken)
        {
            var ak = await _damAuthenticationService.GetAccessKey().ConfigureAwait(false);
            _logger.LogDebug("Using AccessKey", nameof(ak), ak);

            var (client, request) = _serviceHttpWrapper.GetClientAndRequest(ServiceType.Dmm3bwsv3, UploadEndpoint);
            request.AddQueryParameter("method", "UploadAsset")
                .AddQueryParameter("itemId", itemId)
                .AddQueryParameter("digiuploadId", uploadId)
                .AddAccessKey(ak);

            _logger.LogTrace("Finishing upload", nameof(uploadId), uploadId, nameof(itemId), itemId);
            var response = await client.PostAsync<DigiResponse<object>>(request, cancellationToken).ConfigureAwait(false);

            _logger.LogDebug("Finished upload", nameof(response), response);
            if (!response.Data!.Success)
            {
                _logger.LogError("Finish upload failed", nameof(response), response);
                throw new UploadException("Finish upload failed");
            }
        }

        private async Task FinishReplace(int uploadId, int itemId, int targetAssetId, KeepMetadata keepMetadata, Overwrite overwrite, CancellationToken cancellationToken)
        {
            var ak = await _damAuthenticationService.GetAccessKey().ConfigureAwait(false);

            var (client, request) = _serviceHttpWrapper.GetClientAndRequest(ServiceType.Dmm3bwsv3, UploadEndpoint);
            
            request.AddQueryParameter("method", "ReplaceAsset")
                .AddQueryParameter("digiUploadId", uploadId)
                .AddQueryParameter("itemId", itemId)
                .AddQueryParameter("targetAssetId", targetAssetId)
                .AddQueryParameter("keepMetadata", keepMetadata == KeepMetadata.Keep)
                .AddQueryParameter("overwrite", overwrite == Overwrite.ReplaceHistoryEntry)
                .AddAccessKey(ak);

            _logger.LogTrace("Finishing replace");
            var response = await client.PostAsync<DigiResponse<object>>(request, cancellationToken).ConfigureAwait(false);
            _logger.LogDebug("Finished replace", nameof(response), response);

            if (!response.Data!.Success)
            {
                _logger.LogError("Finish replace failed", nameof(response), response);
                throw new UploadException("Finish replace failed");
            }
        }


        /// <summary>
        /// Helper method for reading as much data into the buffer as possible
        /// </summary>
        // Modified from here https://stackoverflow.com/questions/7514101/how-do-i-read-exactly-n-bytes-from-a-stream
        private async Task<int> AttemptToFillBuffer(Stream stream, byte[] buffer)
        {
            var offset = 0;
            while (offset < buffer.Length)
            {
                _logger.LogTrace("Reading bytes from stream", nameof(offset), offset, nameof(buffer.Length),
                    buffer.Length);
                var read = await stream.ReadAsync(buffer, offset, buffer.Length - offset).ConfigureAwait(false);
                if (read == 0)
                {
                    _logger.LogTrace("Read zero bytes from stream. This probably means the stream has ended");
                    return offset;
                }

                offset += read;
            }

            return offset;
        }


        [SuppressMessage("ReSharper", "UnusedAutoPropertyAccessor.Local")]
        private class InitiateUploadResponse
        {
            public int ItemId { get; set; }
            public int UploadId { get; set; }
        }
    }
}
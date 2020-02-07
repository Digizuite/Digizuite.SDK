using System;
using System.IO;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Digizuite.Exceptions;
using Digizuite.Helpers;
using Digizuite.Models;
using RestSharp;

namespace Digizuite
{
    public interface IUploadProgressListener
    {
        Task UploadInitiated(int itemId);
        Task ChunkUploaded(int itemId, long totalUploaded);
        Task FinishedUpload(int itemId);
    }
    
    
    public interface IUploadService
    {
        Task<int> Upload(Stream stream, string filename, string computerName, IUploadProgressListener listener);
        Task Replace(Stream stream, int assetId, bool keepMetadata, IUploadProgressListener listener);
    }
    
    public class UploadService : IUploadService
    {
        private const long SliceSize = 1000 * 1000 * 10;
        private const string UploadEndpoint = "UploadService.js";
        private const string UploadFileChunkEndpoint = "UploadFileChunk.js";
        private readonly IDamAuthenticationService _damAuthenticationService;
        private readonly IHttpClientFactory _clientFactory;
        private readonly ILogger<UploadService> _logger;
        private readonly Configuration _configuration;

        public UploadService(IDamAuthenticationService damAuthenticationService, IHttpClientFactory clientFactory, ILogger<UploadService> logger, Configuration configuration)
        {
            _damAuthenticationService = damAuthenticationService;
            _clientFactory = clientFactory;
            _logger = logger;
            _configuration = configuration;
        }

        public async Task<int> Upload(Stream stream, string filename, string computerName, IUploadProgressListener listener)
        {
            var uploadInfo = await InitiateUpload(filename, computerName);
            if (listener != null)
            {
                await listener.UploadInitiated(uploadInfo.ItemId);
            }

            await UploadFileChunks(stream, uploadInfo.ItemId, listener);

            await FinishUpload(uploadInfo.UploadId, uploadInfo.ItemId);

            if (listener != null)
            {
                await listener.FinishedUpload(uploadInfo.ItemId);
            }

            return uploadInfo.ItemId;
        }
        
        
        public async Task Replace(Stream stream, int assetId, bool keepMetadata, IUploadProgressListener listener)
        {
            throw new System.NotImplementedException();
        }


        private async Task<InitiateUploadResponse> InitiateUpload(string filename, string computerName)
        {
            var ak = await _damAuthenticationService.GetAccessKey();
            
            var request = new RestRequest(UploadEndpoint);
            request.AddParameter("method", "InitiateUpload")
                .AddParameter(DigizuiteConstants.AccessKeyParameter, ak)
                .AddParameter("uploadername", computerName)
                .AddParameter("filename", filename)
                .MakeRequestDamSafe();

            var client = _clientFactory.GetRestClient();
            
            _logger.LogTrace("Sending initiate upload", nameof(filename), filename, nameof(computerName), computerName);
            var response = await client.PostAsync<DigiResponse<InitiateUploadResponse>>(request);
            _logger.LogDebug("Initiate upload response", nameof(response), response);

            if (!response.Success)
            {
                _logger.LogError("Initiate upload failed", nameof(response), response);
                throw new UploadException("Initiate upload failed. Check your logs for more information");
            }

            var item = response.Items[0];

            return item;
        }

        private async Task UploadFileChunks(Stream stream, int itemId, IUploadProgressListener listener)
        {
            var buffer = new byte[SliceSize];

            using (var webClient = new WebClient())
            {
                long totalUploaded = 0;

                var endOfStream = false;
                while (!endOfStream)
                {
                    var read = await AttemptToFillBuffer(stream, buffer);
                    _logger.LogTrace("Read bytes into buffer", nameof(read), read, nameof(buffer.Length),
                        buffer.Length);
                    var request = new RestRequest(UploadFileChunkEndpoint);

                    if (read < buffer.Length)
                    {
                        var tempBuffer = new byte[read];
                        Buffer.BlockCopy(buffer, 0, tempBuffer, 0, read);
                        buffer = tempBuffer;
                        endOfStream = true;
                    }

                    var ak = await _damAuthenticationService.GetAccessKey();


                    _logger.LogDebug("Sending file chunk", nameof(itemId), itemId, nameof(endOfStream), endOfStream);

                    // Restsharp doesn't work for this, so we have to do things to old way
                    var uri = new UriBuilder(new Uri(new Uri(_configuration.GetDmm3bwsv3Url()), UploadFileChunkEndpoint));
                    var finished = endOfStream ? 1 : 0;
                    uri.Query =
                        $"{DigizuiteConstants.AccessKeyParameter}={ak}&itemid={itemId}&jsonresponse=1&finished={finished}";
                    var response = await webClient.UploadDataTaskAsync(uri.Uri, "POST", buffer);
                    var actualResponse = Encoding.UTF8.GetString(response);
                    
                    
                    _logger.LogDebug("Uploaded file chunk", nameof(itemId), itemId, nameof(actualResponse), actualResponse);

                    totalUploaded += read;
                    if (listener != null)
                    {
                        await listener.ChunkUploaded(itemId, totalUploaded);
                    }
                }
            }
        }
        
        private async Task FinishUpload(int uploadId, int itemId)
        {
            var ak = await _damAuthenticationService.GetAccessKey();
            var client = _clientFactory.GetRestClient();
            var request = new RestRequest(UploadEndpoint);
            request.AddParameter("method", "UploadAsset")
                .AddParameter(DigizuiteConstants.AccessKeyParameter, ak)
                .AddParameter("itemId", itemId)
                .AddParameter("digiuploadId", uploadId)
                .MakeRequestDamSafe();

            _logger.LogTrace("Finishing upload", nameof(uploadId), uploadId, nameof(itemId), itemId);
            var response = await client.PostAsync<DigiResponse<object>>(request);
            _logger.LogDebug("Finished upload", nameof(response), response);
            if (!response.Success)
            {
                _logger.LogError("Finish upload failed", nameof(response), response);
                throw new UploadException("Finish upload failed");
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
                _logger.LogTrace("Reading bytes from stream", nameof(offset), offset, nameof(buffer.Length), buffer.Length);
                var read = await stream.ReadAsync(buffer, offset, buffer.Length - offset);
                if (read == 0)
                {
                    _logger.LogTrace("Read zero bytes from stream. This probably means the stream has ended");
                    return offset;
                }

                offset += read;
            }

            return offset;
        }
        
        
        private class InitiateUploadResponse
        {
            public int ItemId { get; set; }
            public int UploadId { get; set; }
        }
    }
}
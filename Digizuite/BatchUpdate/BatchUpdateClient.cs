using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Digizuite.BatchUpdate.Models;
using Digizuite.Models;
using Newtonsoft.Json;
using RestSharp;
using ValueType = Digizuite.BatchUpdate.Models.ValueType;

namespace Digizuite.BatchUpdate
{
    public class BatchUpdateClient : IBatchUpdateClient
    {
        private readonly IDamAuthenticationService _authenticationService;
        private readonly IDamRestClient _restClient;
        private readonly IConfiguration _damInfo;

        private readonly ILogger<BatchUpdateClient> _logger;

        public BatchUpdateClient(ILogger<BatchUpdateClient> logger, IDamAuthenticationService authenticationService,
            IConfiguration damInfo, IDamRestClient restClient)
        {
            _logger = logger;
            _authenticationService = authenticationService;
            _damInfo = damInfo;
            _restClient = restClient;
        }

        public async Task<List<BatchUpdateResponse>> ApplyBatch(Batch batch, bool useVersionedMetadata = false)
        {
            if (batch == null)
            {
                throw new ArgumentNullException(nameof(batch));
            }
            var request = CreateBatchRequest(batch);

            _logger.LogInformation("Sending batch request: ", nameof(request), request, nameof(_damInfo.BaseUrl),
                _damInfo.BaseUrl);

            var accessKey = await _authenticationService.GetAccessKey().ConfigureAwait(false);
           
            
            var restRequest = new RestRequest("BatchUpdateService.js");
            restRequest.AddParameter("updateXML", request.UpdateXml)
                .AddParameter("values", request.Values)
                .AddParameter("useVersionedMetadata", useVersionedMetadata);

            var res = await _restClient.Execute<DigiResponse<BatchUpdateResponse>>(Method.POST, restRequest, accessKey).ConfigureAwait(false);
            if (!res.Data.Success)
            {
                _logger.LogError("Batch Update request failed", "response", res.Content);
                throw new Exception("Batch update request failed");
            }

            _logger.LogDebug("Batch update response", "response", res.Content);

            return res.Data.Items;
        }

        private BatchRequestBodyPartial CreateBatchRequest(Batch batch)
        {
            var values = batch.Values;

            var containerFieldIdInc = 0;
            var containers = values.Select(container =>
            {
                var containerFieldName = container.Target.Value;
                var containerFieldId = $"Update_{++containerFieldIdInc}";
                var containerType = container.BatchType;
                var rowId = container.RowId;
                var itemIds = container.ItemIds;
                var baseIds = container.BaseIds;
                var fieldName = container.FieldName;
                var forceDelete = container.ForceDelete;
                var repositoryType = container.RepositoryType;

                var valueFieldIdInc = 0;
                var parts = container.Values.Select(value =>
                {
                    var fieldId = $"{containerFieldId}_{++valueFieldIdInc}";
                    var type = value.ValueType;

                    // #TotallyNotJavascript
                    var actualValues = value.Value;
                    if (actualValues == null)
                        actualValues = new List<object>();
                    else
                        switch (type)
                        {
                            case ValueType.Int:
                            case ValueType.Bool:
                            case ValueType.String:
                                actualValues = new List<object> {actualValues};
                                break;
                        }

                    var updateKey = value.Properties?.ToUpdateKey() ?? "";

                    var xml = $"<{value.FieldName} fieldId=\"{fieldId}\" {updateKey} />";
                    var json = value.ToJsonValue(fieldId, actualValues);

                    return (xml, json);
                }).ToList();

                var xmls = parts.Select(part => part.Item1);
                var containerXml = $@"<{containerFieldName} fieldId=""{containerFieldId}"">
    {string.Join("\n", xmls)}
</{containerFieldName}>";

                var valueJson = new BatchValuesJson
                {
                    Id = containerFieldId,
                    FieldId = containerFieldId,
                    ContainerType = containerType,
                    RowId = rowId,
                    ItemIds = itemIds.Count == 0 ? null : itemIds,
                    BaseIds = baseIds.Count == 0 ? null : baseIds,
                    RepositoryType = repositoryType,
                    Values = parts.Select(part => part.Item2).ToList(),
                    ForceDelete = forceDelete ? true : (bool?) null
                };

                if (fieldName != null) valueJson.FieldName = fieldName;

                return (containerXml, valueJson);
            }).ToList();

            var innerXml = string.Join("\n", containers.Select(container => container.Item1));
            var innerJson = JsonConvert.SerializeObject(containers.Select(container => container.Item2),
                new JsonSerializerSettings
                {
                    NullValueHandling = NullValueHandling.Ignore,
                });

            return new BatchRequestBodyPartial($"<r>{innerXml}</r>", innerJson);
        }

        private class BatchRequestBodyPartial
        {
            public BatchRequestBodyPartial(string updateXml, string values)
            {
                UpdateXml = updateXml;
                Values = values;
            }

            public string UpdateXml { get; set; }
            public string Values { get; set; }

            public override string ToString()
            {
                return $"{nameof(UpdateXml)}: {UpdateXml}, {nameof(Values)}: {Values}";
            }
        }
    }
}
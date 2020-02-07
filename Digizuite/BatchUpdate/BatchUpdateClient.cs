using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Digizuite.BatchUpdate.Models;
using Digizuite.Helpers;
using Digizuite.Models;
using Newtonsoft.Json;
using RestSharp;

namespace Digizuite.BatchUpdate
{
    public class BatchUpdateClient : IBatchUpdateClient
    {
        private readonly IDamAuthenticationService _authenticationService;
        private readonly IHttpClientFactory _clientFactory;
        private readonly IConfiguration _damInfo;

        private readonly ILogger<BatchUpdateClient> _logger;

        public BatchUpdateClient(ILogger<BatchUpdateClient> logger, IDamAuthenticationService authenticationService,
            IConfiguration damInfo, IHttpClientFactory clientFactory)
        {
            _logger = logger;
            _authenticationService = authenticationService;
            _damInfo = damInfo;
            _clientFactory = clientFactory;
        }

        public async Task ApplyBatch(Batch batch,
            bool useVersionedMetadata = false)
        {
            var request = CreateBatchRequest(batch);

            _logger.LogInformation("Sending batch request: ", nameof(request), request, nameof(_damInfo.BaseUrl),
                _damInfo.BaseUrl);

            var accessKey = await _authenticationService.GetAccessKey();
            var client = _clientFactory.GetRestClient();
            var restRequest = new RestRequest("BatchUpdateService.js");
            restRequest.AddParameter("updateXML", request.UpdateXml)
                .AddParameter("values", request.Values)
                .AddParameter("accessKey", accessKey)
                .AddParameter("useVersionedMetadata", useVersionedMetadata);

            restRequest.MakeRequestDamSafe();

            var res = await client.PostAsync<DigiResponse<object>>(restRequest);

            if (!res.Success)
            {
                _logger.LogError("Batch Update request failed", "response", res);
                throw new System.Exception("Batch update request failed");
            }

            _logger.LogDebug("Batch update response", "response", res);
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

                    var updateKey = "";
                    switch (value.Properties)
                    {
                        case null:
                            updateKey = "";
                            break;
                        case BatchItemGuidProperties p:
                            updateKey = $"itemGuid=\"{p.ItemGuid}\"";
                            break;
                        case BatchLabelIdProperties p:
                            updateKey = $"labelId=\"{p.LabelId}\"";
                            break;
                    }

                    var xml = $"<{value.FieldName} fieldId=\"{fieldId}\" {updateKey} />";
                    var json = new BatchValueJsonValue
                    {
                        Type = type,
                        FieldId = fieldId,
                        Values = actualValues
                    };

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
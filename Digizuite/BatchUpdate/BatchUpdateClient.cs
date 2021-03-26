using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Digizuite.BatchUpdate.Models;
using Digizuite.Extensions;
using Digizuite.HttpAbstraction;
using Digizuite.Logging;
using Digizuite.Models;
using Newtonsoft.Json;
using ValueType = Digizuite.BatchUpdate.Models.ValueType;

namespace Digizuite.BatchUpdate
{
    public class BatchUpdateClient : IBatchUpdateClient
    {
        private readonly IDamAuthenticationService _authenticationService;
        private readonly IConfiguration _damInfo;
        private readonly ServiceHttpWrapper _serviceHttpWrapper;

        private readonly ILogger<BatchUpdateClient> _logger;

        public BatchUpdateClient(ILogger<BatchUpdateClient> logger, IDamAuthenticationService authenticationService,
            IConfiguration damInfo, ServiceHttpWrapper serviceHttpWrapper)
        {
            _logger = logger;
            _authenticationService = authenticationService;
            _damInfo = damInfo;
            _serviceHttpWrapper = serviceHttpWrapper;
        }

        public async Task<List<BatchUpdateResponse>> ApplyBatch(Batch batch, bool useVersionedMetadata = false,
            CancellationToken cancellationToken = default)
        {
            if (batch == null)
            {
                throw new ArgumentNullException(nameof(batch));
            }
            var request = CreateBatchRequest(batch);

            _logger.LogInformation("Sending batch request: ", nameof(request), request, nameof(_damInfo.BaseUrl),
                _damInfo.BaseUrl);

            var accessKey = await _authenticationService.GetAccessKey().ConfigureAwait(false);

            var (client, restRequest) =
                _serviceHttpWrapper.GetClientAndRequest(ServiceType.Dmm3bwsv3, "BatchUpdateService.js");

            var parameters = new Dictionary<string, string>
            {
                {"updateXML", request.UpdateXml},
                {"values", request.Values},
                {"useVersionedMetadata", useVersionedMetadata.ToString()}
            };
            restRequest.Body = new FormUrlEncodedBody(parameters);
            restRequest.AddAccessKey(accessKey);
            
            
            var res = await client.PostAsync<DigiResponse<BatchUpdateResponse>>(restRequest,cancellationToken).ConfigureAwait(false);
            if (!res.Data!.Success)
            {
                _logger.LogError("Batch Update request failed", nameof(res), res);
                throw new Exception("Batch update request failed");
            }

            _logger.LogDebug("Batch update response", nameof(res), res);

            return res.Data!.Items;
        }

        private BatchRequestBodyPartial CreateBatchRequest(Batch batch)
        {
            var values = batch.Values;

            var containerFieldIdInc = 0;
            var containers = values.Select(container =>
            {
                var containerFieldName = container.Target;
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
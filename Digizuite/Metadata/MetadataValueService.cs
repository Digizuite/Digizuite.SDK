using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Digizuite.Collections;
using Digizuite.Extensions;
using Digizuite.Logging;
using Digizuite.Metadata.RequestModels;
using Digizuite.Metadata.RequestModels.UpdateModels;
using Digizuite.Metadata.RequestModels.UpdateModels.Values;
using Digizuite.Metadata.ResponseModels;
using Digizuite.Models.Metadata.Fields;

namespace Digizuite.Metadata
{
    public class MetadataValueService : IMetadataValueService
    {
        private readonly IDamAuthenticationService _authenticationService;
        private readonly ILogger<MetadataValueService> _logger;
        private readonly ServiceHttpWrapper _serviceHttpWrapper;

        public MetadataValueService(IDamAuthenticationService authenticationService,
            ILogger<MetadataValueService> logger,
            ServiceHttpWrapper serviceHttpWrapper)
        {
            _authenticationService = authenticationService;
            _logger = logger;
            _serviceHttpWrapper = serviceHttpWrapper;
        }

        /// <inheritdoc />
        public async Task UpdateFields(IEnumerable<int> assetItemId, CancellationToken cancellationToken = default,
            params Field[] fields)
        {
            var updates = fields.Select(CreateUpdateRequests).ToList();

            var itemIds = assetItemId.ToHashSetNetstandard();

            foreach (var update in updates) update.TargetItemIds = itemIds;

            await ApplyUpdate(updates, cancellationToken);
        }

        public async Task ApplyUpdate(IEnumerable<MetadataUpdate> updates,
            CancellationToken cancellationToken = default)
        {
            var accessKey = await _authenticationService.GetAccessKey().ConfigureAwait(false);

            var updateWrapper = new UpdateMetadataRequest
            {
                Updates = updates.ToList()
            };

            var (client, request) =
                _serviceHttpWrapper.GetClientAndRequest(ServiceType.LegacyService, "/api/metadata/update");


            request.AddAccessKey(accessKey)
                .AddJsonBody(updateWrapper);


            var response = await client.ExecutePostAsync(request, cancellationToken).ConfigureAwait(false);

            if (!response.IsSuccessful)
            {
                _logger.LogError(response.ErrorException, "Update metadata request failed", nameof(response.Content), response.Content, nameof(response.StatusCode), response.StatusCode, nameof(response.ErrorMessage), response.ErrorMessage);
                throw new Exception("Update metadata request failed: " + response.Content);
            }
        }

        public async Task<MetadataEditorResponse> GetRawMetadata(int assetItemId, List<int> fieldItemIds,
            int languageId = 0, CancellationToken cancellationToken = default)
        {
            var accessKey = await _authenticationService.GetAccessKey().ConfigureAwait(false);

            var (client, request) =
                _serviceHttpWrapper.GetClientAndRequest(ServiceType.LegacyService, "/api/metadata/editor");

            var requestBody = new GetMetadataRequest
            {
                ItemIds = new List<int> {assetItemId},
                FieldItemIds = fieldItemIds.ToHashSetNetstandard()
            };

            if (languageId != 0) requestBody.Languages.Add(languageId);

            request
                .AddJsonBody(requestBody)
                .AddAccessKey(accessKey);


            var response = await client.ExecutePostAsync<MetadataEditorResponse>(request, cancellationToken)
                .ConfigureAwait(false);
            if (!response.IsSuccessful)
            {
                _logger.LogError(response.ErrorException, "GetMetadata failed", "response", response.Content, nameof(response.StatusCode), response.StatusCode, nameof(response.ErrorMessage), response.ErrorMessage);
                throw new Exception("GetMetadata request failed: " + response.Content);
            }

            if (response.Data.Fields == null || response.Data.Fields.Count == 0)
            {
                _logger.LogError("Request successful, no metafields returned", "response", response.Content,
                    nameof(fieldItemIds), fieldItemIds);
                throw new Exception("Request successful, no metafields returned: " + response.Content);
            }

            _logger.LogTrace("Get metadata response", "response", response.Data);

            return response.Data;
        }

        private static MetadataUpdate CreateUpdateRequests(Field field)
        {
            MetadataUpdate update = field switch
            {
                ComboMetafield comboMetafield => new ComboValueMetadataUpdate
                {
                    ComboValue = comboMetafield.Value == null ? null : 
                        new ExistingCombo(comboMetafield.Value.Id)
                },
                DateTimeMetafield dateTimeMetafield => new DateTimeMetadataUpdate
                {
                    Value = dateTimeMetafield.Value
                },
                EditComboMetafield editComboMetafield => new EditComboValueMetadataUpdate
                {
                    ComboValue = editComboMetafield.Value?.OptionValue
                },
                EditMultiComboMetafield editMultiComboMetafield => new EditMultiComboValueMetadataUpdate
                {
                    ComboValues = editMultiComboMetafield.Value.Select(cv => cv.OptionValue).ToList()
                },
                BitMetafield bitMetafield => new BitMetadataUpdate
                {
                    Value = bitMetafield.Value
                },
                FloatMetafield floatMetafield => new FloatMetadataUpdate
                {
                    Value = floatMetafield.Value
                },
                IntMetafield intMetafield => new IntMetadataUpdate
                {
                    Value = intMetafield.Value
                },
                LinkMetafield linkMetafield => new StringMetadataUpdate
                {
                    Value = linkMetafield.Value
                },
                MasterItemReferenceMetafield masterItemReferenceMetafield => new ItemReferenceMetadataUpdate
                {
                    ItemIds = masterItemReferenceMetafield.Value.Select(v => v.ItemId).ToHashSetNetstandard()
                },
                MultiComboMetafield multiComboMetafield => new MultiComboValueMetadataUpdate
                {
                    ComboValues = multiComboMetafield.Value
                        .Select(cv => new ExistingCombo(cv.Id)).Cast<BaseInputCombo>().ToList()
                },
                NoteMetafield noteMetafield => new StringMetadataUpdate
                {
                    Value = noteMetafield.Value
                },
                SlaveItemReferenceMetafield slaveItemReferenceMetafield => new ItemReferenceMetadataUpdate
                {
                    ItemIds = slaveItemReferenceMetafield.Value.Select(v => v.ItemId).ToHashSetNetstandard()
                },
                StringMetafield stringMetafield => new StringMetadataUpdate
                {
                    Value = stringMetafield.Value
                },
                TreeMetafield treeMetafield => new TreeMetadataUpdate
                {
                    TreeValues = treeMetafield.Value.Select(tv => new ExistingTreeNode(tv.Id))
                        .Cast<BaseTreeNodeUpdate>().ToValueList()
                },
                _ => throw new ArgumentOutOfRangeException(nameof(field))
            };

            update.MetaFieldLabelId = field.LabelId;

            return update;
        }
    }
}
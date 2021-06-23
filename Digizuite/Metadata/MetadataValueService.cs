using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Digizuite.Collections;
using Digizuite.Extensions;
using Digizuite.HttpAbstraction;
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
        public async Task UpdateFields(IEnumerable<int> assetItemId, string? accessKey = null, CancellationToken cancellationToken = default,
            params Field[] fields)
        {
            var updates = fields.Select(CreateUpdateRequests).ToList();

            var itemIds = assetItemId.ToHashSetNetstandard();

            foreach (var update in updates) update.TargetItemIds = itemIds;

            await ApplyUpdate(updates, accessKey, cancellationToken);
        }

        public async Task ApplyUpdate(IEnumerable<MetadataUpdate> updates, string? accessKey = null,
            CancellationToken cancellationToken = default)
        {
            accessKey ??= await _authenticationService.GetAccessKey().ConfigureAwait(false);

            var updateWrapper = new UpdateMetadataRequest
            {
                Updates = updates.ToList()
            };

            var (client, request) =
                _serviceHttpWrapper.GetClientAndRequest(ServiceType.LegacyService, "/api/metadata/update");


            request.AddAccessKey(accessKey)
                .AddJsonBody(updateWrapper);

            var response = await client.PostAsync<object>(request, cancellationToken).ConfigureAwait(false);

            if (!response.IsSuccessful)
            {
                _logger.LogError("Update metadata request failed", nameof(response), response);
                throw new Exception("Update metadata request failed: " + response);
            }
        }

        public async Task<MetadataEditorResponse> GetRawMetadata(GetMetadataRequest requestBody, CancellationToken cancellationToken = default, string? accessKey = null)
        {
            accessKey ??= await _authenticationService.GetAccessKey().ConfigureAwait(false);

            var (client, request) =
                _serviceHttpWrapper.GetClientAndRequest(ServiceType.LegacyService, "/api/metadata/editor");

            request
                .AddJsonBody(requestBody)
                .AddAccessKey(accessKey);


            var response = await client.PostAsync<MetadataEditorResponse>(request, cancellationToken)
                .ConfigureAwait(false);
            if (!response.IsSuccessful)
            {
                _logger.LogError("GetMetadata failed", nameof(response), response);
                throw new Exception("GetMetadata request failed: " + response);
            }

            if (response.Data!.Fields == null || response.Data!.Fields.Count == 0)
            {
                _logger.LogError("Request successful, no metafields returned", nameof(response), response);
                throw new Exception("Request successful, no metafields returned: " + response);
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
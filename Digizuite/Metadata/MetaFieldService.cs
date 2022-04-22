using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

using Digizuite.Extensions;
using Digizuite.HttpAbstraction;
using Digizuite.Logging;
using Digizuite.Metadata.ResponseModels;
using Digizuite.Metadata.ResponseModels.MetaFields;

namespace Digizuite.Metadata
{
    public class MetaFieldService : IMetaFieldService
    {
        private ILogger<MetaFieldService> _logger;
        private ServiceHttpWrapper _serviceHttpWrapper;
        private IDamAuthenticationService _damAuthenticationService;

        public MetaFieldService(ILogger<MetaFieldService> logger, ServiceHttpWrapper serviceHttpWrapper,
            IDamAuthenticationService damAuthenticationService)
        {
            _logger = logger;
            _serviceHttpWrapper = serviceHttpWrapper;
            _damAuthenticationService = damAuthenticationService;
        }

        private MetaFieldResponse CopyMetaFieldResponse(MetaFieldResponse metaFieldResponse, MetaFieldLabelResponse label)
        {
            MetaFieldResponse field = metaFieldResponse switch
            {
                IntMetaFieldResponse => new IntMetaFieldResponse(),
                StringMetaFieldResponse stringMetaFieldResponse => new StringMetaFieldResponse { MaxLength = stringMetaFieldResponse.MaxLength },
                BitMetaFieldResponse => new BitMetaFieldResponse(),
                NoteMetaFieldResponse noteMetaFieldResponse => new NoteMetaFieldResponse { ShowRichTextEditor = noteMetaFieldResponse.ShowRichTextEditor },
                DateTimeMetaFieldResponse dateTimeMetaFieldResponse => new DateTimeMetaFieldResponse { ViewType = dateTimeMetaFieldResponse.ViewType},
                MultiComboValueMetaFieldResponse  => new MultiComboValueMetaFieldResponse(),
                ComboValueMetaFieldResponse comboValueMetaFieldResponse => new ComboValueMetaFieldResponse { ViewType = comboValueMetaFieldResponse.ViewType},
                MasterItemReferenceMetaFieldResponse masterItemReferenceMetaFieldResponse =>
                    new MasterItemReferenceMetaFieldResponse 
                    { 
                        ItemType = masterItemReferenceMetaFieldResponse.ItemType,
                        MaxCount = masterItemReferenceMetaFieldResponse.MaxCount,
                        RelatedMetaFieldLabelId = masterItemReferenceMetaFieldResponse.RelatedMetaFieldLabelId
                    },
                SlaveItemReferenceMetaFieldResponse slaveItemReferenceMetaFieldResponse => 
                    new SlaveItemReferenceMetaFieldResponse
                    {
                        ItemType = slaveItemReferenceMetaFieldResponse.ItemType,
                        RelatedMetaFieldLabelId = slaveItemReferenceMetaFieldResponse.RelatedMetaFieldLabelId
                    },
                FloatMetaFieldResponse => new FloatMetaFieldResponse(),
                LinkMetaFieldResponse => new LinkMetaFieldResponse(),
                TreeMetaFieldResponse treeMetaFieldResponse => new TreeMetaFieldResponse { RecursiveToRoot = treeMetaFieldResponse.RecursiveToRoot },
                EditMultiComboValueMetaFieldResponse => new EditMultiComboValueMetaFieldResponse(),
                EditComboValueMetaFieldResponse => new EditComboValueMetaFieldResponse(),
                _ => throw new ArgumentOutOfRangeException(nameof(metaFieldResponse),
                        $"Unknown metafield value type. Got {metaFieldResponse.GetType()}")
            };

            field.MetafieldId = metaFieldResponse.MetafieldId;
            field.SortIndex = metaFieldResponse.SortIndex;
            field.Required = metaFieldResponse.Required;
            field.Readonly = metaFieldResponse.Readonly;
            field.AutoTranslated = metaFieldResponse.AutoTranslated;
            field.AutoTranslatedOverwriteExisting = metaFieldResponse.AutoTranslatedOverwriteExisting;
            field.VisibilityMetaFieldId = metaFieldResponse.VisibilityMetaFieldId;
            field.VisibilityRegex = metaFieldResponse.VisibilityRegex;
            field.GroupId = metaFieldResponse.GroupId;
            field.Guid = metaFieldResponse.Guid;
            field.ItemId = metaFieldResponse.ItemId;
            field.RestrictToItemType = metaFieldResponse.RestrictToItemType;
            field.Audited = metaFieldResponse.Audited;
#pragma warning disable 612
            field.LabelId = label.LabelId;
            field.LanguageId = label.LanguageId;
            field.Label = label.Label;
#pragma warning restore 612

            return field;
        }

        public async Task<List<MetaFieldResponse>> GetAllMetaFields(string? accessKey = null,
            CancellationToken cancellationToken = default)
        {
            _logger.LogDebug("Loading metafields");
            accessKey ??= await _damAuthenticationService.GetAccessKey();

            var (client, request) =
                _serviceHttpWrapper.GetClientAndRequest(ServiceType.LegacyService, "/api/metafield");

            request.AddAccessKey(accessKey);

            var response = await client.GetAsync<List<MetaFieldResponse>>(request, cancellationToken);

            if (!response.IsSuccessful)
            {
                _logger.LogError("Failed to load metafields", nameof(response), response);
                throw new Exception("Failed to load metafields: " + response);
            }

            var result = new List<MetaFieldResponse>();

            foreach (var field in response.Data!)
            {
                if (field.Labels.Any())
                {
                    result.AddRange(field.Labels.Select(l => CopyMetaFieldResponse(field, l.Value)));
                }
                else
                {
                    result.Add(field);
                }
            }

            _logger.LogDebug("Loaded metafields without issues");

            return result;
        }
    }
}
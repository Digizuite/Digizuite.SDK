using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using Digizuite.BatchUpdate;
using Digizuite.BatchUpdate.Models;
using Digizuite.Models;
using Digizuite.Models.Metadata;
using Digizuite.Models.Metadata.Fields;
using Digizuite.Models.Metadata.Internal;
using Digizuite.Models.Metadata.Values;
using RestSharp;

namespace Digizuite.Metadata
{
    public class MetadataValueService : IMetadataValueService
    {
        private const string DamDateTimeFormat = "dd-MM-yyyy HH:mm:ss";
        private readonly IDamAuthenticationService _authenticationService;
        private readonly IDamRestClient _restClient;
        private readonly ILogger<MetadataValueService> _logger;
        private readonly IBatchUpdateClient _batchUpdateClient;

        public MetadataValueService(IDamAuthenticationService authenticationService,
            ILogger<MetadataValueService> logger, IDamRestClient restClient, IBatchUpdateClient batchUpdateClient)
        {
            _authenticationService = authenticationService;
            _logger = logger;
            _restClient = restClient;
            _batchUpdateClient = batchUpdateClient;
        }


        /// <inheritdoc cref="IMetadataValueService.GetBitMetafield"/>
        public Task<BitMetafield> GetBitMetafield(int assetItemId, int metafieldItemId)
        {
            return SearchAndFind(assetItemId, metafieldItemId, ParseBitMetafield);
        }

        /// <inheritdoc cref="IMetadataValueService.GetComboMetafield"/>
        public Task<ComboMetafield> GetComboMetafield(int assetItemId, int metafieldItemId)
        {
            return SearchAndFind(assetItemId, metafieldItemId, ParseComboMetafield);
        }

        /// <inheritdoc cref="IMetadataValueService.GetEditComboMetafield"/>
        public Task<EditComboMetafield> GetEditComboMetafield(int assetItemId, int metafieldItemId)
        {
            return SearchAndFind(assetItemId, metafieldItemId, ParseEditComboMetafield);
        }

        /// <inheritdoc cref="IMetadataValueService.GetMultiComboMetafield"/>
        public Task<MultiComboMetafield> GetMultiComboMetafield(int assetItemId, int metafieldItemId)
        {
            return SearchAndFind(assetItemId, metafieldItemId, ParseMultiComboMetafield);
        }

        /// <inheritdoc cref="IMetadataValueService.GetEditMultiComboMetafield"/>
        public Task<EditMultiComboMetafield> GetEditMultiComboMetafield(int assetItemId, int metafieldItemId)
        {
            return SearchAndFind(assetItemId, metafieldItemId, ParseEditMultiComboMetafield);
        }

        /// <inheritdoc cref="IMetadataValueService.GetIntMetafield"/>
        public Task<IntMetafield> GetIntMetafield(int assetItemId, int metafieldItemId)
        {
            return SearchAndFind(assetItemId, metafieldItemId, ParseIntMetafield);
        }

        /// <inheritdoc cref="IMetadataValueService.GetStringMetafield"/>
        public Task<StringMetafield> GetStringMetafield(int assetItemId, int metafieldItemId)
        {
            return SearchAndFind(assetItemId, metafieldItemId, ParseStringMetafield);
        }

        /// <inheritdoc cref="IMetadataValueService.GetLinkMetafield"/>
        public Task<LinkMetafield> GetLinkMetafield(int assetItemId, int metafieldItemId)
        {
            return SearchAndFind(assetItemId, metafieldItemId, ParseLinkMetafield);
        }

        /// <inheritdoc cref="IMetadataValueService.GetMoneyMetafield"/>
        public Task<MoneyMetafield> GetMoneyMetafield(int assetItemId, int metafieldItemId)
        {
            return SearchAndFind(assetItemId, metafieldItemId, ParseMoneyMetafield);
        }

        /// <inheritdoc cref="IMetadataValueService.GetFloatMetafield"/>
        public Task<FloatMetafield> GetFloatMetafield(int assetItemId, int metafieldItemId)
        {
            return SearchAndFind(assetItemId, metafieldItemId, ParseFloatMetafield);
        }

        /// <inheritdoc cref="IMetadataValueService.GetUniqueVersionMetafield"/>
        public Task<UniqueVersionMetafield> GetUniqueVersionMetafield(int assetItemId, int metafieldItemId)
        {
            return SearchAndFind(assetItemId, metafieldItemId, ParseUniqueVersionMetafield);
        }

        /// <inheritdoc cref="IMetadataValueService.GetNoteMetafield"/>
        public Task<NoteMetafield> GetNoteMetafield(int assetItemId, int metafieldItemId)
        {
            return SearchAndFind(assetItemId, metafieldItemId, ParseNoteMetafield);
        }

        /// <inheritdoc cref="IMetadataValueService.GetTreeMetafield"/>
        public Task<TreeMetafield> GetTreeMetafield(int assetItemId, int metafieldItemId)
        {
            return SearchAndFind(assetItemId, metafieldItemId, ParseTreeMetafield);
        }

        /// <inheritdoc cref="IMetadataValueService.GetDateTimeMetafield"/>
        public Task<DateTimeMetafield> GetDateTimeMetafield(int assetItemId, int metafieldItemId)
        {
            return SearchAndFind(assetItemId, metafieldItemId, ParseDateTimeMetafield);
        }

        /// <inheritdoc cref="IMetadataValueService.GetMasterItemReferenceMetafield"/>
        public Task<MasterItemReferenceMetafield> GetMasterItemReferenceMetafield(int assetItemId,
            int metafieldItemId)
        {
            return SearchAndFind(assetItemId, metafieldItemId, ParseMasterItemReferenceMetafield);
        }

        /// <inheritdoc cref="IMetadataValueService.GetSlaveItemReferenceMetafield"/>
        public Task<SlaveItemReferenceMetafield> GetSlaveItemReferenceMetafield(int assetItemId,
            int metafieldItemId)
        {
            return SearchAndFind(assetItemId, metafieldItemId, ParseSlaveItemReferenceMetafield);
        }

        /// <inheritdoc cref="IMetadataValueService.GetAllMetadata"/>
        public async Task<List<Field>> GetAllMetadata(int assetItemId, List<int> metaFieldItemIds, int languageId = 0)
        {
            var values = await GetMetadata(assetItemId, metaFieldItemIds, languageId).ConfigureAwait(false);

            return values
                .Select<MetaFieldResponse, Field>(v =>
                {
                    switch (v.MetafieldId.Item_datatypeid)
                    {
                        case MetaFieldDataType.Int:
                            return ParseIntMetafield(v);
                        case MetaFieldDataType.MultiCombo:
                            return ParseMultiComboMetafield(v);
                        case MetaFieldDataType.Url:
                            return ParseLinkMetafield(v);
                        case MetaFieldDataType.Combo:
                            return ParseComboMetafield(v);
                        case MetaFieldDataType.String:
                            return ParseStringMetafield(v);
                        case MetaFieldDataType.Bit:
                            return ParseBitMetafield(v);
                        case MetaFieldDataType.Money:
                            return ParseMoneyMetafield(v);
                        case MetaFieldDataType.DateTime:
                            return ParseDateTimeMetafield(v);
                        case MetaFieldDataType.MultiComboValue:
                            return ParseMultiComboMetafield(v);
                        case MetaFieldDataType.ComboValue:
                            return ParseComboMetafield(v);
                        case MetaFieldDataType.EditComboValue:
                            return ParseEditComboMetafield(v);
                        case MetaFieldDataType.Note:
                            return ParseNoteMetafield(v);
                        case MetaFieldDataType.MasterItemReference:
                            return ParseMasterItemReferenceMetafield(v);
                        case MetaFieldDataType.SlaveItemReference:
                            return ParseSlaveItemReferenceMetafield(v);
                        case MetaFieldDataType.Float:
                            return ParseFloatMetafield(v);
                        case MetaFieldDataType.EditMultiComboValue:
                            return ParseEditMultiComboMetafield(v);
                        case MetaFieldDataType.UniqueVersion:
                            return ParseUniqueVersionMetafield(v);
                        case MetaFieldDataType.Tree:
                            return ParseTreeMetafield(v);
                        case MetaFieldDataType.Link:
                            return ParseLinkMetafield(v);
                        default:
                            _logger.LogWarning("Unknown meta field type", "type", v.MetafieldId.Item_datatypeid);
                            return null;
                    }
                })
                .Where(f => f != null)
                .ToList();
        }

        /// <inheritdoc cref="IMetadataValueService.UpdateFields(int,Digizuite.Models.Metadata.Fields.Field[])"/>
        public Task UpdateFields(int assetItemId, params Field[] fields)
        {
            return UpdateFields(new[] {assetItemId}, fields);
        }

        /// <inheritdoc cref="IMetadataValueService.UpdateFields(System.Collections.Generic.IEnumerable{int},Digizuite.Models.Metadata.Fields.Field[])"/>
        public Task UpdateFields(IEnumerable<int> assetItemId, params Field[] fields)
        {
            var batch = new Batch(new BatchPart
            {
                ItemIds = assetItemId.ToList(),
                Target = FieldType.Asset,
                BatchType = BatchType.ItemIdsValuesRowId,
                RowId = 1,
                Values = fields
                    .Select(GetBatchValue)
                    .Where(v => v != null)
                    .ToList()
            });

            return _batchUpdateClient.ApplyBatch(batch);
        }

        private static BatchValue GetBatchValue(Field field)
        {
            switch (field)
            {
                case BitMetafield bm:
                    return new BoolBatchValue(FieldType.Metafield, bm.Value, new BatchLabelIdProperties(bm.LabelId));
                case DateTimeMetafield dtm:
                    return dtm.Value != null
                        ? new DateTimeBatchValue(FieldType.Metafield, dtm.Value.Value,
                            new BatchLabelIdProperties(field.LabelId))
                        : GetDeleteBatch(field.LabelId);

                case FloatMetafield fm:
                    return fm.Value != null
                        ? new FloatBatchValue(FieldType.Metafield, fm.Value.Value,
                            new BatchLabelIdProperties(field.LabelId))
                        : GetDeleteBatch(field.LabelId);
                
                case ComboMetafield cm:
                    return GetIntBatch(cm.Value?.Value, field.LabelId);
                case MultiComboMetafield mcm:
                    return GetIntListBatch(mcm.Value?.Select(v => v.Value).ToList(), field.LabelId);
                case EditComboMetafield ecm:
                    return GetStringBatch(ecm.Value?.Value, field.LabelId);
                case EditMultiComboMetafield emcm:
                    return GetStringListBatch(emcm.Value?.Select(v => v.Value).ToList(), field.LabelId);
                
                case TreeMetafield m:
                    return GetIntListBatch(m.Value?.Select(v => v.Id).ToList(), field.LabelId);
                
                case Field<int?> f:
                    return GetIntBatch(f.Value, f.LabelId);
                case Field<string> f:
                    return GetStringBatch(f.Value, f.LabelId);
                case Field<List<ItemReferenceOption>> f:
                    return GetIntListBatch(f.Value?.Select(v => v.ItemId).ToList(), field.LabelId);
            }
            
            return null;
        }

        private static BatchValue GetIntBatch(int? value, int labelId)
        {
            if (value == null)
            {
                return GetDeleteBatch(labelId);
            }
            
            return new IntBatchValue(FieldType.Metafield, value.Value, new BatchLabelIdProperties(labelId));
        }

        private static BatchValue GetIntBatch(string value, int labelId)
        {
            if (value == null)
            {
                return GetDeleteBatch(labelId);
            }

            return GetIntBatch(int.Parse(value, NumberFormatInfo.InvariantInfo), labelId);
        }

        private static BatchValue GetIntListBatch(List<int> values, int labelId)
        {
            if (values == null)
            {
                return GetDeleteBatch(labelId);
            }
            
            return new IntListBatchValue(FieldType.Metafield, values, new BatchLabelIdProperties(labelId));
        }

        private static BatchValue GetIntListBatch(List<string> values, int labelId)
        {
            if (values == null)
            {
                return GetDeleteBatch(labelId);
            }

            return GetIntListBatch(values.Select(int.Parse).ToList(), labelId);
        }

        private static BatchValue GetStringBatch(string value, int labelId)
        {
            if (value == null)
            {
                return GetDeleteBatch(labelId);
            }
            
            return new StringBatchValue(FieldType.Metafield, value, new BatchLabelIdProperties(labelId));
        }

        private static BatchValue GetStringListBatch(List<string> value, int labelId)
        {
            if (value == null)
            {
                return GetDeleteBatch(labelId);
            }
            
            return new StringListBatchValue(FieldType.Metafield, value, new BatchLabelIdProperties(labelId));
        }

        private static BatchValue GetDeleteBatch(int labelId)
        {
            return new DeleteBatchValue(FieldType.Metafield, new BatchLabelIdProperties(labelId));
        }

        private async Task<List<MetaFieldResponse>> GetMetadata(int assetItemId, List<int> fieldItemIds,
            int languageId = 0)
        {
            var accessKey = await _authenticationService.GetAccessKey().ConfigureAwait(false);

            var restRequest = new RestRequest("SearchService.js");
            restRequest.AddParameter("SearchName", "GetAllMetafieldAndValues")
                .AddParameter("limit", "9999")
                .AddParameter("page", "1")
                .AddParameter("itemid_note", assetItemId)
                .AddParameter("itemid_value", assetItemId)
                .AddParameter("sir_itemid_value", assetItemId)
                .AddParameter("itemid_note_type_MultiIds", "1")
                .AddParameter("itemid_value_type_MultiIds", "1")
                .AddParameter("sir_itemid_value_type_MultiIds", "1")
                .AddParameter("metafielditemids", string.Join(",", fieldItemIds))
                .AddParameter("metafielditemids_type_MultiIds", "1");

            if (languageId != 0)
            {
                restRequest.AddParameter("language", languageId);
            }

            var response = await _restClient.Execute<DigiResponse<MetaFieldResponse>>(Method.GET, restRequest, accessKey).ConfigureAwait(false);
            if (!response.Data.Success)
            {
                _logger.LogError("GetMetadata failed", "response", response.Content);
                throw new Exception(
                    $"response from api went horrible: Warnings are {response.Data.Warnings}, Errors are {response.Data.Errors}");
            }

            if (response.Data.Items == null)
            {
                _logger.LogError("Request successful, no metafields returned", "response", response.Content,
                    nameof(fieldItemIds), fieldItemIds);
                throw new Exception("Request successful, no metafields returned");
            }

            if (response.Data.Items.Count == 0)
            {
                _logger.LogWarning("Request successful, no metafields returned", nameof(response), response.Content,
                    nameof(fieldItemIds), fieldItemIds);
            }

            _logger.LogTrace("Get metadata response", "response", response.Content);

            return response.Data.Items;
        }

        /// <summary>
        ///     For debug purposes
        /// </summary>
        /// <param name="assetId"></param>
        /// <param name="metafieldItemId"></param>
        /// <returns></returns>
        internal async Task<MetaFieldResponse> GetRawMetadata(int assetId, int metafieldItemId)
        {
            var data = await GetMetadata(assetId, new List<int> {metafieldItemId}).ConfigureAwait(false); 
            return data.Single();
        }

        private async Task<T> SearchAndFind<T>(int assetItemId, int fieldItemId, Func<MetaFieldResponse, T> parse)
        {
            var item = await GetRawMetadata(assetItemId, fieldItemId).ConfigureAwait(false);
            return parse(item);
        }

        /// <summary>
        ///     Converts the horrible digizuite boolean response into an actual boolean
        ///     If the value cannot be parsed, will return false
        /// </summary>
        /// <param name="b"></param>
        /// <returns></returns>
        private static bool ParseBool(string b)
        {
            if (string.Equals(b, "true", StringComparison.InvariantCultureIgnoreCase) || b == "1") return true;

            return false;
        }

        private static void PopulateField<T>(MetaFieldResponse response, Field<T> field)
        {
            var isReadOnly = ParseBool(response.MetafieldId.MetafieldReadonly);
            var hasSecurityAccess = ParseBool(response.MetafieldId.MetafieldSecWriteaccess);
            field.ItemId = response.MetafieldId.MetafieldItemId;
            field.MetafieldId = response.MetafieldId.Metafieldid;
            field.LabelId = response.MetafieldLabelId;
            field.Label = response.MetafieldLabellabel;
            field.LanguageId = response.MetafieldLabellanguageid;
            field.ReadOnly = isReadOnly || !hasSecurityAccess;
            field.Required = ParseBool(response.MetafieldId.MetafieldIsRequired);
            field.SortIndex = response.MetafieldLabelSortindex;
            // Will be set in one of the getter methods, we cannot do it right now.
            field.AutoTranslated = ParseBool(response.MetafieldId.MetafieldAutoTranslate);
            if (int.TryParse(response.MetafieldId.MetafieldVisibilityMetafieldId, out var value))
                field.VisibilityMetaFieldId = value;
        }

        private static BitMetafield ParseBitMetafield(MetaFieldResponse response)
        {
            var metaField = new BitMetafield();
            PopulateField(response, metaField);
            if (response.HasItemMetaFieldValueId())
            {
                var valueId = response.GetSingleMetaFieldValueId();

                metaField.Value = ParseBool(valueId.metaValue);
            }

            return metaField;
        }

        private static ComboMetafield ParseComboMetafield(MetaFieldResponse response)
        {
            var metaField = new ComboMetafield();
            PopulateField(response, metaField);

            if (response.HasItemMetaFieldValueId())
            {
                var value = response.GetSingleMetaFieldValueId();

                metaField.Value = value.ToComboValue();
            }

            metaField.ViewType = response.MetafieldId.MetafieldComboViewType == "1" ? "radio" : "combo";

            return metaField;
        }

        private static EditComboMetafield ParseEditComboMetafield(MetaFieldResponse response)
        {
            var metaField = new EditComboMetafield();
            PopulateField(response, metaField);
            if (response.HasItemMetaFieldValueId())
            {
                var value = response.GetSingleMetaFieldValueId();
                metaField.Value = value.ToEditComboValue();
            }

            return metaField;
        }

        private static MultiComboMetafield ParseMultiComboMetafield(MetaFieldResponse response)
        {
            var metaField = new MultiComboMetafield();
            PopulateField(response, metaField);
            metaField.Value = response.GetMetaFieldValueId().Select(value => value.ToComboValue()).ToList();


            return metaField;
        }

        private static EditMultiComboMetafield ParseEditMultiComboMetafield(MetaFieldResponse response)
        {
            var metaField = new EditMultiComboMetafield();
            PopulateField(response, metaField);

            metaField.Value = response.GetMetaFieldValueId().Select(v => v.ToEditComboValue()).ToList();

            return metaField;
        }

        private static IntMetafield ParseIntMetafield(MetaFieldResponse response)
        {
            var metaField = new IntMetafield();
            PopulateField(response, metaField);

            if (!response.HasItemMetaFieldValueId()) return metaField;
            var value = response.GetSingleMetaFieldValueId();

            if (int.TryParse(value.metaValue, out var v)) metaField.Value = v;


            return metaField;
        }

        private static StringMetafield ParseStringMetafield(MetaFieldResponse response)
        {
            var metaField = new StringMetafield();
            PopulateField(response, metaField);

            if (int.TryParse(response.MetafieldId.MetafieldMaxlength, out var maxLength))
                metaField.MaxLength = maxLength;

            if (response.HasItemMetaFieldValueId())
            {
                var value = response.GetSingleMetaFieldValueId();

                metaField.Value = value.metaValue;
            }

            return metaField;
        }

        private static LinkMetafield ParseLinkMetafield(MetaFieldResponse response)
        {
            var metaField = new LinkMetafield();
            PopulateField(response, metaField);

            if (response.HasItemMetaFieldValueId())
            {
                var value = response.GetSingleMetaFieldValueId();

                metaField.Value = value.metaValue;
            }

            return metaField;
        }

        private static MoneyMetafield ParseMoneyMetafield(MetaFieldResponse response)
        {
            var metaField = new MoneyMetafield();
            PopulateField(response, metaField);

            if (response.HasItemMetaFieldValueId())
            {
                var value = response.GetSingleMetaFieldValueId();

                metaField.Value = value.metaValue;
            }

            return metaField;
        }

        private static FloatMetafield ParseFloatMetafield(MetaFieldResponse response)
        {
            var metaField = new FloatMetafield();
            PopulateField(response, metaField);

            if (response.HasItemMetaFieldValueId())
            {
                var value = response.GetSingleMetaFieldValueId();

                if (double.TryParse(value.metaValue, out var v)) metaField.Value = v;
            }


            return metaField;
        }

        private static NoteMetafield ParseNoteMetafield(MetaFieldResponse response)
        {
            var metaField = new NoteMetafield();
            PopulateField(response, metaField);

            if (response.HasNoteValue())
            {
                var value = response.GetNoteValue();

                metaField.Value = value.Note;
            }

            metaField.IsHtml = ParseBool(response.MetafieldId.Is_html);
            return metaField;
        }

        private static UniqueVersionMetafield ParseUniqueVersionMetafield(MetaFieldResponse response)
        {
            var metaField = new UniqueVersionMetafield();
            PopulateField(response, metaField);

            if (response.HasItemMetaFieldValueId())
            {
                var value = response.GetSingleMetaFieldValueId();

                metaField.Value = new UniqueVersionValue
                {
                    Value = value.metaValue,
                    Version = value.extraValue
                };
            }

            return metaField;
        }

        private static TreeMetafield ParseTreeMetafield(MetaFieldResponse response)
        {
            var metaField = new TreeMetafield();
            PopulateField(response, metaField);

            if (Enum.TryParse<TreeViewType>(response.MetafieldId.Treeview_format, out var format))
                metaField.ViewType = format;

            metaField.RecursiveToRoot = ParseBool(response.MetafieldId.MetafieldRecursiveToRoot);

            metaField.Value = response.GetMetaFieldValueId().Select(v => v.ToTreeValue()).ToList();

            return metaField;
        }

        private static DateTimeMetafield ParseDateTimeMetafield(MetaFieldResponse response)
        {
            var metaField = new DateTimeMetafield();
            PopulateField(response, metaField);

            metaField.SubType = ParseBool(response.MetafieldId.Show_extra_field) ? "datetime" : "date";

            if (response.HasItemMetaFieldValueId())
            {
                var value = response.GetSingleMetaFieldValueId();

                if (DateTime.TryParseExact(value.metaValue, DamDateTimeFormat, DateTimeFormatInfo.InvariantInfo,
                    DateTimeStyles.None, out var date))
                    metaField.Value = date;
            }

            return metaField;
        }

        private static void PopulateItemReferenceFields<T>(ItemReferenceMetaField<T> metaField, MetaFieldResponse response)
        {
            if (int.TryParse(response.MetafieldId.Ref_itemid, out var refItemId)) metaField.RefItemId = refItemId;

            if (int.TryParse(response.MetafieldId.RefItemBaseId, out var refItemBaseId))
                metaField.RefItemBaseId = refItemBaseId;
            metaField.RefItemTitle = response.MetafieldId.RefItemTitle ?? "";

            if (int.TryParse(response.MetafieldReferenceMaxItems, out var maxItems))
                metaField.MaxItems = maxItems;
        }

        private static MasterItemReferenceMetafield ParseMasterItemReferenceMetafield(MetaFieldResponse response)
        {
            var metaField = new MasterItemReferenceMetafield();
            PopulateField(response, metaField);


            if (Enum.TryParse<ReferenceItemType>(response.ReferenceTypeId, out var refType))
                metaField.RefType = refType;

            PopulateItemReferenceFields(metaField, response);

            metaField.Value = response.GetMetaFieldValueId().Select(v => v.ToItemReferenceOption()).ToList();


            return metaField;
        }

        private static SlaveItemReferenceMetafield ParseSlaveItemReferenceMetafield(MetaFieldResponse response)
        {
            var metaField = new SlaveItemReferenceMetafield();
            PopulateField(response, metaField);

            PopulateItemReferenceFields(metaField, response);

            if (int.TryParse(response.MetafieldReferenceMetafieldLabelId, out var refLabelId))
                metaField.RefLabelId = refLabelId;
            
            metaField.Value = response.GetSirValueId().Select(v => v.ToItemReferenceOption()).ToList();
            
            

            return metaField;
        }
    }
}
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using Digizuite.Helpers;
using Digizuite.Models;
using Digizuite.Models.Metadata;
using Digizuite.Models.Metadata.Fields;
using Digizuite.Models.Metadata.Internal;
using Digizuite.Models.Metadata.Values;
using RestSharp;

namespace Digizuite
{
    public class MetaDataService
    {
        private const string DamDateTimeFormat = "dd-MM-yyyy HH:mm:ss";
        private readonly DamAuthenticationService _authenticationService;
        private readonly HttpClientFactory _clientFactory;
        private readonly ILogger<MetaDataService> _logger;

        public MetaDataService(DamAuthenticationService authenticationService, ILogger<MetaDataService> logger, HttpClientFactory clientFactory)
        {
            _authenticationService = authenticationService;
            _logger = logger;
            _clientFactory = clientFactory;
        }


        public async Task<BitMetafield> GetBitMetafield(int assetItemId, int metafieldItemId)
        {
            return await SearchAndFind(assetItemId, metafieldItemId, ParseBitMetafield);
        }

        public async Task<ComboMetafield> GetComboMetafield(int assetItemId, int metafieldItemId)
        {
            return await SearchAndFind(assetItemId, metafieldItemId, ParseComboMetafield);
        }

        public async Task<EditComboMetafield> GetEditComboMetafield(int assetItemId, int metafieldItemId)
        {
            return await SearchAndFind(assetItemId, metafieldItemId, ParseEditComboMetafield);
        }

        public async Task<MultiComboMetafield> GetMultiComboMetafield(int assetItemId, int metafieldItemId)
        {
            return await SearchAndFind(assetItemId, metafieldItemId, ParseMultiComboMetafield);
        }

        public async Task<EditMultiComboMetafield> GetEditMultiComboMetafield(int assetItemId, int metafieldItemId)
        {
            return await SearchAndFind(assetItemId, metafieldItemId, ParseEditMultiComboMetafield);
        }

        public async Task<IntMetafield> GetIntMetafield(int assetItemId, int metafieldItemId)
        {
            return await SearchAndFind(assetItemId, metafieldItemId, ParseIntMetafield);
        }

        public async Task<StringMetafield> GetStringMetafield(int assetItemId, int metafieldItemId)
        {
            return await SearchAndFind(assetItemId, metafieldItemId, ParseStringMetafield);
        }

        public async Task<LinkMetafield> GetLinkMetafield(int assetItemId, int metafieldItemId)
        {
            return await SearchAndFind(assetItemId, metafieldItemId, ParseLinkMetafield);
        }

        public async Task<MoneyMetafield> GetMoneyMetafield(int assetItemId, int metafieldItemId)
        {
            return await SearchAndFind(assetItemId, metafieldItemId, ParseMoneyMetafield);
        }

        public async Task<FloatMetafield> GetFloatMetafield(int assetItemId, int metafieldItemId)
        {
            return await SearchAndFind(assetItemId, metafieldItemId, ParseFloatMetafield);
        }

        public async Task<UniqueVersionMetafield> GetUniqueVersionMetafield(int assetItemId, int metafieldItemId)
        {
            return await SearchAndFind(assetItemId, metafieldItemId, ParseUniqueVersionMetafield);
        }

        public async Task<NoteMetafield> GetNoteMetafield(int assetItemId, int metafieldItemId)
        {
            return await SearchAndFind(assetItemId, metafieldItemId, ParseNoteMetafield);
        }

        public async Task<TreeMetafield> GetTreeMetafield(int assetItemId, int metafieldItemId)
        {
            return await SearchAndFind(assetItemId, metafieldItemId, ParseTreeMetafield);
        }

        public async Task<DateTimeMetafield> GetDateTimeMetafield(int assetItemId, int metafieldItemId)
        {
            return await SearchAndFind(assetItemId, metafieldItemId, ParseDateTimeMetafield);
        }

        public async Task<MasterItemReferenceMetafield> GetMasterItemReferenceMetafield(int assetItemId,
            int metafieldItemId)
        {
            return await SearchAndFind(assetItemId, metafieldItemId, ParseMasterItemReferenceMetafield);
        }

        public async Task<SlaveItemReferenceMetafield> GetSlaveItemReferenceMetafield(int assetItemId,
            int metafieldItemId)
        {
            return await SearchAndFind(assetItemId, metafieldItemId, ParseSlaveItemReferenceMetafield);
        }

        /// <summary>
        /// Gets all the metafields specified for the given asset
        /// </summary>
        /// <param name="assetItemId"></param>
        /// <param name="metaFieldItemIds"></param>
        /// <param name="languageId"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        public async Task<List<Field>> GetAllMetadata(int assetItemId, List<int> metaFieldItemIds, int languageId = 0)
        {
            var values = await GetMetadata(assetItemId, metaFieldItemIds, languageId);

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

        private async Task<List<MetaFieldResponse>> GetMetadata(int assetItemId, List<int> fieldItemIds, int languageId = 0)
        {
            var accessKey = await _authenticationService.GetAccessKey();
            var restClient = _clientFactory.GetRestClient();
            restClient.UseSerializer(new JsonNetSerializer());
            var restRequest = new RestRequest("/dmm3bwsv3/SearchService.js");
            restRequest.AddParameter("SearchName", "GetAllMetafieldAndValues")
                .AddParameter("accessKey", accessKey)
                .AddParameter("limit", "9999")
                .AddParameter("page", "1")
                .AddParameter("itemid_note", assetItemId)
                .AddParameter("itemid_value", assetItemId)
                .AddParameter("itemid_note_type_MultiIds", "1")
                .AddParameter("itemid_value_type_MultiIds", "1")
                .AddParameter("metafielditemids", string.Join(",", fieldItemIds))
                .AddParameter("metafielditemids_type_MultiIds", "1")
                .MakeRequestDamSafe();

            if (languageId != 0)
            {
                restRequest.AddParameter("language", languageId);
            }

            var response = await restClient.GetAsync<DigiResponse<MetaFieldResponse>>(restRequest);
            if (!response.Success)
            {
                _logger.LogError("GetMetadata failed", "response", response);
                throw new Exception(
                    $"response from api went horrible: Warnings are {response.Warnings}, Errors are {response.Errors}");
            }

            if (response.Items == null)
            {
                _logger.LogError("Request successful, no metafields returned", "response", response,
                    nameof(fieldItemIds), fieldItemIds);
                throw new Exception("Request successful, no metafields returned");
            }

            if (response.Items.Count == 0)
            {
                _logger.LogWarning("Request successful, no metafields returned", nameof(response), response,
                    nameof(fieldItemIds), fieldItemIds);
            }

            _logger.LogTrace("Get metadata response", "response", response);

            return response.Items;
        }

        /// <summary>
        ///     For debug purposes
        /// </summary>
        /// <param name="assetId"></param>
        /// <param name="metafieldItemId"></param>
        /// <returns></returns>
        internal async Task<MetaFieldResponse> GetRawMetadata(int assetId, int metafieldItemId)
        {
            return (await GetMetadata(assetId, new List<int> { metafieldItemId })).Single();
        }

        private async Task<T> SearchAndFind<T>(int assetItemId, int fieldItemId, Func<MetaFieldResponse, T> parse)
        {
            var item = (await GetMetadata(assetItemId, new List<int> { fieldItemId })).Single();
            return parse(item);
        }

        /// <summary>
        ///     Converts the horrible digizuite boolean response into an actual boolean
        ///     If the value cannot be parsed, will return false
        /// </summary>
        /// <param name="b"></param>
        /// <returns></returns>
        private bool parseBool(string b)
        {
            if (string.Equals(b, "true", StringComparison.InvariantCultureIgnoreCase) || b == "1") return true;

            return false;
        }

        private void PopulateField<T>(MetaFieldResponse response, Field<T> field)
        {
            var isReadOnly = parseBool(response.MetafieldId.MetafieldReadonly);
            var hasSecurityAccess = parseBool(response.MetafieldId.MetafieldSecWriteaccess);
            field.ItemId = response.MetafieldId.MetafieldItemId;
            field.MetafieldId = response.MetafieldId.Metafieldid;
            field.LabelId = response.MetafieldLabelId;
            field.Label = response.MetafieldLabellabel;
            field.LanguageId = response.MetafieldLabellanguageid;
            field.ReadOnly = isReadOnly || !hasSecurityAccess;
            field.Required = parseBool(response.MetafieldId.MetafieldIsRequired);
            field.SortIndex = response.MetafieldLabelSortindex;
            // Will be set in one of the getter methods, we cannot do it right now.
            field.AutoTranslated = parseBool(response.MetafieldId.MetafieldAutoTranslate);
            if (int.TryParse(response.MetafieldId.MetafieldVisibilityMetafieldId, out var value))
                field.VisibilityMetaFieldId = value;
        }

        private BitMetafield ParseBitMetafield(MetaFieldResponse response)
        {
            var metaField = new BitMetafield();
            PopulateField(response, metaField);
            if (response.HasItemMetaFieldValueId())
            {
                var valueId = response.GetSingleMetaFieldValueId();

                metaField.Value = parseBool(valueId.metaValue);
            }

            return metaField;
        }

        private ComboMetafield ParseComboMetafield(MetaFieldResponse response)
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

        private EditComboMetafield ParseEditComboMetafield(MetaFieldResponse response)
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

        private MultiComboMetafield ParseMultiComboMetafield(MetaFieldResponse response)
        {
            var metaField = new MultiComboMetafield();
            PopulateField(response, metaField);
            metaField.Value = response.GetMetaFieldValueId().Select(value => value.ToComboValue()).ToList();


            return metaField;
        }

        private EditMultiComboMetafield ParseEditMultiComboMetafield(MetaFieldResponse response)
        {
            var metaField = new EditMultiComboMetafield();
            PopulateField(response, metaField);

            metaField.Value = response.GetMetaFieldValueId().Select(v => v.ToEditComboValue()).ToList();

            return metaField;
        }

        private IntMetafield ParseIntMetafield(MetaFieldResponse response)
        {
            var metaField = new IntMetafield();
            PopulateField(response, metaField);

            if (!response.HasItemMetaFieldValueId()) return metaField;
            var value = response.GetSingleMetaFieldValueId();

            if (int.TryParse(value.metaValue, out var v)) metaField.Value = v;


            return metaField;
        }

        private StringMetafield ParseStringMetafield(MetaFieldResponse response)
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

        private LinkMetafield ParseLinkMetafield(MetaFieldResponse response)
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

        private MoneyMetafield ParseMoneyMetafield(MetaFieldResponse response)
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

        private FloatMetafield ParseFloatMetafield(MetaFieldResponse response)
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

        private NoteMetafield ParseNoteMetafield(MetaFieldResponse response)
        {
            var metaField = new NoteMetafield();
            PopulateField(response, metaField);

            if (response.HasNoteValue())
            {
                var value = response.GetNoteValue();

                metaField.Value = value.Note;
            }

            metaField.IsHtml = parseBool(response.MetafieldId.Is_html);
            return metaField;
        }

        private UniqueVersionMetafield ParseUniqueVersionMetafield(MetaFieldResponse response)
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

        private TreeMetafield ParseTreeMetafield(MetaFieldResponse response)
        {
            var metaField = new TreeMetafield();
            PopulateField(response, metaField);

            if (Enum.TryParse<TreeViewType>(response.MetafieldId.Treeview_format, out var format))
                metaField.ViewType = format;

            metaField.RecursiveToRoot = parseBool(response.MetafieldId.MetafieldRecursiveToRoot);

            metaField.Value = response.GetMetaFieldValueId().Select(v => v.ToTreeValue()).ToList();

            return metaField;
        }

        private DateTimeMetafield ParseDateTimeMetafield(MetaFieldResponse response)
        {
            var metaField = new DateTimeMetafield();
            PopulateField(response, metaField);

            metaField.SubType = parseBool(response.MetafieldId.Show_extra_field) ? "datetime" : "date";

            if (response.HasItemMetaFieldValueId())
            {
                var value = response.GetSingleMetaFieldValueId();

                if (DateTime.TryParseExact(value.metaValue, DamDateTimeFormat, DateTimeFormatInfo.InvariantInfo,
                    DateTimeStyles.None, out var date))
                    metaField.Value = date;
            }

            return metaField;
        }

        private void PopulateItemReferenceFields<T>(ItemReferenceMetaField<T> metaField, MetaFieldResponse response)
        {
            if (int.TryParse(response.MetafieldId.Ref_itemid, out var refItemId)) metaField.RefItemId = refItemId;

            if (int.TryParse(response.MetafieldId.RefItemBaseId, out var refItemBaseId))
                metaField.RefItemBaseId = refItemBaseId;
            metaField.RefItemTitle = response.MetafieldId.RefItemTitle ?? "";

            if (int.TryParse(response.MetafieldReferenceMaxItems, out var maxItems))
                metaField.MaxItems = maxItems;
        }

        private MasterItemReferenceMetafield ParseMasterItemReferenceMetafield(MetaFieldResponse response)
        {
            var metaField = new MasterItemReferenceMetafield();
            PopulateField(response, metaField);


            if (Enum.TryParse<ReferenceItemType>(response.ReferenceTypeId, out var refType))
                metaField.RefType = refType;

            PopulateItemReferenceFields(metaField, response);


            //            metaField.RefByMetafieldId = response.MetafieldReferencedMetafieldId.Select(int.Parse).ToList();
            //            metaField.RefByMetafieldLabelId = response.MetafieldReferencedMetafieldLabelId.Select(int.Parse).ToList();

            metaField.Value = response.GetMetaFieldValueId().Select(v => v.ToItemReferenceOption()).ToList();


            return metaField;
        }

        private SlaveItemReferenceMetafield ParseSlaveItemReferenceMetafield(MetaFieldResponse response)
        {
            var metaField = new SlaveItemReferenceMetafield();
            PopulateField(response, metaField);

            PopulateItemReferenceFields(metaField, response);

            if (int.TryParse(response.MetafieldReferenceMetafieldLabelId, out var refLabelId))
                metaField.RefLabelId = refLabelId;

            return metaField;
        }
    }
}

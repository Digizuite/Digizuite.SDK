using System;
using System.Linq;
using System.Threading.Tasks;
using Digizuite.BatchUpdate.BatchBuilder;
using Digizuite.Models.Metadata.Values;
using Digizuite.Models.Search;
using Newtonsoft.Json;

namespace Digizuite.Metadata
{
    public interface IComboValueService
    {
        Task<ComboValueDefinition> CreateComboValue(int metaFieldLabelId, ComboValueDefinition comboValue);
        Task<ComboValueDefinition> UpdateComboValue(ComboValueDefinition comboValue);
        Task DeleteComboValue(int comboValueId);

        Task<ComboValueListResponse> GetComboValuesForMetaField(int metaFieldLabelId,
            int page = 1, int count = int.MaxValue);

        Task<ComboValueListResponse> GetComboValuesForMetaField(ComboValueSearchParameters parameters);
    }

    public class ComboValueService : IComboValueService
    {
        private readonly IBatchBuilderService _batchBuilderService;
        private readonly ISearchService _searchService;

        public ComboValueService(IBatchBuilderService batchBuilderService,
            ISearchService searchService)
        {
            _batchBuilderService = batchBuilderService;
            _searchService = searchService;
        }

        public async Task<ComboValueDefinition> CreateComboValue(int metaFieldLabelId, ComboValueDefinition comboValue)
        {
            if (comboValue == null) throw new ArgumentNullException(nameof(comboValue));

            var response = await _batchBuilderService
                .CreateBatch()
                .Values()
                .Target("metacombo_definition")
                .WithoutUpdateTarget()
                .WithValue("item_metafield_labelid", metaFieldLabelId)
                .WithValue("combovalue", comboValue.Text)
                .WithValue("optionvalue", comboValue.Value)
                .WithValue("sortindex", comboValue.SortIndex)
                .WithValue("ispublic", comboValue.IsPublic)
                .WithValue("visible", comboValue.Visible)
                .Apply()
                .ConfigureAwait(false);

            var res = response[0];

            comboValue.Id = res.BaseId;
            comboValue.ItemId = res.ItemId;

            return comboValue;
        }

        public async Task<ComboValueDefinition> UpdateComboValue(ComboValueDefinition comboValue)
        {
            if (comboValue == null) throw new ArgumentNullException(nameof(comboValue));

            await _batchBuilderService
                .CreateBatch()
                .Values()
                .Target("metacombo_definition")
                .BaseIds(comboValue.Id)
                .WithValue("combovalue", comboValue.Text)
                .WithValue("optionvalue", comboValue.Value)
                .WithValue("sortindex", comboValue.SortIndex)
                .WithValue("ispublic", comboValue.IsPublic)
                .WithValue("visible", comboValue.Visible)
                .Apply()
                .ConfigureAwait(false);

            return comboValue;
        }

        public async Task DeleteComboValue(int comboValueId)
        {
            await _batchBuilderService
                .CreateBatch()
                .Delete()
                .Target("metacombo_definition")
                .BaseIds(comboValueId)
                .EmptyValues()
                .Apply()
                .ConfigureAwait(false);
        }

        public Task<ComboValueListResponse> GetComboValuesForMetaField(int metaFieldLabelId,
            int page = 1, int count = int.MaxValue)
        {
            return GetComboValuesForMetaField(new ComboValueSearchParameters(metaFieldLabelId)
            {
                Page = page,
                PageSize = count
            });
        }

        public async Task<ComboValueListResponse> GetComboValuesForMetaField(ComboValueSearchParameters parameters)
        {
            parameters = new ComboValueSearchParameters(parameters);
            
            
            var searchResponse = await _searchService
                .Search(new SearchParameters<ComboValueSearchResponse>("GetMetafieldComboValues", parameters.Page, parameters.PageSize)
                {
                    {"sfMetafieldLabelId", parameters.MetaFieldLabelId}
                })
                .ConfigureAwait(false);

            var items = searchResponse.Items.Select(c => new ComboValueDefinition
            {
                Id = c.Id,
                Text = c.Text,
                Value = c.Value,
                Visible = c.Visible,
                IsPublic = c.IsPublic,
                ItemId = c.ItemId,
                SortIndex = c.SortIndex
            }).ToList();
            
            return new ComboValueListResponse(parameters, items, searchResponse.Total);
        }

        private class ComboValueSearchResponse
        {
            [JsonProperty("combovalue")] public string Text { get; set; } = default!;

            [JsonProperty("optionvalue")] public string Value { get; set; } = default!;

            [JsonProperty("isPublic")] public bool IsPublic { get; set; } = default!;

            [JsonProperty("visible")] public bool Visible { get; set; } = default!;

            [JsonProperty("itemId")] public int ItemId { get; set; } = default!;

            [JsonProperty("item_combo_valueid")] public int Id { get; set; } = default!;

            [JsonProperty("sortIndex")] public int SortIndex { get; set; } = default!;
        }
    }
}
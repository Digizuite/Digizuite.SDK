using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Digizuite.BatchUpdate;
using Digizuite.BatchUpdate.Models;

namespace Digizuite.Metadata
{
    public interface IMetadataUpdateService
    {
        Task<List<BatchUpdateResponse>> SetComboValues(int itemId, int metafieldId, List<string> comboValues, bool addOnly,
            ExistsCheck existsCheck = ExistsCheck.OptionValue);
    }

    public class MetadataUpdateService : IMetadataUpdateService
    {
        private IBatchUpdateClient _batchUpdateClient;
        private ILogger<MetadataUpdateService> _logger;

        public MetadataUpdateService(IBatchUpdateClient batchUpdateClient, ILogger<MetadataUpdateService> logger)
        {
            _batchUpdateClient = batchUpdateClient;
            _logger = logger;
        }

        public async Task<List<BatchUpdateResponse>> SetComboValues(int itemId, int metafieldId, List<string> comboValues, bool addOnly,
            ExistsCheck existsCheck = ExistsCheck.OptionValue)
        {
            _logger.LogTrace("Setting combo value", nameof(itemId), itemId, nameof(metafieldId), metafieldId,
                nameof(comboValues), comboValues, nameof(addOnly), addOnly, nameof(existsCheck), existsCheck);

            var batch = new Batch(
                new BatchPart
                {
                    Target = FieldType.Asset,
                    ItemIds = {itemId},
                    BatchType = BatchType.ItemIdsValuesRowId,
                    Values = new List<BatchValue>
                    {
                        new BatchValueComboList(FieldType.Metafield, comboValues.Select(c => new ComboCreateValue
                        {
                            ComboValue = c,
                            OptionValue = c,
                            ExistsCheck = existsCheck,
                            Visible = true,
                            IsPublic = true
                        }).ToList(), new BatchIdProperties(metafieldId))
                    }
                }
            );

            return await _batchUpdateClient.ApplyBatch(batch).ConfigureAwait(false);
        }
    }

    public enum ExistsCheck
    {
        Text,
        OptionValue,
        None
    }
}
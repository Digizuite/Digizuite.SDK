using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Digizuite.Models.Metadata.Fields;

namespace Digizuite.Metadata
{
    public interface IMetadataValueService
    {
        /// <summary>
        /// Gets the specified metafield
        /// </summary>
        /// <param name="assetItemId">The itemId of the asset to load metadata for</param>
        /// <param name="metafieldItemId">The itemId of the metafield to load</param>
        Task<BitMetafield> GetBitMetafield(int assetItemId, int metafieldItemId);
        /// <inheritdoc cref="GetBitMetafield"/>
        Task<ComboMetafield> GetComboMetafield(int assetItemId, int metafieldItemId);
        /// <inheritdoc cref="GetBitMetafield"/>
        Task<EditComboMetafield> GetEditComboMetafield(int assetItemId, int metafieldItemId);
        /// <inheritdoc cref="GetBitMetafield"/>
        Task<MultiComboMetafield> GetMultiComboMetafield(int assetItemId, int metafieldItemId);
        /// <inheritdoc cref="GetBitMetafield"/>
        Task<EditMultiComboMetafield> GetEditMultiComboMetafield(int assetItemId, int metafieldItemId);
        /// <inheritdoc cref="GetBitMetafield"/>
        Task<IntMetafield> GetIntMetafield(int assetItemId, int metafieldItemId);
        /// <inheritdoc cref="GetBitMetafield"/>
        Task<StringMetafield> GetStringMetafield(int assetItemId, int metafieldItemId);
        /// <inheritdoc cref="GetBitMetafield"/>
        Task<LinkMetafield> GetLinkMetafield(int assetItemId, int metafieldItemId);
        /// <inheritdoc cref="GetBitMetafield"/>
        Task<MoneyMetafield> GetMoneyMetafield(int assetItemId, int metafieldItemId);
        /// <inheritdoc cref="GetBitMetafield"/>
        Task<FloatMetafield> GetFloatMetafield(int assetItemId, int metafieldItemId);
        /// <inheritdoc cref="GetBitMetafield"/>
        Task<UniqueVersionMetafield> GetUniqueVersionMetafield(int assetItemId, int metafieldItemId);
        /// <inheritdoc cref="GetBitMetafield"/>
        Task<NoteMetafield> GetNoteMetafield(int assetItemId, int metafieldItemId);
        /// <inheritdoc cref="GetBitMetafield"/>
        Task<TreeMetafield> GetTreeMetafield(int assetItemId, int metafieldItemId);
        /// <inheritdoc cref="GetBitMetafield"/>
        Task<DateTimeMetafield> GetDateTimeMetafield(int assetItemId, int metafieldItemId);
        
        /// <inheritdoc cref="GetBitMetafield"/>
        Task<MasterItemReferenceMetafield> GetMasterItemReferenceMetafield(int assetItemId,
            int metafieldItemId);
        
        /// <inheritdoc cref="GetBitMetafield"/>
        Task<SlaveItemReferenceMetafield> GetSlaveItemReferenceMetafield(int assetItemId,
            int metafieldItemId);

        /// <summary>
        /// Gets all the metafields specified for the given asset
        /// </summary>
        /// <param name="assetItemId"></param>
        /// <param name="metaFieldItemIds"></param>
        /// <param name="languageId"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        Task<List<Field>> GetAllMetadata(int assetItemId, List<int> metaFieldItemIds, int languageId = 0);

        /// <summary>
        /// Updates the value of all the specified fields for the given asset.
        /// 
        /// Does NOT change the definition of the field itself
        /// </summary>
        /// <param name="assetItemId">The item id of the asset to update</param>
        /// <param name="fields">All the fields with their values</param>
        Task UpdateFields(IEnumerable<int> assetItemId, params Field[] fields);
        /// <inheritdoc cref="UpdateFields(System.Collections.Generic.IEnumerable{int},Digizuite.Models.Metadata.Fields.Field[])"/>
        Task UpdateFields(int assetItemId, params Field[] fields);
    }
}
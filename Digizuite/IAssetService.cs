using System.Threading.Tasks;
using Digizuite.Models;
using Digizuite.Models.Search;

namespace Digizuite
{
    /// <summary>
    ///     A high level utility for dealing with assets
    ///     If you need to work with specific asset models, look at <see cref="ISearchService" /> instead
    /// </summary>
    public interface IAssetService
    {
        /// <summary>
        ///     Gets the specific asset from the Digizuite
        /// </summary>
        /// <param name="itemId">The item id of the asset</param>
        Task<Asset> GetAssetByItemId(int itemId);

        /// <summary>
        ///     Gets the specific asset from the Digizuite
        /// </summary>
        /// <param name="assetId">The asset id of the asset</param>
        Task<Asset> GetAssetByAssetId(int assetId);

        /// <summary>
        ///     Gets the previous versions of this asset
        /// </summary>
        /// <param name="itemId">The item id of the asset</param>
        /// <returns>All the previous versions, with the newest version first, and the oldest last</returns>
        Task<SearchResponse<Asset>> GetAssetVersionsByItemId(int itemId);

        /// <summary>
        ///     Gets the previous versions of this asset
        /// </summary>
        /// <param name="assetId">The asset id of the asset</param>
        /// <returns>All the previous versions, with the newest version first, and the oldest last</returns>
        Task<SearchResponse<Asset>> GetAssetVersionsByAssetId(int assetId);

        /// <summary>
        ///     Gets the assets matching the specified search parameters
        /// </summary>
        Task<SearchResponse<Asset>> GetAssets(SearchParameters? parameters = null);

        /// <summary>
        ///     Deletes the specified asset
        /// </summary>
        /// <param name="itemId">The asset id</param>
        /// <param name="deleteType">What kind of delete should be done</param>
        /// <param name="accepts">Only required if you are doing a hard delete</param>
        Task DeleteAsset(int itemId, AssetDeleteType deleteType, AcceptsConsequence accepts = AcceptsConsequence.No);

        /// <summary>
        ///     Creates a new meta asset (An asset without an attached file)
        /// </summary>
        /// <param name="title">The title/name of the asset</param>
        /// <param name="catalogFolderId">The id of the catalog folder to create the asset in</param>
        /// <returns></returns>
        Task<Asset> CreateMetaAsset(string title, int catalogFolderId);
    }

    /// <summary>
    ///     How should the asset be deleted
    /// </summary>
    public enum AssetDeleteType
    {
        /// <summary>
        ///     The asset will be removed from everywhere, and will not really be accessible, however it can be restored.
        /// </summary>
        Soft,

        /// <summary>
        ///     The asset will be permanently deleted with NO way of restoring
        /// </summary>
        Hard
    }

    /// <summary>
    ///     Used to indicate if you accept a hard delete. Is ignored when doing a soft delete
    /// </summary>
    public enum AcceptsConsequence
    {
        /// <summary>
        ///     You don't accept the consequences, and thus will not be allowed to hard delete an asset
        /// </summary>
        No,

        /// <summary>
        ///     You accept to consequences of your actions, and are aware that there is no way to
        ///     recover the asset afterwards.
        /// </summary>
        // ReSharper disable once InconsistentNaming
        IHaveReadTheConditionsAndAcceptThereIsNoWayToRecoverTheAssetAfterThisOperation
    }
}
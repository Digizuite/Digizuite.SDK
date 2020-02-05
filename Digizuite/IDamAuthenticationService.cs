using System.Threading.Tasks;

namespace Digizuite
{
    public interface IDamAuthenticationService
    {
        /// <summary>
        ///     Gets the active access key for the system user
        /// </summary>
        /// <param name="forceNew">
        ///     If true, a new access key will be generated,
        ///     even if the old one is still considered valid
        /// </param>
        Task<string> GetAccessKey(bool forceNew = false);

        /// <summary>
        ///     Gets the member id of the authenticated user
        /// </summary>
        /// <param name="forceNew"></param>
        /// <returns></returns>
        Task<int> GetMemberId(bool forceNew = false);
    }
}
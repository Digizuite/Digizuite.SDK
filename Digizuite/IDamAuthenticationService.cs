using System.Threading.Tasks;

namespace Digizuite
{
    public interface IDamAuthenticationService
    {
        /// <summary>
        ///     Gets the active access key for the system user
        /// </summary>
        Task<string> GetAccessKey();

        /// <summary>
        ///     Gets the member id of the authenticated user
        /// </summary>
        Task<int> GetMemberId();
    }
}
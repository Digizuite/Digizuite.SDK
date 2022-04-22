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
        /// Impersonate memberId with Options
        /// </summary>
        /// <param name="memberId">memberId to impersonate</param>
        /// <param name="options">AccessKeyOptions to use</param>
        /// <returns>a new impersonated accessKey</returns>
        /// <remarks>
        /// Calling user needs CanImpersonate role
        /// </remarks>
        Task<string> Impersonate(int memberId, DamAuthenticationService.AccessKeyOptions options);
        /// <summary>
        ///     Gets the member id of the authenticated user
        /// </summary>
        Task<int> GetMemberId();
    }
}
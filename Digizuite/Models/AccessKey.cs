using System;

namespace Digizuite.Models
{
    /// <summary>
    /// Describes all the details around an access key
    /// </summary>
    public class AccessKey
    {
        /// <summary>
        /// The underlying guid
        /// </summary>
        public string Token { get; }
        
        /// <summary>
        /// When this access key expires
        /// </summary>
        public DateTimeOffset Expiration { get; set; }
        
        /// <summary>
        /// The id of the member for who this access key is valid
        /// </summary>
        public int MemberId { get; set; }
        
        /// <summary>
        /// The id of the language for this access key
        /// </summary>
        public int LanguageId { get; set; }
        
        /// <summary>
        /// The config version this access key was created for
        /// </summary>
        public string ConfigVersionId { get; set; }

        /// <summary>
        /// Indicates if it is this exact version (I know, horrible naming)
        /// </summary>
        public bool FirstVersion { get; set; }
        
        /// <summary>
        /// Creates a new access key
        /// </summary>
        public AccessKey(string token, DateTimeOffset expiration, int memberId, int languageId, string configVersionId, bool firstVersion)
        {
            Token = token;
            Expiration = expiration;
            MemberId = memberId;
            LanguageId = languageId;
            ConfigVersionId = configVersionId;
            FirstVersion = firstVersion;
        }

        /// <summary>
        /// Checks if the current access key is valid
        /// </summary>
        /// <returns></returns>
        public bool IsValid()
        {
            return Expiration > DateTimeOffset.Now;
        }

        public string GetSpecificConfigId()
        {
            return FirstVersion ? '!' + ConfigVersionId : ConfigVersionId;
        }
    }
}
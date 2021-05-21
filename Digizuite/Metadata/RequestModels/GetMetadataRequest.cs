using System;
using System.Collections.Generic;

namespace Digizuite.Metadata.RequestModels
{
    public class GetMetadataRequest
    {
        /// <summary>
        /// The itemIds of the items to load metadata for
        /// </summary>
        public List<int> ItemIds { get; set; } = new();

        /// <summary>
        /// The labelId's of the fields to load
        /// </summary>
        public HashSet<int> LabelIds { get; set; } = new();

        /// <summary>
        /// The item ids of the fields to load
        /// </summary>
        public HashSet<int> FieldItemIds { get; set; } = new();

        /// <summary>
        /// The item guids of the fields to load
        /// </summary>
        public HashSet<Guid> FieldItemGuids { get; set; } = new();

        /// <summary>
        /// The languages to load fields in, if FieldItemIds or FieldItemGuids is specified.
        /// If no value is specified for this list, then the current AccessKey language is used
        /// </summary>
        public HashSet<int> Languages { get; set; } = new();
        
        /// <summary>
        /// How should the api deal with auto translate cases.
        /// </summary>
        public AutoTranslateHandling AutoTranslateHandling { get; set; } = AutoTranslateHandling.OnlyPrimaryLanguage;
    }
    
    
    public enum AutoTranslateHandling
    {
        /// <summary>
        /// Only the access key language is returned. All values are the same across all languages
        /// anyway, and only one language needs to be shown in editor with support for this
        /// </summary>
        OnlyPrimaryLanguage,
        /// <summary>
        /// All requested languages are returned, even if the field is "AutoTranslateOverwriteExisting",
        /// and all values are the same.
        /// </summary>
        AllLanguages
    }
}
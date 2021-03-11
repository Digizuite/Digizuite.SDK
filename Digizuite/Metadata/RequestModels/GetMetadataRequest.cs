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
    }
}
using System.Collections.Generic;
using Digizuite.Metadata.RequestModels.UpdateModels;

namespace Digizuite.Metadata.RequestModels
{
    public class UpdateMetadataRequest
    {
        public List<MetadataUpdate> Updates { get; set; } = new();
    }
}

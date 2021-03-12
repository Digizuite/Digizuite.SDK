using System;
using Digizuite.Models.Metadata;

namespace Digizuite.Metadata.ResponseModels.Metadata
{
    public record DateTimeMetadataResponse(DateTime? Value) : MetadataResponse
    {
        public override MetaFieldDataType Type => MetaFieldDataType.DateTime;
    }
}
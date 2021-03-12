﻿using Digizuite.Collections;
using Digizuite.Metadata.ResponseModels.Properties;
using Digizuite.Models.Metadata;

namespace Digizuite.Metadata.ResponseModels.Metadata
{
    public record MultiComboValueMetadataResponse(ValueList<ComboValueResponse> Values) : MetadataResponse
    {
        public override MetaFieldDataType Type => MetaFieldDataType.MultiComboValue;
    }
}
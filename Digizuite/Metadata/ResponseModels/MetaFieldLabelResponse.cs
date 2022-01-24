using System;
using System.Collections.Generic;
using System.Text;

namespace Digizuite.Metadata.ResponseModels
{
    public record MetaFieldLabelResponse(
        int LabelId,
        int LanguageId,
        int MetafieldId,
        string Label,
        string Description)
    {
    }
}

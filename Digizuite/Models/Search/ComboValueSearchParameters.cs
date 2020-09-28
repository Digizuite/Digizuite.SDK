using System;

namespace Digizuite.Models.Search
{
    public class ComboValueSearchParameters : PagedParameters
    {
        public ComboValueSearchParameters(int metaFieldLabelId)
        {
            MetaFieldLabelId = metaFieldLabelId;
        }

        public ComboValueSearchParameters(ComboValueSearchParameters parameters)
        {
            if (parameters == null) throw new ArgumentNullException(nameof(parameters));

            Page = parameters.Page;
            PageSize = parameters.PageSize;
            MetaFieldLabelId = parameters.MetaFieldLabelId;
        }

        public int MetaFieldLabelId { get; set; }
    }
}
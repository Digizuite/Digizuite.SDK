using System.Collections.Generic;
using Digizuite.Models.Metadata.Values;

namespace Digizuite.Models.Search
{
    public class ComboValueListResponse : PagedResponse<ComboValueDefinition, ComboValueSearchParameters>
    {
        public ComboValueListResponse(ComboValueSearchParameters parameters, IReadOnlyList<ComboValueDefinition> items,
            int total) : base(parameters, items, total)
        {
        }

        protected override ComboValueSearchParameters GetParametersForPage(int page)
        {
            return new ComboValueSearchParameters(Parameters)
            {
                Page = page
            };
        }
    }
}
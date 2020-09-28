using System.Collections.Generic;

namespace Digizuite.BatchUpdate.BatchBuilder
{
    public interface IBatchBuilderWithoutUpdateTarget
    {
        IBatchBuilderWithoutValue BaseIds(IEnumerable<int> baseIds);
        IBatchBuilderWithoutValue ItemIds(IEnumerable<int> itemIds);
        IBatchBuilderWithoutValue WithoutUpdateTarget();
    }
}
using System.Collections.Generic;
using Digizuite.BatchUpdate.Models;

namespace Digizuite.BatchUpdate.BatchBuilder
{
    public interface IBatchBuilderValues
    {
        IApplyableBatchBuilder WithValue(string fieldName, string value, IBatchProperties? properties = null);
        IApplyableBatchBuilder WithValue(string fieldName, bool value, IBatchProperties? properties = null);
        IApplyableBatchBuilder WithValue(string fieldName, int value, IBatchProperties? properties = null);
        IApplyableBatchBuilder WithValue(string fieldName, IEnumerable<string> value, IBatchProperties? properties = null);
        IApplyableBatchBuilder WithValue(string fieldName, IEnumerable<int> value, IBatchProperties? properties = null);
    }
}
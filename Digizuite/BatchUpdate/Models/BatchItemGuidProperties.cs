namespace Digizuite.BatchUpdate.Models
{
    public class BatchItemGuidProperties : IBatchProperties
    {
        public string ItemGuid { get; }

        public BatchItemGuidProperties(string itemGuid)
        {
            ItemGuid = itemGuid;
        }

        public string ToUpdateKey()
        {
            return $"itemGuid=\"{ItemGuid}\"";
        }
    }
}
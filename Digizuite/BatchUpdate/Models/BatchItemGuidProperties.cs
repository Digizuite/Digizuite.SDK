namespace Digizuite.BatchUpdate.Models
{
    public class BatchItemGuidProperties : IBatchProperties
    {
        public string ItemGuid;

        public BatchItemGuidProperties(string itemGuid)
        {
            ItemGuid = itemGuid;
        }
    }
}
using System.Collections.Generic;

namespace Digizuite.BatchUpdate.Models
{
    public class FolderBatchValue : BatchValue
    {
        public FolderBatchValue(FieldType fieldName, int folderId, IBatchProperties properties,
            RepositoryType repositoryType
        ) : this(fieldName, new List<int> {folderId, (int) repositoryType}, properties)
        {
        }

        public FolderBatchValue(FieldType fieldType, int folderId, IBatchProperties properties,
            RepositoryType repositoryType, bool delete) : this(fieldType,
            new List<int> {folderId, (int) repositoryType, delete ? 1 : 0}, properties)
        {
        }

        public FolderBatchValue(FieldType fieldName, List<int> values, IBatchProperties properties) : base(fieldName,
            values, properties)
        {
        }

        public override ValueType ValueType => ValueType.Folder;
    }
}
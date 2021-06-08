namespace Digizuite.Metadata.RequestModels.UpdateModels
{
    public abstract record MultiMetadataUpdate : MetadataUpdate
    {
        /// <summary>
        /// How should values be handled. Check the docs for the individual enums for descriptions. 
        /// </summary>
        public MultiUpdateMethod UpdateMethod { get; set; } = MultiUpdateMethod.Set;
    }
}
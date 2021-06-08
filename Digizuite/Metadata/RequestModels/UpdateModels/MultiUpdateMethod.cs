namespace Digizuite.Metadata.RequestModels.UpdateModels
{
    /// <summary>
    /// How updates to metafield with multiple values selected at the same time should be done
    /// </summary>
    public enum MultiUpdateMethod
    {
        /// <summary>
        /// Specifically sets the selected values, and removes all other
        /// </summary>
        Set,
        /// <summary>
        /// Merges the existing values with the new values
        /// </summary>
        Merge,
        /// <summary>
        /// Removes the provided values from the existing ones
        /// </summary>
        Unset,
    }
}
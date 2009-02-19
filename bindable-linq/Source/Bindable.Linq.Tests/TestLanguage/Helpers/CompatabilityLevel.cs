
namespace Bindable.Linq.Tests.TestLanguage.Helpers
{
    /// <summary>
    /// Specifies the levels of compatability between a Bindable LINQ query and 
    /// standard LINQ to Objects.
    /// </summary>
    public enum CompatabilityLevel
    {
        /// <summary>
        /// The queries, including items, groups and order, should be the same.
        /// </summary>
        FullyCompatible,
        /// <summary>
        /// The queries should be the same, but differences in the ordering of items
        /// or child items should be ignored, so long as all items are present.
        /// </summary>
        FullyCompatibleExceptOrdering
    }
}

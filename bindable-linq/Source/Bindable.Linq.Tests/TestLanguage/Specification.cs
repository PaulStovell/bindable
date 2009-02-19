using Bindable.Linq.Tests.TestLanguage.Specifications;

namespace Bindable.Linq.Tests.TestLanguage
{
    /// <summary>
    /// A factory class for defining a specification.
    /// </summary>
    internal static class Specification
    {
        /// <summary>
        /// Sets the title of a specification and returns a specification with no input 
        /// or output types specified yet.
        /// </summary>
        /// <param name="title">The title.</param>
        /// <returns></returns>
        public static UntypedSpecification Title(string title)
        {
            return new UntypedSpecification(title);
        }
    }
}

namespace Bindable.Linq.Tests.TestLanguage.Specifications
{
    /// <summary>
    /// Represents a specification which has a title, but no input/output information 
    /// specified.
    /// </summary>
    internal class UntypedSpecification
    {
        private readonly string _title;

        /// <summary>
        /// Initializes a new instance of the <see cref="UntypedSpecification"/> class.
        /// </summary>
        /// <param name="title">The title.</param>
        public UntypedSpecification(string title)
        {
            _title = title;
        }

        /// <summary>
        /// Allows the input types to be specified, resulting in a specification with known 
        /// input types but unknown output types.
        /// </summary>
        /// <typeparam name="TInput">The type of the input.</typeparam>
        /// <returns></returns>
        public FullSpecification<TInput> TestingOver<TInput>()
        {
            return new FullSpecification<TInput>(_title);
        }
    }
}

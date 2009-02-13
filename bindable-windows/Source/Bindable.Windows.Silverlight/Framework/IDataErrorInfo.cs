
namespace Bindable.Windows.Framework
{
    /// <summary>
    /// A definition for IDataErrorInfo, as Silverlight does not support it currently.
    /// Provides the functionality to offer custom error information that a user interface can bind to.
    /// </summary>
    public interface IDataErrorInfo
    {
        /// <summary>Gets an error message indicating what is wrong with this object.</summary>
        /// <returns>An error message indicating what is wrong with this object. The default is an empty string ("").</returns>
        string Error { get; }

        /// <summary>Gets the error message for the property with the given name.</summary>
        /// <returns>The error message for the property. The default is an empty string ("").</returns>
        /// <param name="columnName">The name of the property whose error message to get. </param>
        string this[string columnName] { get; }
    }
}

namespace Bindable.Linq.Interfaces
{
    /// <summary>
    /// Implemented by objects that provide a Refresh method.
    /// </summary>
    public interface IRefreshable
    {
        /// <summary>
        /// Refreshes the object.
        /// </summary>
        void Refresh();
    }
}
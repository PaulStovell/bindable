namespace Bindable.Linq.Tests.MockObjectModel
{
    /// <summary>
    /// A summary of a contact, used for projection.
    /// </summary>
    public class ContactSummary : BindableObject
    {
        /// <summary>
        /// Gets or sets the summary.
        /// </summary>
        /// <value>The summary.</value>
        public string Summary { get; set; }

        public override bool Equals(object obj)
        {
            return Summary.Equals(((ContactSummary)obj).Summary);
        }

        public bool Equals(ContactSummary obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            return Equals(obj.Summary, Summary);
        }

        public override int GetHashCode()
        {
            return (Summary != null ? Summary.GetHashCode() : 0);
        }
    }
}
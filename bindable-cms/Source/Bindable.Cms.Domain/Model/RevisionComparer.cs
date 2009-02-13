using System.Collections.Generic;

namespace Bindable.Cms.Domain.Model
{
    public class RevisionComparer : IComparer<Revision>
    {
        public int Compare(Revision x, Revision y)
        {
            return -(x.RevisionDateUtc.CompareTo(y.RevisionDateUtc));
        }
    }
}
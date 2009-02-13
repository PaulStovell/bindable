using System.Collections.Generic;

namespace PaulStovell.Domain.Model
{
    public class RevisionComparer : IComparer<Revision>
    {
        public int Compare(Revision x, Revision y)
        {
            return -(x.RevisionDateUtc.CompareTo(y.RevisionDateUtc));
        }
    }
}
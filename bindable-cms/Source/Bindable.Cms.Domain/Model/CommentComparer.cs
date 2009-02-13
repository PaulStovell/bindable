using System.Collections.Generic;

namespace Bindable.Cms.Domain.Model
{
    public class CommentComparer : IComparer<Comment>
    {
        public int Compare(Comment x, Comment y)
        {
            return -(x.PostedDateUtc.CompareTo(y.PostedDateUtc));
        }
    }
}
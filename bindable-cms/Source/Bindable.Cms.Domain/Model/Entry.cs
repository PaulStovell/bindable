using System;
using System.Collections.Generic;
using System.Linq;
using Iesi.Collections.Generic;
using Bindable.Cms.Domain.Framework;

namespace Bindable.Cms.Domain.Model
{
    public class Entry : IPostSave
    {
        public Entry()
        {
            Revisions = new HashedSet<Revision>();
            Comments = new HashedSet<Comment>();
        }

        public virtual int Id { get; private set; }
        public virtual string Name { get; set; }
        public virtual string Title { get; set; }
        public virtual string Summary { get; set; }
        public virtual Wiki Wiki { get; set; }
        public virtual IEnumerable<Revision> Revisions { get; private set; }
        public virtual IEnumerable<Comment> Comments { get; private set; }
        public virtual bool IsActive { get; set; }
        
        public virtual Revision LatestRevision
        {
            get { return Revisions.FirstOrDefault(); }
        }

        public virtual Revision CreateRevision()
        {
            var result = null as Revision;
            if (Revisions.Count() == 0)
            {
                result = new Revision();
            }
            else
            {
                result = (Revision)Revisions.First().Clone();
            }
            result.RevisionDateUtc = DateTime.Now.ToUniversalTime();
            result.Entry = this;
            ((ISet<Revision>)Revisions).Add(result);
            return result;
        }

        public virtual Comment CreateComment()
        {
            var result = new Comment();
            result.PostedDateUtc = DateTime.Now.ToUniversalTime();
            result.Entry = this;
            ((ISet<Comment>)Comments).Add(result);
            return result;
        }

        void IPostSave.OnSaved()
        {
            foreach (var revision in Revisions)
            {
                revision.Seal();
            }
        }
    }
}
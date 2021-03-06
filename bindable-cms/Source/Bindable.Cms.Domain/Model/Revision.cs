using System;
using Bindable.Aspects.Parameters;
using Bindable.Cms.Domain.Framework;
using Bindable.Cms.Domain.Framework.Aspects;

namespace Bindable.Cms.Domain.Model
{
    public class Revision : Entity, IComparable<Revision>, ICloneable
    {
        public virtual int Id { get; [OneTime][Seal]set; }
        public virtual string Body { [NeverNull]get; [Seal]set; }
        public virtual string RevisionComment { [NeverNull]get; [Seal]set; }
        public virtual RevisionModerationStatus ModerationStatus { get; set; }
        public virtual Member Member { get; [Seal]set; }
        public virtual Entry Entry { get; [Seal]set; }
        public virtual DateTime RevisionDateUtc { get; [OneTime][Seal]set; }
        public virtual bool IsActive { get; set; }

        public virtual void Seal()
        {
            SealInternal();
        }

        public virtual int CompareTo(Revision other)
        {
            return -RevisionDateUtc.CompareTo(other.RevisionDateUtc);
        }

        public virtual object Clone()
        {
            return new Revision()
                       {
                           Body = Body,
                           Member = Member,
                           ModerationStatus = RevisionModerationStatus.Approved,
                           RevisionComment = string.Empty
                       };
        }
    }
}
using System;
using Bindable.Aspects.Parameters;
using PaulStovell.Domain.Framework;
using PaulStovell.Domain.Framework.Aspects;
using PaulStovell.Domain.Framework.Validation;

namespace PaulStovell.Domain.Model
{
    public class Comment : Entity, IComparable<Comment>
    {
        public virtual int Id { get; [OneTime][Seal]set; }
        public virtual Member Member { get; [Seal]set; }
        public virtual Entry Entry { get; [Seal]set; }
        public virtual string AuthorName { [NeverNull]get; [Seal]set; }
        public virtual string AuthorEmail { [NeverNull]get; [Seal]set; }
        public virtual string AuthorUrl { [NeverNull]get; [Seal]set; }
        public virtual string AuthorIP { [NeverNull]get; [Seal]set; }
        public virtual DateTime PostedDateUtc { get; [OneTime][Seal]set; }
        public virtual string CommentBody { [NeverNull]get; [Seal]set; }
        public virtual CommentModerationStatus ModerationStatus { get; set; }
        public virtual string History { [NeverNull]get; set; }

        public virtual ValidationResult Validate()
        {
            if (Member != null)
            {
                return new ValidationResult()
                    .Required("AuthorIP.Required", AuthorIP, "Author IP address is required.")
                    .Required("Comment.Required", CommentBody, "Please enter a comment.");
            }
            return new ValidationResult()
                .Required("AuthorIP.Required", AuthorIP, "Author IP address is required.")
                .Required("AuthorName.Required", AuthorName, "Please enter your name.")
                .Required("Comment.Required", CommentBody, "Please enter a comment.");
        }

        #region IComparable<Comment> Members

        public virtual int CompareTo(Comment other)
        {
            return PostedDateUtc.CompareTo(other.PostedDateUtc);
        }

        #endregion
    }
}
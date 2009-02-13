using System;
using Bindable.Aspects.Parameters;
using PaulStovell.Domain.Framework;
using PaulStovell.Domain.Framework.Aspects;
using PaulStovell.Domain.Framework.Validation;

namespace PaulStovell.Domain.Model
{
    public class Member : Entity
    {
        public virtual int Id { get; [OneTime]set; }
        public virtual string OpenId { [NeverNull]get; [Seal]set; }
        public virtual string EmailAddress { [NeverNull]get; set; }
        public virtual string FullName { [NeverNull]get; set; }
        public virtual DateTime LastLogin { get; set; }
        public virtual bool IsActive { get; set; }

        public virtual ValidationResult Validate()
        {
            var email = new Email("John<john@smith.com>", "Foo", "Hi John!");

            return new ValidationResult()
                .Required("OpenId", OpenId, "Please supply an OpenID.");
        }
    }

    public class Email
    {
        private readonly string _to;
        private readonly string _subject;
        private readonly string _message;

        public Email(string to, string subject, string message)
        {
            _to = to;
            _subject = subject;
            _message = message;
        }

        public virtual string To
        {
            get { return _to; }
        }

        public virtual string Subject
        {
            get { return _subject; }
        }

        public virtual string Message
        {
            get { return _message; }
        }
    }

}

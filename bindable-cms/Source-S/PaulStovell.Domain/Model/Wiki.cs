using System;
using Bindable.Aspects.Parameters;
using PaulStovell.Domain.Framework.Aspects;
using PaulStovell.Domain.Framework.Validation;

namespace PaulStovell.Domain.Model
{
    public class Wiki : ISecurable
    {
        public Wiki()
        {
            IsActive = true;
            SecurityKey = Guid.NewGuid();
        }
        
        public virtual int Id { get; [OneTime]set; }
        public virtual string Name { [NeverNull]get; set; }
        public virtual string Title { [NeverNull]get; set; }
        public virtual Guid SecurityKey { get; private set; }
        public virtual bool IsActive { get; set; }

        public virtual ValidationResult Validate()
        {
            return new ValidationResult()
                .Required("Name", Name, "Please enter a name.")
                .Required("Title", Name, "Please enter a title.");
        }
    }
}
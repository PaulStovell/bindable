using System;

namespace PaulStovell.Domain.Model
{
    public class Permission
    {
        public virtual int Id { get; set; }
        public virtual Operation Operation { get; set; }
        public virtual Group Group { get; set; }
        public virtual Guid InRelationTo { get; set; }
        public virtual bool IsAllow { get; set; }
    }
}
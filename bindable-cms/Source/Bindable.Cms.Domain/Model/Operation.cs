namespace Bindable.Cms.Domain.Model
{
    public class Operation
    {
        public virtual string Id { get; set; }
        public virtual string Name { get; set; }
        public virtual string Note { get; set; }
        public virtual Operation Parent { get; set; }
    }
}
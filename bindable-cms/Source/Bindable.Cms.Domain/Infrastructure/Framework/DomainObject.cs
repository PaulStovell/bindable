using Bindable.Cms.Domain.Framework;

namespace Bindable.Cms.Domain.Framework
{
    public abstract class Entity : ISealable
    {
        private bool _isSealed;

        protected virtual void SealInternal()
        {
            _isSealed = true;
        }

        bool ISealable.IsSealed
        {
            get { return _isSealed; }
        }
    }

    public interface IPersistanceErrors
    {
        
    }
}
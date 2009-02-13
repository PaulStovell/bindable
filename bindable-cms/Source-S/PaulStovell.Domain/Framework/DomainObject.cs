using PaulStovell.Domain.Framework;

namespace PaulStovell.Domain.Framework
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
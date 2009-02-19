using System.Reflection;

namespace Bindable.Core.Dynamics
{
    public interface IDynamicImplementation
    {
        object HandleFunction(MethodBase method, object[] arguments);
        void HandleAction(MethodBase method, object[] arguments);
    }
}
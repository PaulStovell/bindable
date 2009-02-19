using System;

namespace Bindable.Core.Dynamics
{
    public static class Adaptor
    {
        public static TInterface Adapt<TInterface>(object item) where TInterface : class
        {
            if (item == null) return null;
            var generator = new DelegatingTypeGenerator(typeof(TInterface));
            return (TInterface)generator.Instantiate(new AdaptorImplementation(item));
        }

        private class AdaptorImplementation : IDynamicImplementation
        {
            private readonly object _originalObject;

            public AdaptorImplementation(object originalObject)
            {
                _originalObject = originalObject;
            }

            public object HandleFunction(System.Reflection.MethodBase method, object[] arguments)
            {
                Console.WriteLine("Forwarding method {0}", method);
                return _originalObject.GetType().GetMethod(method.Name).Invoke(_originalObject, arguments);
            }

            public void HandleAction(System.Reflection.MethodBase method, object[] arguments)
            {
                Console.WriteLine("Forwarding method {0}", method);
                _originalObject.GetType().GetMethod(method.Name).Invoke(_originalObject, arguments);
            }
        }
    }
}

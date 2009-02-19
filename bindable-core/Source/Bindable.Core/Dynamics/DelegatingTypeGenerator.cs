using System;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;

namespace Bindable.Core.Dynamics
{
    public class DelegatingTypeGenerator
    {
        private readonly TypeGenerator _generator;
        private readonly FieldBuilder __interceptionService;
        
        public DelegatingTypeGenerator(params Type[] interfaces)
        {
            _generator = new TypeGenerator(typeof(object), "Interceptor", "InterceptedObject");
            foreach (var interfaceType in interfaces)
            {
                _generator.Implements(interfaceType);
            }
            _generator.AddConstructor(Constructor);
            _generator.AddField("__interceptionService", typeof(IDynamicImplementation), FieldAttributes.Private | FieldAttributes.InitOnly, ref __interceptionService);
            _generator.WhenMethodImplementationRequired(ImplementMethod);
            _generator.WhenPropertyImplementationRequired(ImplementProperty);
            _generator.WhenEventImplementationRequired(ImplementEvent);
        }

        public object Instantiate(IDynamicImplementation delegator)
        {
            var type = _generator.GenerateType();
            var instance = Activator.CreateInstance(type, delegator);
            return instance;
        }

        private void Constructor(TypeBuilder typeBuilder)
        {
            var constructor = typeBuilder.DefineConstructor(MethodAttributes.Public | MethodAttributes.RTSpecialName, CallingConventions.Standard, new[] { typeof(IDynamicImplementation) });
            var constructorILGenerator = constructor.GetILGenerator();
            constructorILGenerator.Emit(OpCodes.Ldarg_0);
            var originalConstructor = typeof(Object).GetConstructor(BindingFlags.Instance | BindingFlags.Public, null, new Type[0], null);
            constructorILGenerator.Emit(OpCodes.Call, originalConstructor);
            constructorILGenerator.Emit(OpCodes.Ldarg_0);
            constructorILGenerator.Emit(OpCodes.Ldarg_1);
            constructorILGenerator.Emit(OpCodes.Stfld, __interceptionService);
            constructorILGenerator.Emit(OpCodes.Ret);
        }

        private MethodBuilder ImplementMethod(TypeBuilder typeBuilder, MethodInfo methodToImplement, Type targetInterface)
        {
            var parameterTypes = methodToImplement.GetParameters().Select(p => p.ParameterType);
            var methodBuilder = typeBuilder.DefineMethod(methodToImplement.Name, MethodAttributes.Public | MethodAttributes.Virtual, methodToImplement.ReturnType, parameterTypes.ToArray());
            methodBuilder.CreateMethodBody(null, 0);
            var methodILGenerator = methodBuilder.GetILGenerator();
            if (methodToImplement.ReturnType != typeof(void))
            {
                methodILGenerator.DeclareLocal(methodToImplement.ReturnType);
            }
            var param = methodILGenerator.DeclareLocal(typeof(object[]));
            methodILGenerator.Emit(OpCodes.Ldarg_0);
            methodILGenerator.Emit(OpCodes.Ldfld, __interceptionService);
            methodILGenerator.Emit(OpCodes.Call, GetMethodBaseGetCurrentMethod());
            methodILGenerator.Emit(OpCodes.Ldc_I4, methodToImplement.GetParameters().Length);
            methodILGenerator.Emit(OpCodes.Newarr, typeof(object));
            if (methodToImplement.GetParameters().Length > 0)
            {
                methodILGenerator.Emit(OpCodes.Stloc_1);
                methodILGenerator.Emit(OpCodes.Ldloc_1);
                for (var parameterIndex = 0;
                     parameterIndex < methodToImplement.GetParameters().Length;
                     parameterIndex++)
                {
                    methodILGenerator.Emit(OpCodes.Ldc_I4, parameterIndex);
                    methodILGenerator.Emit(OpCodes.Ldarg, parameterIndex + 1);
                    methodILGenerator.Emit(OpCodes.Stelem_Ref);
                }
                methodILGenerator.Emit(OpCodes.Ldloc_1);
            }
            methodILGenerator.Emit(OpCodes.Callvirt, GetInterceptorMethod(methodToImplement));
            if (methodToImplement.ReturnType == typeof(void))
            {
                methodILGenerator.Emit(OpCodes.Nop);
            }
            else
            {
                methodILGenerator.Emit(OpCodes.Stloc_0);
                methodILGenerator.Emit(OpCodes.Ldloc_0);
            }
            methodILGenerator.Emit(OpCodes.Ret);
            typeBuilder.DefineMethodOverride(methodBuilder, methodToImplement);
            return methodBuilder;
        }

        private MethodInfo GetInterceptorMethod(MethodInfo interfaceMethod)
        {
            if (interfaceMethod.ReturnType == typeof(void))
            {
                return typeof (IDynamicImplementation).GetMethod("HandleAction");
            }
            return typeof(IDynamicImplementation).GetMethod("HandleFunction");
        }

        private PropertyBuilder ImplementProperty(TypeBuilder typeBuilder, PropertyInfo propertyToImplement, Type targetInterface)
        {
            var propertyBuilder = typeBuilder.DefineProperty(propertyToImplement.Name, PropertyAttributes.HasDefault, propertyToImplement.PropertyType, Type.EmptyTypes);
            if (propertyToImplement.CanRead)
            {
                var getterMethodInfo = propertyToImplement.GetGetMethod();
                var getterMethodBuilder = ImplementMethod(typeBuilder, getterMethodInfo, targetInterface);
                propertyBuilder.SetGetMethod(getterMethodBuilder);
            }
            if (propertyToImplement.CanWrite)
            {
                var setterMethodInfo = propertyToImplement.GetSetMethod();
                var setterMethodBuilder = ImplementMethod(typeBuilder, setterMethodInfo, targetInterface);
                propertyBuilder.SetSetMethod(setterMethodBuilder);
            }
            return propertyBuilder;
        }

        private EventBuilder ImplementEvent(TypeBuilder typeBuilder, EventInfo eventToImplement, Type targetInterface)
        {
            var eventBuilder = typeBuilder.DefineEvent(eventToImplement.Name, EventAttributes.None, eventToImplement.EventHandlerType);
            var addMethodInfo = eventToImplement.GetAddMethod();
            var removeMethodInfo = eventToImplement.GetRemoveMethod();
            var addMethodBuilder = ImplementMethod(typeBuilder, addMethodInfo, targetInterface);
            var removeMethodBuilder = ImplementMethod(typeBuilder, removeMethodInfo, targetInterface);
            eventBuilder.SetAddOnMethod(addMethodBuilder);
            eventBuilder.SetRemoveOnMethod(removeMethodBuilder);
            return eventBuilder;
        }

        private static MethodInfo GetMethodBaseGetCurrentMethod()
        {
            return typeof(MethodBase).GetMethod("GetCurrentMethod", BindingFlags.Public | BindingFlags.Static);
        }
    }
}

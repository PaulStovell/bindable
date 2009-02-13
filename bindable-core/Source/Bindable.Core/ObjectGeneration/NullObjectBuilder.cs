using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;

namespace Bindable.Core.ObjectGeneration
{
    /// <summary>
    /// Builds classes that implements interfaces to provide a "null object" with methods and 
    /// properties that do nothing.
    /// </summary>
    internal class NullObjectBuilder : TemplateObjectBuilder
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="NullObjectBuilder"/> class.
        /// </summary>
        /// <param name="interfaceToImplement">The interface to implement.</param>
        public NullObjectBuilder(Type interfaceToImplement)
        {
            this.ImplementInterface(interfaceToImplement);
            this.FinalizeType();
        }

        /// <summary>
        /// Creates the fields.
        /// </summary>
        protected override void ImplementFields()
        {

        }

        /// <summary>
        /// Generates a default constructor.
        /// </summary>
        protected override void ImplementConstructors()
        {
            ConstructorInfo originalConstructor = this.BaseType.GetConstructor(BindingFlags.Instance | BindingFlags.Public, null, new Type[0], null);
            
            ConstructorBuilder defaultConstructor = this.TypeBuilder.DefineConstructor(MethodAttributes.Public | MethodAttributes.RTSpecialName, CallingConventions.Standard, new Type[0]);
            ILGenerator defaultConstructorILGenerator = defaultConstructor.GetILGenerator();
            defaultConstructorILGenerator.Emit(OpCodes.Ldarg_0);
            defaultConstructorILGenerator.Emit(OpCodes.Call, originalConstructor);
            defaultConstructorILGenerator.Emit(OpCodes.Ret);
        }

        /// <summary>
        /// Implements a default implementation of the method (either with no body, or return
        /// default(returnType)).
        /// </summary>
        /// <param name="methodToImplement">The method to implement.</param>
        /// <param name="targetInterface"></param>
        protected override MethodBuilder ImplementMethod(MethodInfo methodToImplement, Type targetInterface)
        {
            // Get all types for parameters to the method
            IEnumerable<Type> parameterTypes = methodToImplement.GetParameters().Select(p => p.ParameterType);
            
            // Generate the method
            MethodBuilder methodBuilder = this.TypeBuilder.DefineMethod(methodToImplement.Name, MethodAttributes.Public | MethodAttributes.Virtual, methodToImplement.ReturnType, parameterTypes.ToArray());
            methodBuilder.CreateMethodBody(null, 0);
            ILGenerator methodILGenerator = methodBuilder.GetILGenerator();
            if (methodToImplement.ReturnType == typeof(void))
            {
                // No return types (void):
                methodILGenerator.Emit(OpCodes.Ret);
            }
            else if (methodToImplement.ReturnType.IsValueType)
            {
                // Value types:
                LocalBuilder resultLocalBuilder = methodILGenerator.DeclareLocal(methodToImplement.ReturnType);
                LocalBuilder defaultValueLocalBuilder = methodILGenerator.DeclareLocal(methodToImplement.ReturnType);
                methodILGenerator.Emit(OpCodes.Ldloca_S, defaultValueLocalBuilder);
                methodILGenerator.Emit(OpCodes.Initobj, methodToImplement.ReturnType);
                methodILGenerator.Emit(OpCodes.Ldloc_1);
                methodILGenerator.Emit(OpCodes.Stloc_0);
                methodILGenerator.Emit(OpCodes.Ldloc_0);
                methodILGenerator.Emit(OpCodes.Ret);
            }
            else if (methodToImplement.ReturnType.IsInterface)
            {
                // Reference type that can be null-object'ed
                LocalBuilder resultLocalBuilder = methodILGenerator.DeclareLocal(methodToImplement.ReturnType);
                methodILGenerator.Emit(OpCodes.Call, GetNullObjectForMethod(methodToImplement.ReturnType));
                methodILGenerator.Emit(OpCodes.Stloc_0);
                methodILGenerator.Emit(OpCodes.Ldloc_0);
                methodILGenerator.Emit(OpCodes.Ret);
            }
            else
            {
                // Reference types that can't be nulled:
                LocalBuilder resultLocalBuilder = methodILGenerator.DeclareLocal(methodToImplement.ReturnType);
                methodILGenerator.Emit(OpCodes.Ldnull);
                methodILGenerator.Emit(OpCodes.Stloc_0);
                methodILGenerator.Emit(OpCodes.Ldloc_0);
                methodILGenerator.Emit(OpCodes.Ret);
            }
            this.TypeBuilder.DefineMethodOverride(methodBuilder, methodToImplement);
            return methodBuilder;
        }

        /// <summary>
        /// Implements a property.
        /// </summary>
        /// <param name="propertyToImplement">The property to implement.</param>
        /// <param name="targetInterface">The target interface.</param>
        protected override PropertyBuilder ImplementProperty(PropertyInfo propertyToImplement, Type targetInterface)
        {
            PropertyBuilder propertyBuilder = this.TypeBuilder.DefineProperty(propertyToImplement.Name, PropertyAttributes.HasDefault, propertyToImplement.PropertyType, Type.EmptyTypes);
            if (propertyToImplement.CanRead) 
            {
                MethodInfo getterMethodInfo = propertyToImplement.GetGetMethod();
                MethodBuilder getterMethodBuilder = this.ImplementMethod(getterMethodInfo, targetInterface);
                propertyBuilder.SetGetMethod(getterMethodBuilder);
            }
            if (propertyToImplement.CanWrite)
            {
                MethodInfo setterMethodInfo = propertyToImplement.GetGetMethod();
                MethodBuilder setterMethodBuilder = this.ImplementMethod(setterMethodInfo, targetInterface);
                propertyBuilder.SetSetMethod(setterMethodBuilder);
            }
            return propertyBuilder;
        }

        /// <summary>
        /// Implements the event.
        /// </summary>
        /// <param name="eventToImplement">The event to implement.</param>
        /// <param name="targetInterface">The target interface.</param>
        /// <returns></returns>
        protected override EventBuilder ImplementEvent(EventInfo eventToImplement, Type targetInterface)
        {
            EventBuilder eventBuilder = this.TypeBuilder.DefineEvent(eventToImplement.Name, EventAttributes.None, eventToImplement.EventHandlerType);
            MethodInfo addMethodInfo = eventToImplement.GetAddMethod();
            MethodInfo removeMethodInfo = eventToImplement.GetRemoveMethod();
            MethodBuilder addMethodBuilder = this.ImplementMethod(addMethodInfo, targetInterface);
            MethodBuilder removeMethodBuilder = this.ImplementMethod(removeMethodInfo, targetInterface);
            eventBuilder.SetAddOnMethod(addMethodBuilder);
            eventBuilder.SetRemoveOnMethod(removeMethodBuilder);
            return eventBuilder;
        }

        private static MethodInfo GetNullObjectForMethod(Type genericType)
        {
            MethodInfo nullObjectForMethod = typeof(NullObject).GetMethods(BindingFlags.Public | BindingFlags.Static)
                .Where(m => m.Name == "For" && m.IsGenericMethod)
                .First();
            nullObjectForMethod = nullObjectForMethod.MakeGenericMethod(genericType);
            return nullObjectForMethod;
        }
    }
}
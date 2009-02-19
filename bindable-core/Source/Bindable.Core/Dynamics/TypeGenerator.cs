using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using System.Threading;

namespace Bindable.Core.Dynamics
{
    /// <summary>
    /// A helper for generating objects at runtime using Reflection.Emit.
    /// </summary>
    public sealed class TypeGenerator
    {
        private readonly List<Action<TypeBuilder>> _constructors = new List<Action<TypeBuilder>>();
        private readonly List<Type> _interfaces = new List<Type>();
        private readonly TypeBuilder _typeBuilder;
        private readonly AssemblyBuilder _assemblyBuilder;
        private readonly Type _inherits = typeof(object);
        private Func<TypeBuilder, MethodInfo, Type, MethodBuilder> _methodImplementor;
        private Func<TypeBuilder, PropertyInfo, Type, PropertyBuilder> _propertyImplementor;
        private Func<TypeBuilder, EventInfo, Type, EventBuilder> _eventImplementor;
        private Type _generatedType;
        
        /// <summary>
        /// Initializes a new instance of the <see cref="TypeGenerator"/> class.
        /// </summary>
        /// <param name="baseType">The base type that this type will inherit.</param>
        /// <param name="assemblyPrefix">The assembly prefix.</param>
        /// <param name="typePrefix">The type prefix.</param>
        public TypeGenerator(Type baseType, string assemblyPrefix, string typePrefix)
        {
            _inherits = baseType;
            var assemblyName = new AssemblyName(assemblyPrefix + ".Generated-" + GetHashCode());
            var assemblyBuilder = Thread.GetDomain().DefineDynamicAssembly(assemblyName, AssemblyBuilderAccess.RunAndSave);
            var moduleBuilder = assemblyBuilder.DefineDynamicModule(assemblyName.Name, "Me2.dll");
            var typeBuilder = moduleBuilder.DefineType(assemblyPrefix + ".Generated." + typePrefix + GetHashCode(), TypeAttributes.Public, _inherits);
            _typeBuilder = typeBuilder;
            _assemblyBuilder = assemblyBuilder;
        }

        /// <summary>
        /// An interface which the generated type will implement.
        /// </summary>
        /// <typeparam name="T">The type to implement.</typeparam>
        public void Implements<T>()
        {
            Implements(typeof(T));
        }

        /// <summary>
        /// An interface which the generated type will implement.
        /// </summary>
        /// <param name="type">The type.</param>
        public void Implements(Type type)
        {
            if (!type.IsInterface)
            {
                throw new ArgumentException(string.Format("Type '{0}' is not an interface.", type.Name));
            }
            
            _interfaces.Add(type);
            foreach (var baseType in type.GetInterfaces())
            {
                _interfaces.Add(baseType);
            }
        }

        /// <summary>
        /// Adds a constructor to the generated type.
        /// </summary>
        /// <param name="constructorBuilder">The constructor builder.</param>
        public void AddConstructor(Action<TypeBuilder> constructorBuilder)
        {
            _constructors.Add(constructorBuilder);
        }

        /// <summary>
        /// Adds a field to the generated type.
        /// </summary>
        /// <param name="fieldName">Name of the field.</param>
        /// <param name="fieldType">Type of the field.</param>
        /// <param name="attributes">The attributes.</param>
        /// <param name="field">The field.</param>
        public void AddField(string fieldName, Type fieldType, FieldAttributes attributes, ref FieldBuilder field)
        {
            field = _typeBuilder.DefineField(fieldName, fieldType, attributes);
        }

        /// <summary>
        /// Specifies a callback to use when the type generator needs to implement a method of an interface.
        /// </summary>
        /// <param name="method">The method.</param>
        public void WhenMethodImplementationRequired(Func<TypeBuilder, MethodInfo, Type, MethodBuilder> method)
        {
            _methodImplementor = method;
        }

        /// <summary>
        /// Specifies a callback to use when the type generator needs to implement a property of an interface.
        /// </summary>
        /// <param name="property">The property.</param>
        public void WhenPropertyImplementationRequired(Func<TypeBuilder, PropertyInfo, Type, PropertyBuilder> property)
        {
            _propertyImplementor = property;
        }
        /// <summary>
        /// Specifies a callback to use when the type generator needs to implement an event of an interface.
        /// </summary>
        /// <param name="eventInfo">The event.</param>
        public void WhenEventImplementationRequired(Func<TypeBuilder, EventInfo, Type, EventBuilder> eventInfo)
        {
            _eventImplementor = eventInfo;
        }

        /// <summary>
        /// Generates the type.
        /// </summary>
        /// <returns></returns>
        public Type GenerateType()
        {
            if (_generatedType == null)
            {
                // Create all constructors
                foreach (var ctor in _constructors)
                {
                    ctor(_typeBuilder);
                }

                // Implement all interfaces
                foreach (var targetInterface in _interfaces)
                {
                    _typeBuilder.AddInterfaceImplementation(targetInterface);
                    foreach (var method in targetInterface.GetMethods(BindingFlags.Public | BindingFlags.Instance).Where(
                                m => !m.Name.StartsWith("get_") 
                                  && !m.Name.StartsWith("set_") 
                                  && !m.Name.StartsWith("add_") 
                                  && !m.Name.StartsWith("remove_")))
                    {
                        _methodImplementor(_typeBuilder, method, targetInterface);
                    }
                    foreach (var property in targetInterface.GetProperties(BindingFlags.Public | BindingFlags.Instance))
                    {
                        _propertyImplementor(_typeBuilder, property, targetInterface);
                    }
                    foreach (var eventInfo in targetInterface.GetEvents(BindingFlags.Public | BindingFlags.Instance))
                    {
                        _eventImplementor(_typeBuilder, eventInfo, targetInterface);
                    }
                }

                var generatedType = _typeBuilder.CreateType();
                _assemblyBuilder.Save("Me.dll");
                _generatedType = generatedType;
            }

            return _generatedType;
        }
    }
}
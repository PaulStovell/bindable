using System;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using System.Threading;

namespace Bindable.Core.ObjectGeneration
{
    /// <summary>
    /// Controls the automatic code generation of a mock object.
    /// </summary>
    public abstract class TemplateObjectBuilder
    {
        private readonly TypeBuilder _typeBuilder;
        private readonly AssemblyBuilder _assemblyBuilder;
        private readonly Type _baseType;
        private Type _generatedType;

        /// <summary>
        /// Initializes a new instance of the <see cref="TemplateObjectBuilder"/> class.
        /// </summary>
        /// <param name="baseType">Type of the base.</param>
        protected TemplateObjectBuilder(Type baseType)
        {
            _baseType = baseType;

            // Setup the assembly and type builder
            var assemblyName = new AssemblyName("NullObject.Generated-" + GetHashCode());
            _assemblyBuilder = Thread.GetDomain().DefineDynamicAssembly(assemblyName, AssemblyBuilderAccess.RunAndSave);
            var moduleBuilder = _assemblyBuilder.DefineDynamicModule(assemblyName.Name);
            _typeBuilder = moduleBuilder.DefineType("NullObject.Generated.Null" + GetHashCode(), TypeAttributes.Public, baseType);

            // Generate the body of the type
            ImplementFields();
            ImplementConstructors();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="TemplateObjectBuilder"/> class.
        /// </summary>
        protected TemplateObjectBuilder() : this(typeof(Object))
        {
        }

        /// <summary>
        /// Gets the type builder.
        /// </summary>
        /// <value>The type builder.</value>
        protected TypeBuilder TypeBuilder
        {
            get { return _typeBuilder; }
        }

        /// <summary>
        /// Gets the type of the base.
        /// </summary>
        /// <value>The type of the base.</value>
        protected Type BaseType
        {
            get { return _baseType; }
        }

        /// <summary>
        /// Creates the fields.
        /// </summary>
        protected abstract void ImplementFields();

        /// <summary>
        /// When overridden in a derived class, provides a chance to implement any constructors.
        /// </summary>
        protected abstract void ImplementConstructors();

        /// <summary>
        /// When overridden in a derived class, implements an event.
        /// </summary>
        /// <param name="eventInfo">The event info.</param>
        /// <param name="targetInterface">The target interface.</param>
        protected abstract EventBuilder ImplementEvent(EventInfo eventInfo, Type targetInterface);

        /// <summary>
        /// When overridden in a derived class, implements a method from a given interface.
        /// </summary>
        /// <param name="methodToImplement">The method to implement.</param>
        /// <param name="targetInterface">When implementing a method, specifies the interface on which the method is declared.</param>
        protected abstract MethodBuilder ImplementMethod(MethodInfo methodToImplement, Type targetInterface);

        /// <summary>
        /// When overridden in a derived class, implements a property.
        /// </summary>
        /// <param name="property">The property.</param>
        /// <param name="targetInterface">The target interface.</param>
        protected abstract PropertyBuilder ImplementProperty(PropertyInfo property, Type targetInterface);

        /// <summary>
        /// Implements the interface.
        /// </summary>
        /// <param name="targetInterface">The target interface.</param>
        protected void ImplementInterface(Type targetInterface)
        {
            if (!targetInterface.IsInterface)
            {
                throw new ArgumentException("Type '{0}' is not an interface.".FormatWith(targetInterface.Name));
            }

            TypeBuilder.AddInterfaceImplementation(targetInterface);
            foreach (var method in targetInterface.GetMethods(BindingFlags.Public | BindingFlags.Instance).Where(m => !m.Name.StartsWith("get_") && !m.Name.StartsWith("set_") && !m.Name.StartsWith("add_") && !m.Name.StartsWith("remove_")))
            {
                ImplementMethod(method, targetInterface);
            }
            foreach (var property in targetInterface.GetProperties(BindingFlags.Public | BindingFlags.Instance))
            {
                ImplementProperty(property, targetInterface);
            }
            foreach (var eventInfo in targetInterface.GetEvents(BindingFlags.Public | BindingFlags.Instance))
            {
                ImplementEvent(eventInfo, targetInterface);
            }

            foreach (var inheritedInterfaces in targetInterface.GetInterfaces())
            {
                ImplementInterface(inheritedInterfaces);
            }
        }

        /// <summary>
        /// Finalizes this instance.
        /// </summary>
        /// <returns></returns>
        public Type FinalizeType()
        {
            if (_generatedType == null)
            {
                // Store the generated type for use again
                _generatedType = this.TypeBuilder.CreateType();
                _assemblyBuilder.Save("Me.dll");
            }
            return _generatedType;
        }

        /// <summary>
        /// Instantiates an instance of the generated type.
        /// </summary>
        /// <param name="constructorArguments">The constructor arguments.</param>
        /// <returns></returns>
        public object Instantiate(params object[] constructorArguments)
        {
            Type type = this.FinalizeType();
            object result = Activator.CreateInstance(type, constructorArguments);
            return result;
        }

        /// <summary>
        /// Instantiates an instance of the generated type.
        /// </summary>
        /// <typeparam name="TInterface">The type of the interface.</typeparam>
        /// <param name="constructorArguments">The constructor arguments.</param>
        /// <returns></returns>
        public TInterface Instantiate<TInterface>(params object[] constructorArguments)
        {
            return (TInterface)Instantiate(constructorArguments);
        }
    }
}
using Castle.Core;
using Castle.MicroKernel;

namespace Bindable.Cms.Domain.Infrastructure.ApplicationModel.CastleExtensions
{
    /// <summary>
    /// Adds the ability to resolve an array of dependencies as an argument when activating a component.
    /// </summary>
    public class ArrayResolver : ISubDependencyResolver
    {
        private readonly IKernel _kernel;

        /// <summary>
        /// Initializes a new instance of the <see cref="ArrayResolver"/> class.
        /// </summary>
        /// <param name="kernel">The kernel.</param>
        public ArrayResolver(IKernel kernel)
        {
            _kernel = kernel;
        }

        /// <summary>
        /// Should return an instance of a service or property values as
        /// specified by the dependency model instance.
        /// It is also the responsability of <see cref="T:Castle.MicroKernel.IDependencyResolver"/>
        /// to throw an exception in the case a non-optional dependency
        /// could not be resolved.
        /// </summary>
        /// <param name="context">Creation context, which is a resolver itself</param>
        /// <param name="parentResolver">Parent resolver</param>
        /// <param name="model">Model of the component that is requesting the dependency</param>
        /// <param name="dependency">The dependency model</param>
        /// <returns>The dependency resolved value or null</returns>
        public object Resolve(CreationContext context, ISubDependencyResolver parentResolver, ComponentModel model, DependencyModel dependency)
        {
            return _kernel.ResolveAll(dependency.TargetType.GetElementType(), null);
        }

        /// <summary>
        /// Returns true if the resolver is able to satisfy this dependency.
        /// </summary>
        /// <param name="context">Creation context, which is a resolver itself</param>
        /// <param name="parentResolver">Parent resolver</param>
        /// <param name="model">Model of the component that is requesting the dependency</param>
        /// <param name="dependency">The dependency model</param>
        /// <returns>
        /// 	<c>true</c> if the dependency can be satisfied
        /// </returns>
        public bool CanResolve(CreationContext context, ISubDependencyResolver parentResolver, ComponentModel model, DependencyModel dependency)
        {
            return dependency.TargetType != null &&
                   dependency.TargetType.IsArray &&
                   dependency.TargetType.GetElementType().IsInterface;
        }
    }
}
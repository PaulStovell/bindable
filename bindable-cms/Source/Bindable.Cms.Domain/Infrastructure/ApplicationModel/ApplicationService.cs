using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Bindable.Cms.Domain.Infrastructure.ApplicationModel.CastleExtensions;
using Castle.Core;
using Castle.MicroKernel.Registration;
using Castle.Windsor;
using Microsoft.Practices.ServiceLocation;

namespace Bindable.Cms.Domain.ApplicationModel
{
    /// <summary>
    /// An implementation of the ApplicationService based on Castle Windsor.
    /// </summary>
    public class ApplicationService : ServiceLocatorImplBase, IApplicationService
    {
        private static IApplicationService _current = new ApplicationService();
        private readonly IWindsorContainer _container;

        /// <summary>
        /// Initializes a new instance of the <see cref="ApplicationService"/> class.
        /// </summary>
        public ApplicationService()
        {
            _container = new WindsorContainer();
            _container.Kernel.Resolver.AddSubResolver(new ArrayResolver(_container.Kernel));
            _container.Kernel.AddFacility<DelegateSupportFacility>();
        }

        /// <summary>
        /// Gets the current application service.
        /// </summary>
        public static IApplicationService Current
        {
            get { return _current; }
            set
            {
                _current = value;
                ServiceLocator.SetLocatorProvider(() => value);
            }
        }

        private static LifestyleType ConvertLifetime(Lifetime lifetime)
        {
            switch (lifetime)
            {
                case Lifetime.Singleton: return LifestyleType.Singleton;
                case Lifetime.PerWebRequest: return LifestyleType.PerWebRequest;
                case Lifetime.Transient: return LifestyleType.Transient;
                default: throw new ArgumentOutOfRangeException("lifetime");
            }
        }

        protected override IEnumerable<object> DoGetAllInstances(Type serviceType)
        {
            return (object[])_container.ResolveAll(serviceType);
        }

        protected override object DoGetInstance(Type serviceType, string key)
        {
            return key != null ? _container.Resolve(key, serviceType) : _container.Resolve(serviceType);
        }

        public IApplicationService Register<TService, TImplementation>(string key) where TImplementation : class, TService
        {
            _container.AddComponent<TService, TImplementation>(key);
            return this;
        }

        IApplicationService IApplicationService.Register<I, T>()
        {
            return Register();
        }

        IApplicationService IApplicationService.Register<I, T>(string key)
        {
            return Register(key);
        }

        public IApplicationService Register<TService, TImplementation>() where TImplementation : class, TService
        {
            _container.AddComponent<TService, TImplementation>();
            return this;
        }

        public IApplicationService Register<TImplementation>(string key)
        {
            _container.AddComponent<TImplementation>(key);
            return this;
        }

        public IApplicationService Register<TImplementation>()
        {
            _container.AddComponent<TImplementation>();
            return this;
        }

        public IApplicationService Register<TService>(TService instance)
        {
            _container.Register(Component.For<TService>().Instance(instance));
            return this;
        }

        public IApplicationService Register(string key, Type serviceType, Type classType)
        {
            _container.AddComponent(key, serviceType, classType);
            return this;
        }

        public IApplicationService Register(string key, Type classType)
        {
            _container.AddComponent(key, classType);
            return this;
        }

        public IApplicationService Register<TService, TImplementation>(string key, Lifetime lifestyle) where TImplementation : class, TService
        {
            _container.AddComponentLifeStyle<TService, TImplementation>(key, ConvertLifetime(lifestyle));
            return this;
        }

        public IApplicationService Register<TService, TImplementation>(Lifetime lifestyle) where TImplementation : class, TService
        {
            _container.AddComponentLifeStyle<TService, TImplementation>(ConvertLifetime(lifestyle));
            return this;
        }

        public IApplicationService Register<TImplementation>(string key, Lifetime lifestyle)
        {
            _container.AddComponentLifeStyle<TImplementation>(key, ConvertLifetime(lifestyle));
            return this;
        }

        public IApplicationService Register<TImplementation>(Lifetime lifestyle)
        {
            _container.AddComponentLifeStyle<TImplementation>(ConvertLifetime(lifestyle));
            return this;
        }

        public IApplicationService Register(string key, Type serviceType, Type classType, Lifetime lifestyle)
        {
            _container.AddComponentLifeStyle(key, serviceType, classType, ConvertLifetime(lifestyle));
            return this;
        }

        public IApplicationService Register(string key, Type classType, Lifetime lifestyle)
        {
            _container.AddComponentLifeStyle(key, classType, ConvertLifetime(lifestyle));
            return this;
        }

        public IApplicationService RegisterAll<TService>(Assembly assembly, Lifetime lifetime)
        {
            var types = assembly.GetTypes().Where(type => typeof (TService).IsAssignableFrom(type) && !type.IsAbstract && !type.IsInterface);
            foreach (var type in types)
            {
                var serviceTypes = type.GetInterfaces().Union(new[] { typeof(TService) }).Distinct().Except(new[] { typeof(IDisposable) });
                _container.Register(Component.For(serviceTypes.ToArray()).ImplementedBy(type).LifeStyle.Is(ConvertLifetime(lifetime)));
            }
            return this;
        }

        public IApplicationService RegisterAll<TService>(Assembly assembly, Func<Type, string> namer, Lifetime lifetime)
        {
            _container.Register(
                AllTypes
                    .FromAssembly(assembly)
                    .BasedOn<TService>()
                    .Configure(
                        comp => comp.Named(namer(comp.Implementation)).LifeStyle.Is(ConvertLifetime(lifetime))));
            return this;
        }

        public IApplicationService Register<TService>(Func<TService> callback)
        {
            var wrapper = new Func<object>(delegate { return callback(); });
            _container.Register(
                Component.For<TService>().ExtendedProperties(
                    Property.ForKey("callback").Eq(wrapper)));
            return this;
        }

        public IApplicationService Register<TService>(Func<TService> callback, Lifetime lifetime)
        {
            var wrapper = new Func<object>(delegate { return callback(); });
            _container.Register(
                Component.For<TService>().ExtendedProperties(
                    Property.ForKey("callback").Eq(wrapper)).LifeStyle.Is(ConvertLifetime(lifetime)));
            return this;
        }

        public T RegisterAndResolve<T>(Lifetime lifetime)
        {
            if (!_container.Kernel.HasComponent(typeof(T)))
            {
                _container.AddComponentLifeStyle<T>(ConvertLifetime(lifetime));
            }
            return _container.Resolve<T>();
        }

        public IApplicationService Register(params IRegistration[] registrations)
        {
            _container.Register(registrations);
            return this;
        }

        public TService Resolve<TService>()
        {
            return _container.Resolve<TService>();
        }

        public TService Resolve<TService>(string name)
        {
            return _container.Resolve<TService>(name);
        }

        public object Resolve(Type serviceType)
        {
            return _container.Resolve(serviceType);
        }

        public object Resolve(string name)
        {
            return _container.Resolve(name);
        }

        public void Release(object o)
        {
            _container.Release(o);
        }
    }
}
using System;
using Castle.Core;
using Microsoft.Practices.ServiceLocation;
using System.Reflection;

namespace Bindable.Cms.Domain.ApplicationModel
{
    public interface IApplicationService : IServiceLocator
    {
        IApplicationService Register<I>(I instance);
        IApplicationService Register<I, T>(string key) where T : class, I;
        IApplicationService Register<I, T>() where T : class, I;
        IApplicationService Register<T>(string key);
        IApplicationService Register<T>();
        IApplicationService Register(string key, Type serviceType, Type classType);
        IApplicationService Register(string key, Type classType);
        IApplicationService Register<I, T>(string key, Lifetime lifetime) where T : class, I;
        IApplicationService Register<I, T>(Lifetime lifetime) where T : class, I;
        IApplicationService Register<T>(string key, Lifetime lifetime);
        IApplicationService Register<T>(Lifetime lifetime);
        IApplicationService Register(string key, Type serviceType, Type classType, Lifetime lifetime);
        IApplicationService Register(string key, Type classType, Lifetime lifetime);
        IApplicationService RegisterAll<TService>(Assembly assembly, Lifetime lifetime);
        IApplicationService RegisterAll<TService>(Assembly assembly, Func<Type, string> namer, Lifetime lifetime);
        IApplicationService Register<TService>(Func<TService> callback);
        IApplicationService Register<TService>(Func<TService> callback, Lifetime lifetime);
        T RegisterAndResolve<T>(Lifetime lifetime);
        TService Resolve<TService>();
        TService Resolve<TService>(string name);
        object Resolve(Type serviceType);
        object Resolve(string name);
        void Release(object o);
    }
}
using System;
using System.Diagnostics;
using System.ServiceModel;

namespace Bindable.Core.Communication
{
    /// <summary>
    /// Helper class for Service Hosts.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class WcfServiceHost<T> : ServiceHost
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="WcfServiceHost&lt;T&gt;"/> class.
        /// </summary>
        public WcfServiceHost()
            : base(typeof(T)) 
        { 
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="WcfServiceHost&lt;T&gt;"/> class.
        /// </summary>
        /// <param name="baseAddresses">The base addresses.</param>
        public WcfServiceHost(params string[] baseAddresses)
            : base(typeof(T), Convert(baseAddresses)) 
        { 
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="WcfServiceHost&lt;T&gt;"/> class.
        /// </summary>
        /// <param name="baseAddresses">The base addresses.</param>
        public WcfServiceHost(params Uri[] baseAddresses)
            : base(typeof(T), baseAddresses) 
        { 
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="WcfServiceHost&lt;T&gt;"/> class.
        /// </summary>
        /// <param name="singleton">The singleton.</param>
        /// <param name="baseAddresses">The base addresses.</param>
        public WcfServiceHost(T singleton, params string[] baseAddresses)
            : base(singleton, Convert(baseAddresses))
        { 
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="WcfServiceHost&lt;T&gt;"/> class.
        /// </summary>
        /// <param name="singleton">The singleton.</param>
        public WcfServiceHost(T singleton)
            : base(singleton)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="WcfServiceHost&lt;T&gt;"/> class.
        /// </summary>
        /// <param name="singleton">The singleton.</param>
        /// <param name="baseAddresses">The base addresses.</param>
        public WcfServiceHost(T singleton, params Uri[] baseAddresses)
            : base(singleton, baseAddresses) { }
        
        static Uri[] Convert(string[] baseAddresses)
        {
            Converter<string, Uri> convert = address => new Uri(address);
            return Array.ConvertAll(baseAddresses, convert);
        }

        /// <summary>
        /// Gets the singleton.
        /// </summary>
        /// <value>The singleton.</value>
        public virtual T Singleton
        {
            get
            {
                if (SingletonInstance == null)
                {
                    return default(T);
                }
                Debug.Assert(SingletonInstance is T);
                return (T)SingletonInstance;
            }
        }
    }
}
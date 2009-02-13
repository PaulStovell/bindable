using System;
using System.ServiceProcess;
using Bindable.Core.Helpers;

namespace Bindable.Core.Communication
{
    /// <summary>
    /// This class acts as a host for hosting WCF services either as Windows Services or as console services.
    /// </summary>
    /// <typeparam name="TService">The type of the service.</typeparam>
    public class WindowsServiceHostForWcfService<TService> : ServiceBase where TService : new()
    {
        private readonly WcfServiceHost<TService> _serviceHost;

        /// <summary>
        /// Construct a WCF Service Base class
        /// </summary>
        /// <param name="serviceName">The name of the Windows Service</param>
        /// <param name="baseAddresses">The base addresses.</param>
        public WindowsServiceHostForWcfService(string serviceName, params string[] baseAddresses)
        {
            Guard.NotNull(serviceName, "serviceName");
            ServiceName = serviceName;
            _serviceHost = new WcfServiceHost<TService>(baseAddresses);
        }

        /// <summary>
        /// When implemented in a derived class, executes when a Start command is sent to the service by the Service Control Manager (SCM) or when the operating system starts (for a service that starts automatically). Specifies actions to take when the service starts.
        /// </summary>
        /// <param name="args">Data passed by the start command.</param>
        protected override void OnStart(string[] args)
        {
            StartWcfService();
            base.OnStart(args);
        }

        /// <summary>
        /// When implemented in a derived class, executes when a Stop command is sent to the service by the Service Control Manager (SCM). Specifies actions to take when a service stops running.
        /// </summary>
        protected override void OnStop()
        {
            StopWcfService();
            base.OnStop();
        }

        /// <summary>
        /// Starts the WCF service.
        /// </summary>
        protected virtual void StartWcfService()
        {
            try
            {
                TraceHelper.TraceVerbose("Starting WCF service {0}", ServiceName);
                _serviceHost.Open();
                TraceHelper.TraceInformation("WCF service {0} started successfully", ServiceName);
            }
            catch (Exception ex)
            {
                TraceHelper.TraceError(ex, "Failed to start WCF service {0}", ServiceName);
                Stop();
            }
        }

        /// <summary>
        /// Stops the WCF service.
        /// </summary>
        protected virtual void StopWcfService()
        {
            try
            {
                TraceHelper.TraceVerbose("Stopping WCF service {0}", ServiceName);
                _serviceHost.Close();
                TraceHelper.TraceInformation("WCF service {0} stopped successfully", ServiceName);
            }
            catch (Exception ex)
            {
                TraceHelper.TraceError(ex, "Failed to stop WCF service {0}", ServiceName);
            }
        }

        /// <summary>
        /// Starts the service as a Windows Service.
        /// </summary>
        public void StartWindowsService()
        {
            var servicesToRun = new ServiceBase[] { this };
            Run(servicesToRun);
        }

        /// <summary>
        /// Starts the service as a console application.
        /// </summary>
        public void StartConsoleService()
        {
            StartWcfService();
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("WCF service {0} started. Press any key to stop...", this.ServiceName);
            Console.ReadKey();
            Console.ForegroundColor = ConsoleColor.Gray;
            StopWcfService();
        }

        public void InstallService(string[] installArguments)
        {
        }

        public void UninstallService(string[] uninstallArguments)
        {      
        }
    }
}
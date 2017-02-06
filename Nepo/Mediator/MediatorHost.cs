using System;
using System.ServiceModel;

namespace Mediator
{
    /// <summary>
    /// Hosts the Mediator Service.
    /// </summary>
    public class MediatorHost : IDisposable
    {
        /// <summary>
        /// Service host. 
        /// </summary>
        private readonly ServiceHost _host;

        /// <summary>
        /// Initializes a new instance of the <see cref="MediatorHost"/>.
        /// </summary>
        public MediatorHost()
        {
            this._host = new ServiceHost(typeof(MediatorService));
        }

        /// <summary>
        /// Starts the Service.
        /// </summary>
        public void Start()
        {
            this._host.Open();
        }

        /// <summary>
        /// Stops the service.
        /// </summary>
        public void Stop()
        {
            this._host.Close();
        }

        /// <summary>
        /// Disposes of the service.
        /// </summary>
        public void Dispose()
        {
            (_host as IDisposable)?.Dispose();
        }
    }
}
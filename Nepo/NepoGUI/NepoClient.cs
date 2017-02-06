using System;
using System.Collections.Generic;
using System.ServiceModel;
using Nepo.Common;
using NepoGUI.MediatorServiceRef;

namespace NepoGUI
{
    /// <summary>
    /// Implements logic for communication with Mediator
    /// </summary>
    public class NepoClient : IDisposable
    {
        private readonly MediatorClient mediatorClient = new MediatorClient(new InstanceContext(new MediatorCallback()));

        private readonly MediatorCallback callback;

        private readonly Guid privateGuid = new Guid();

        public event EventHandler<NewDataEventArgs> NewDataAvailable;

        public NepoClient()
        {
            this.callback = new MediatorCallback();
            this.mediatorClient = new MediatorClient(new InstanceContext(this.callback));
            this.callback.DataIsReady += Callback_DataIsReady;
        }

        private void Callback_DataIsReady(object sender, EventArgs e)
        {
            var newData = this.mediatorClient.GetProposedSolutions(this.privateGuid);
            this.NewDataAvailable?.Invoke(this, new NewDataEventArgs(newData));
        }

        public void Register()
        {
            this.mediatorClient.Register(this.privateGuid);
        }

        public void Dispose()
        {
            ((IDisposable) mediatorClient)?.Dispose();
            this.callback.DataIsReady -= Callback_DataIsReady;
        }
    }

    public class NewDataEventArgs
    {
        public List<Solution> ProposedSolutions { get; }

        public NewDataEventArgs(List<Solution> proposedSolutions)
        {
            this.ProposedSolutions = proposedSolutions;
        }

    }
}
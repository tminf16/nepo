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
        private readonly MediatorClient mediatorClient;

        private readonly MediatorCallback callback;

        private readonly Guid privateGuid = Guid.NewGuid();

        public event EventHandler<NewDataEventArgs> NewDataAvailable;

        public event EventHandler HabemusPapam;

        public NepoClient()
        {
            this.callback = new MediatorCallback();
            this.mediatorClient = new MediatorClient(new InstanceContext(this.callback));
            this.callback.DataIsReady += Callback_DataIsReady;
            
        }

        private void Callback_DataIsReady(object sender, DataReadyEventArgs e)
        {
            if (e.PopeState == CanIHasPope.BlackSmoke)
            {
                var newData = this.mediatorClient.GetProposedSolutions(this.privateGuid);
                this.NewDataAvailable?.Invoke(this, new NewDataEventArgs(newData));
            }
            else
            {
                this.HabemusPapam?.Invoke(this, new EventArgs());
            }
        }

		
        public void Vote(List<Tuple<int, bool>> votes)
        {
            this.mediatorClient.Vote(votes, this.privateGuid);
        }


        public Instance Register()
        {
            return this.mediatorClient.Register(this.privateGuid);
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
using System;
using System.Threading.Tasks;
using Mediator;
using IMediatorCallback = NepoGUI.MediatorServiceRef.IMediatorCallback;

namespace NepoGUI
{
    public class MediatorCallback : IMediatorCallback
    {
        public event EventHandler<DataReadyEventArgs> DataIsReady;

        public void DataReady(CanIHasPope popeState)
        {
            Task.Run(() =>
            {
                this.DataIsReady?.Invoke(this, new DataReadyEventArgs(popeState));
            });
        }
    }

    public class DataReadyEventArgs
    {
        public CanIHasPope PopeState { get; set;  }

        public DataReadyEventArgs(CanIHasPope popeState)
        {
            this.PopeState = popeState;
        }
    }
}
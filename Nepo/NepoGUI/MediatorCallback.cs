using System;
using NepoGUI.MediatorServiceRef;
using System.Threading.Tasks;

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
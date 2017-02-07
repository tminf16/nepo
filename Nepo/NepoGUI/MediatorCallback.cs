using System;
using NepoGUI.MediatorServiceRef;

namespace NepoGUI
{
    public class MediatorCallback : IMediatorCallback
    {
        public event EventHandler<DataReadyEventArgs> DataIsReady;

        public void DataReady(CanIHasPope popeState)
        {
            this.DataIsReady?.Invoke(this, new DataReadyEventArgs(popeState));   
        }
    }

    public class DataReadyEventArgs
    {
        public CanIHasPope popeState;

        public DataReadyEventArgs(CanIHasPope popeState)
        {
            
        }
    }
}
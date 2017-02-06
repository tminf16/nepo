using System;
using NepoGUI.MediatorServiceRef;

namespace NepoGUI
{
    public class MediatorCallback : IMediatorCallback
    {
        public event EventHandler DataIsReady;

        public void DataReady(CanIHasPope popeState)
        {
           
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
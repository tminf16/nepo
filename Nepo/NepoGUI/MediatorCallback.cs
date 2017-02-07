using System;
using NepoGUI.MediatorServiceRef;

namespace NepoGUI
{
    public class MediatorCallback : IMediatorCallback
    {
        public event EventHandler<DataReadyEventArgs> DataIsReady;

        public void DataReady(CanIHasPope popeState)
        {
<<<<<<< Updated upstream
            this.DataIsReady?.Invoke(this, new DataReadyEventArgs(popeState));   
=======
            Console.Write("");
>>>>>>> Stashed changes
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
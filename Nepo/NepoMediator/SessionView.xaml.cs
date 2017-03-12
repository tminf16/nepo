using Mediator;
using Nepo.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace NepoMediator
{
    /// <summary>
    /// Interaction logic for SessionView.xaml
    /// </summary>
    public partial class SessionView : UserControl
    {
        private readonly MediatorHost host;

        public RelayCommand ResetCommand { get; set; }
        public SessionView(Instance inst)
        {
            //Session.Get.CurrentInstance = inst;
            ResetCommand = new RelayCommand(Reset);
            InitializeComponent();
            this.host = new MediatorHost();
            Task.Run(() =>
            {
                this.host.Start();
            });
            Task.Run(async () =>
            {
                while (null == MediatorHandler.HandlerInstance || !MediatorHandler.HandlerInstance.Ready)
                    await Task.Delay(100);
                Dispatcher.Invoke(() =>
                {
                    MediatorHandler.HandlerInstance.Instance = inst;
                    this.MyIncredibleMapControl.Configure(MediatorHandler.HandlerInstance.Instance.Map, null);
                    MediatorHandler.HandlerInstance.NewDataAvailable += HandlerInstance_NewDataAvailable;
                });
            });
        }

        private void HandlerInstance_NewDataAvailable(object sender, EventArgs e)
        {
            this.MyIncredibleMapControl.SetSolution(MediatorHandler.HandlerInstance.GetCurrentSolution(new Guid()));
        }

        private void Reset(object obj)
        {
            var handler = MediatorHandler.HandlerInstance;
            handler.Reset();
        }
    }
}

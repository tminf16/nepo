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
using Mediator;
using Nepo.Common;
using System.Threading;

namespace NepoMediator
{
    /// <summary>
    /// Interaktionslogik für MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly MediatorHost host;
        public RelayCommand ResetCommand { get; set; }
        public MainWindow()
        {
            ResetCommand = new RelayCommand(Reset);
            InitializeComponent();
            this.host = new MediatorHost();
            Task.Run(() =>
            {
                this.host.Start();
            });
            Task.Run(()=>
            {
                while (null == MediatorHandler.HandlerInstance)
                    Thread.Sleep(100);
                Dispatcher.Invoke(()=>
                {
                    MyIncredibleMapControl.Configure(MediatorHandler.HandlerInstance.Instance.Map, null);
                    MediatorHandler.HandlerInstance.NewDataAvailable += HandlerInstance_NewDataAvailable;
                    this.SizeToContent = SizeToContent.WidthAndHeight;
                    this.SizeToContent = SizeToContent.Manual;
                });
            });
        }

        private void HandlerInstance_NewDataAvailable(object sender, EventArgs e)
        {
            MyIncredibleMapControl.SetSolution(MediatorHandler.HandlerInstance.GetCurrentSolution(new Guid()));
        }

        private void Reset(object obj)
        {
            var handler = MediatorHandler.HandlerInstance;
            handler.Reset();
        }
    }
}

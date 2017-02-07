using Nepo.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Navigation;

namespace NepoGUI
{
    public class GuiNavigation
    {
        private ContentControl _presenter;
        private InstanceOverview _overview;
        private SessionView _view;
        private static GuiNavigation _instance;
        public static GuiNavigation Get { get { return _instance; } }

        public GuiNavigation(ContentControl navWindow)
        {
            _instance = this;
            _presenter = navWindow;
            _overview = new InstanceOverview();
            _view = new SessionView();
            _presenter.Content = _overview;
        }

        public void SelectInstance(Guid instanceId)
        {
            Instance currentInstance = Session.Get.Instances.SingleOrDefault(x => x.InstanceId.Equals(instanceId));
            if (null == currentInstance)
                return;
            Session.Get.CurrentInstance = currentInstance;
            _view.LoadValues();
            _presenter.Content = _view;
        }
    }
}

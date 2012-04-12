using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.ServiceModel;

namespace Radial.Lock.WinService
{
    partial class WinHostService : ServiceBase
    {
        ServiceHost _host;

        public WinHostService()
        {
            try
            {
                _host = new ServiceHost(typeof(LockService));
                _host.Opened += new EventHandler(_host_Opened);
                _host.Closed += new EventHandler(_host_Closed);
                InitializeComponent();
            }
            catch (Exception e)
            {
                Logger.Default.Fatal(e);
                throw;
            }
        }

        void _host_Closed(object sender, EventArgs e)
        {
            Logger.Default.Info("service is closed");
        }

        void _host_Opened(object sender, EventArgs e)
        {
            Logger.Default.Info("service is started");
        }

        protected override void OnStart(string[] args)
        {
            try
            {
                _host.Open();
            }
            catch (Exception e)
            {
                Logger.Default.Fatal(e);
                throw;
            }
        }

        protected override void OnStop()
        {
            _host.Close();
        }
    }
}

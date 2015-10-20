using System;
using System.Diagnostics;
using System.IO;
using System.ServiceProcess;

namespace Radial.Tools.Srvd
{
    partial class DaemonService : ServiceBase
    {
        Process _cps;

        public DaemonService(string serviceName)
        {
            InitializeComponent();
            this.ServiceName = serviceName;
        }

        protected override void OnStart(string[] args)
        {
            // TODO:  在此处添加代码以启动服务。
            string repath = ServiceHelper.GetParameter<string>(this.ServiceName, "path");
            File.WriteAllText("D:\\ab1.txt", repath);
            string reargs = ServiceHelper.GetParameter<string>(this.ServiceName, "args");
            File.WriteAllText("D:\\ab2.txt", reargs);

            if (string.IsNullOrWhiteSpace(repath))
                return;

            _cps = new Process();

            _cps.StartInfo = new ProcessStartInfo(repath);


            if (!string.IsNullOrWhiteSpace(reargs))
                _cps.StartInfo.Arguments = reargs;

            _cps.EnableRaisingEvents = true;
            _cps.Exited += Process_Exited;
            _cps.Start();

        }

        void Process_Exited(object sender, EventArgs e)
        {
            OnStart(null);
        }

        protected override void OnStop()
        {
            // TODO:  在此处添加代码以执行停止服务所需的关闭操作。
            if (_cps != null)
                _cps.Kill();
        }
    }
}

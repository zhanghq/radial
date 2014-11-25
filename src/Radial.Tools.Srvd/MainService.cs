using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;

namespace Radial.Tools.Srvd
{
    partial class MainService : ServiceBase
    {
        Process _cps;

        public MainService()
        {
            InitializeComponent();
        }

        protected override void OnStart(string[] args)
        {
            // TODO:  在此处添加代码以启动服务。
            _cps = new Process();

            _cps.StartInfo = new ProcessStartInfo
            {
                FileName = Encoding.UTF8.GetString(Convert.FromBase64String(Cmd.Context.ExePath))
            };

            if (!string.IsNullOrWhiteSpace(Cmd.Context.Args))
                _cps.StartInfo.Arguments = Encoding.UTF8.GetString(Convert.FromBase64String(Cmd.Context.Args));

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
            _cps.Kill();
        }
    }
}

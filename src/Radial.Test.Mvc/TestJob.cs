using Quartz;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Radial.Test.Mvc
{
    public class TestJob : IJob 
    {
        public void Execute(IJobExecutionContext context)
        {
            Logger.Default.Info("Test Job, {0}", DateTime.Now);
        }
    }
}
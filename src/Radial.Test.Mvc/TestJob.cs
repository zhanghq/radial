using Quartz;
using System;

namespace Radial.Test.Mvc
{
    public class TestJob : IJob 
    {
        public void Execute(IJobExecutionContext context)
        {
            Logger.Info("Test Job, {0}", DateTime.Now);
        }
    }
}
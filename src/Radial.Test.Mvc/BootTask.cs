using Microsoft.Practices.Unity;
using Quartz;
using Quartz.Impl;
using Radial.Boot;
using Radial.Param;
using Radial.Persist.Nhs.Param;
using Radial.Web.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Radial.Test.Mvc
{
    public class BootTask : IBootTask
    {
        IScheduler scheduler;
        IJobDetail testJob;
        ITrigger testTrigger;

        public void Initialize()
        {
            Dependency.Container.RegisterType<IParam, KvParam>();

            scheduler = (new StdSchedulerFactory()).GetScheduler();
            testJob = JobBuilder.Create<TestJob>().WithIdentity("testJob").Build();
            testTrigger = TriggerBuilder.Create().WithIdentity("testTrigger").StartNow().WithSimpleSchedule(x => x.WithIntervalInMinutes(2).RepeatForever()).Build();
            scheduler.ScheduleJob(testJob, testTrigger);
            scheduler.Start();
        }

        public void Start()
        {
            
        }

        public void Stop()
        {
            //scheduler.Shutdown();
        }
    }
}
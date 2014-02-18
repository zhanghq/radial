using Microsoft.Practices.Unity;
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
        public void Initialize()
        {
            Components.Container.RegisterType<IParam, NhParam>();
        }

        public void Start()
        {
        }

        public void Stop()
        {
        }
    }
}
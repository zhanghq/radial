using System;
using System.Collections.Generic;
$if$ ($targetframeworkversion$ >= 3.5)using System.Linq;$endif$
using System.Text;
using System.Threading.Tasks;
using $safeprojectname$.Infras;

namespace $safeprojectname$.Application
{
    /// <summary>
    /// ServiceHub
    /// </summary>
    public static class ServiceHub
    {
        public static IUserService User
        {
            get
            {
                return DependencyFactory.CreateService<IUserService>();
            }
        }
    }
}

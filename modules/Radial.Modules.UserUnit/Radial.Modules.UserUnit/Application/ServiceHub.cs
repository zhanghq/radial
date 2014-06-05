using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Radial.Modules.UserUnit.Infras;

namespace Radial.Modules.UserUnit.Application
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

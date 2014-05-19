using QuickStart.Infras;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuickStart.Application
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

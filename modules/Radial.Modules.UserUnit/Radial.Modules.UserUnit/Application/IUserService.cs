using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Radial.Modules.UserUnit.Models;

namespace Radial.Modules.UserUnit.Application
{
    /// <summary>
    /// IUserService
    /// </summary>
    public interface IUserService
    {
        User Register(string name, string email,string mobile, string password, string registerIp);
    }
}

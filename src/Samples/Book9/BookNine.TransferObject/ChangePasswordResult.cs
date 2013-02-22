using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookNine.TransferObject
{
    public enum ChangePasswordResult
    {
        UserNotExist = -2,
        OldPasswordError = -1,
        OK = 0
    }
}

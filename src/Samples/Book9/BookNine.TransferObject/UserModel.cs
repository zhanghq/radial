using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookNine.TransferObject
{
    public class UserModel
    {
        public int Id { get; set; }
        public string Mail { get; set; }
        public DateTime RegisterTime { get; set; }
    }
}

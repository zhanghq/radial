using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Radial;
using Radial.Security;

namespace BookNine.Domain.Model
{
    public class User
    {
        public virtual int Id { get; set; }
        public virtual string Mail { get; set; }
        protected virtual string Password { get; set; }
        protected virtual string Salt { get; set; }
        public virtual DateTime RegisterTime { get; set; }

        public virtual void SetPassword(string password)
        {
            Checker.Requires(!string.IsNullOrWhiteSpace(password), "密码不能为空");

            if (string.IsNullOrWhiteSpace(Salt))
                Salt = RandomCode.Create(5);

            Password = CryptoProvider.SHA1Encrypt(password + Salt);
        }


        public virtual bool VerifyPassword(string password)
        {
            Checker.Requires(!string.IsNullOrWhiteSpace(password), "密码不能为空");

            return string.Compare(Password, CryptoProvider.SHA1Encrypt(password + Salt), true) == 0;
        }
    }
}

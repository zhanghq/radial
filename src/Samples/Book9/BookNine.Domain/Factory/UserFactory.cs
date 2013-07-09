using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BookNine.Domain.Model;
using BookNine.Domain.Repository;
using Radial;
using Radial.Persist;

namespace BookNine.Domain.Factory
{
    public sealed class UserFactory : FactoryBase
    {
        public UserFactory(IUnitOfWorkEssential uow)
            : base(uow)
        { }

        public User CreateUser(string mail, string password)
        {
            Checker.Requires(!string.IsNullOrWhiteSpace(mail), "邮箱不能为空");
            Checker.Requires(!string.IsNullOrWhiteSpace(password), "密码不能为空");

            mail = mail.Trim();

            IUserRepository usrRepo = CreateRepository<IUserRepository>();

            Checker.Requires(!usrRepo.Exist(o => o.Mail == mail), "邮箱已存在");

            User u = new User { Mail = mail };

            u.SetPassword(password);

            return u;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Radial.Persist;
using Microsoft.Practices.Unity;

namespace Radial.Modules.UserUnit
{
    /// <summary>
    /// UserService.
    /// </summary>
    public abstract class UserServiceBase
    {

        /// <summary>
        /// Resolves the unit of work.
        /// </summary>
        /// <returns></returns>
        protected virtual IUnitOfWork ResolveUnitOfWork()
        {
            return Components.Container.Resolve<IUnitOfWork>();
        }

        /// <summary>
        /// Registers a new user.
        /// </summary>
        /// <param name="name">The user name.</param>
        /// <param name="email">The email.</param>
        /// <param name="mobile">The mobile.</param>
        /// <param name="password">The password.</param>
        /// <param name="registerIp">The register ip.</param>
        /// <returns></returns>
        public virtual RegisterResult Register(string name, string email, string mobile, string password, string registerIp)
        {
            Checker.Parameter(!string.IsNullOrWhiteSpace(name), "user name can not be empty or null");
            Checker.Parameter(!string.IsNullOrWhiteSpace(password), "password can not be empty or null");

            name = name.Trim();

            if (!string.IsNullOrWhiteSpace(email))
                email = email.Trim();
            if (!string.IsNullOrWhiteSpace(mobile))
                mobile = mobile.Trim();
            if (!string.IsNullOrWhiteSpace(registerIp))
                registerIp = registerIp.Trim();

            using (var uow = ResolveUnitOfWork())
            {
                var repo = new UserRepository(uow);

                if (repo.Exist(o => o.Name == name))
                {
                    if (Validator.IsEmail(name))
                        return new RegisterResult { Code = RegisterResultCode.DuplicatedEmail };
                    if (Validator.IsMobile(name))
                        return new RegisterResult { Code = RegisterResultCode.DuplicatedMobile };

                    return new RegisterResult { Code = RegisterResultCode.DuplicatedName };
                }

                User obj = new User
                {
                    Name = name,
                    Email = email,
                    Mobile = mobile,
                    RegisterIp = registerIp,
                    RegisterTime = DateTime.Now
                };

                obj.SetPassword(password);

                uow.RegisterNew<User>(obj);

                uow.Commit();

                return new RegisterResult { Code = RegisterResultCode.OK, User = obj };
            }
        }


        /// <summary>
        /// Deletes the user.
        /// </summary>
        /// <param name="uid">The user id.</param>
        public virtual void Delete(string uid)
        {
            Checker.Parameter(!string.IsNullOrWhiteSpace(uid), "user id can not be empty or null");
            using (var uow = ResolveUnitOfWork())
            {
                var repo = new UserRepository(uow);

                User u = repo[uid];

                if(u!=null)
                {
                    u.IsDelete = true;

                    uow.RegisterUpdate<User>(u);

                    uow.Commit();
                }
            }
        }


        /// <summary>
        /// Gets the specified user.
        /// </summary>
        /// <param name="uid">The user id.</param>
        /// <returns></returns>
        public virtual User Get(string uid)
        {
            Checker.Parameter(!string.IsNullOrWhiteSpace(uid), "user id can not be empty or null");
            using (var uow = ResolveUnitOfWork())
            {
                var repo = new UserRepository(uow);

                return repo[uid];
            }
        }
    }
}

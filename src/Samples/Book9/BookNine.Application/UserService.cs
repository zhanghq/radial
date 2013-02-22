using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using BookNine.Domain.Factory;
using BookNine.Domain.Model;
using BookNine.Domain.Repository;
using BookNine.Infrastructure;
using BookNine.TransferObject;
using Radial.Persist;

namespace BookNine.Application
{
    public static class UserService
    {
        public static UserModel Create(string mail, string password)
        {
            using (IUnitOfWork uow = DependencyResolver.CreateUnitOfWork())
            {
                UserFactory userFactory = new UserFactory(uow);

                User u=userFactory.CreateUser(mail, password);

                uow.RegisterNew<User>(u);

                uow.Commit();

                return Mapper.Map<User, UserModel>(u);
            }
        }

        public static ChangePasswordResult ChangePassword(int userId, string oldPassword, string newPassword)
        {
            using (IUnitOfWork uow = DependencyResolver.CreateUnitOfWork())
            {
                IUserRepository userRepo = DependencyResolver.CreateRepository<IUserRepository>(uow);

                User u = userRepo.Find(userId);

                if (u == null)
                    return ChangePasswordResult.UserNotExist;

                if (!u.VerifyPassword(oldPassword))
                    return ChangePasswordResult.OldPasswordError;

                u.SetPassword(newPassword);

                uow.RegisterSave<User>(u);

                uow.Commit();

                return ChangePasswordResult.OK;
            }
        }
    }
}

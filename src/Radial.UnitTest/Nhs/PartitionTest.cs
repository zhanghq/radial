﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using Radial.Data.Nhs;
using Radial.UnitTest.Nhs.PoolInitializer;
using NHibernate;
using Radial.UnitTest.Nhs.Domain;
using Radial.UnitTest.Nhs.Repository;
using System.Threading.Tasks;
using Radial.Data;
using Autofac;

namespace Radial.UnitTest.Nhs
{
    [TestFixture]
    public class PartitionTest
    {
        [SetUp]
        public void SetUp()
        {
            Components.AdditionalRegister = o => o.RegisterType<PartitionFactoryPoolInitializer>().As<IFactoryPoolInitializer>();

            CleanUp();

        }

        [TearDown]
        public void TearDown()
        {
            CleanUp();
        }

        public void CleanUp()
        {
            foreach (string alias in StorageRouter.GetStorageAliases<User>())
            {
                using (IUnitOfWork uow = new NhUnitOfWork(alias))
                {
                    uow.RegisterClear<User>();

                    uow.Commit(true);
                }
            }
        }

        [Test]
        public void Save()
        {
            using (IUnitOfWork uow = new NhUnitOfWork("partition1"))
            {
                uow.RegisterNew(new User { Id = 1, Name = "测试" });
                uow.Commit(true);
            }

            using (IUnitOfWork uow = new NhUnitOfWork("partition2"))
            {
                uow.RegisterNew(new User { Id = 2, Name = "测试" });
                uow.Commit(true);
            }
        }

        [Test]
        public void StorageAlias()
        {
            Assert.AreEqual("partition1", StorageRouter.GetStorageAlias<User>(1));
            Assert.AreEqual("partition2", StorageRouter.GetStorageAlias<User>(2));
        }

        [Test]
        public void Select()
        {
            
            IDictionary<string, IList<User>> ugroup = new Dictionary<string, IList<User>>();

            for (int i = 1; i <= 100; i++)
            {
                var u = new User { Id = i, Name = "测试" + i };

                string alias = StorageRouter.GetStorageAlias<User>(u.Id);

                if (!ugroup.ContainsKey(alias))
                    ugroup[alias] = new List<User>();

                ugroup[alias].Add(u);
            }

            //insert
            foreach (string alias in StorageRouter.GetStorageAliases<User>())
            {
                using (IUnitOfWork uow = new NhUnitOfWork(alias))
                {
                    uow.RegisterNew<User>(ugroup[alias]);

                    uow.Commit(true);
                }
            }



            List<User> selectlist = new List<User>();

            Parallel.ForEach<string>(SessionFactoryPool.GetStorageAliases(), alias =>
            {
                using (IUnitOfWork uow = new NhUnitOfWork(alias))
                {
                    UserRepository userRepository = new UserRepository(uow);

                    selectlist.AddRange(userRepository.FindAll());
                }
            });

            selectlist.Sort((x, y) =>
            {
                if (x.Id > y.Id)
                    return 1;
                else
                    if (x.Id < y.Id)
                        return -1;
                return 0;
            });

            foreach (User u in selectlist)
            {
                Console.WriteLine("id:{0},name:{1}", u.Id, u.Name);
            }
        }
    }
}

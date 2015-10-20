using NUnit.Framework;
using Radial.Persist;
using Radial.Persist.Nhs;
using Radial.UnitTest.Persist.Nhs.Domain;
using Radial.UnitTest.Persist.Nhs.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Practices.Unity;

namespace Radial.UnitTest.Persist.Nhs.Shard
{
    [TestFixture]
    public class ShardTest
    {
        [TestFixtureSetUp]
        public void SetUp()
        {
            Dependency.Container.RegisterType<IFactoryPoolInitializer, ShardPoolInitializer>();
            Dependency.Container.RegisterType<IStoragePolicy, ShardStoragePolicy>();
            Dependency.Container.RegisterType<ITableShardable, ShardStoragePolicy>();
        }

        [Test]
        public void Test1()
        {
            /*
             * 随机生成20个Book
             *如果Book对象Id的最后一位为偶数，则它将会被保存在表Book1中，如果为奇数，则保存在表Book2中
             *然后再把两个表中的数据查询出来
             */

            List<Book> books = new List<Book>();

            for (int i = 1; i <= 20; i++)
            {
                books.Add(new Book
                {
                    Id = Guid.NewGuid().ToString().ToUpper().Substring(0, 8) + i.ToString("D2"),
                    Name = "Book" + i.ToString("D2")
                });
            }

            //Book支持的存储别名
            string[] aliases = StorageRouter.GetTypeAliases<Book>();

            //保存
            foreach (var alias in aliases)
            {
                using (var uow = new NhUnitOfWork(alias))
                {
                    //批量保存与当前alias相匹配的books
                    uow.RegisterNew<Book>(books.Where(o => StorageRouter.GetStorageAlias<Book>(o.Id) == alias).ToArray());
                    uow.Commit();
                }

            }

            //查询
            List<Book> nBooks = new List<Book>();

            Parallel.ForEach<string>(aliases, alias =>
            {
                using (var uow = new NhUnitOfWork(alias))
                {
                    BookRepository repo = new BookRepository(uow);
                    nBooks.AddRange(repo.FindAll());
                }
            });

            foreach (var o in nBooks.OrderBy(o => o.Name))
            {
                Console.WriteLine("id:{0},name:{1}", o.Id, o.Name);
            }


            //清理
            Parallel.ForEach<string>(aliases, alias =>
            {
                using (var uow = new NhUnitOfWork(alias))
                {
                    uow.RegisterClear<Book>();

                    uow.Commit();
                }
            });
        }
    }
}

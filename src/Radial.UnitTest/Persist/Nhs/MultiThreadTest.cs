using NUnit.Framework;
using Radial.Persist;
using Radial.Persist.Nhs;
using Radial.UnitTest.Persist.Nhs.Domain;
using System.Threading;

namespace Radial.UnitTest.Persist.Nhs
{
    [TestFixture]
    public class MultiThreadTest
    {
        [Test]
        public void Insert()
        {
            for (int i = 0; i < 3; i++)
            {
                Thread t = new Thread(InsertThread);
                t.IsBackground = true;
                t.Start();
            }

            Thread.CurrentThread.Join(10 * 1000);
        }

        private void InsertThread()
        {
            using (IUnitOfWork uow = new NhUnitOfWork())
            {
                User u = new User
                {
                    Id = RandomCode.NewInstance.Next(10000, int.MaxValue),
                    Name = Thread.CurrentThread.ManagedThreadId + "-mt-text2"
                };
                uow.RegisterNew<User>(u);
                uow.Commit();
            }
        }
    }
}
